using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVCWebApp.Domain;
using MVCWebApp.Domain.Repositories.Abstract;
using MVCWebApp.Domain.Repositories.EntityFramework;
using MVCWebApp.Service;


namespace MVCWebApp
{
    public class Startup
    {
        public IConfiguration Configuration { get;  }

        public Startup(IConfiguration configuration) => Configuration = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // ���������� ������ �� appsettings.json
            Configuration.Bind("Project", new Config());

            // ���������� ������ ���������� ���������� � �������� ��������
            services.AddTransient<ITextFieldsRepository, EFTextFieldsRepository>();
            services.AddTransient<IServiceItemsRepository, EFServiceItemsRepository>();
            services.AddTransient<DataManager>();

            // ���������� ��
            services.AddDbContext<AppDbContext>(x => x.UseSqlServer(Config.ConnectionString));

            // ��������� indentity �������
            services.AddIdentity<IdentityUser, IdentityRole>(opts =>
            {
                opts.User.RequireUniqueEmail = true;
                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            // ��������� authentication cookie
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "myCompanyAuth";
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/account/login";
                options.AccessDeniedPath = "/account/accessdenied";
                options.SlidingExpiration = true;
            });

            // ������������ �������� ����������� ��� Admin area
            services.AddAuthorization(x =>
            {
                x.AddPolicy("AdminArea", policy => { policy.RequireRole("admin"); });
            });


            // ��������� ��������� ������������ � ������������� (mvc)
            services.AddControllersWithViews(x =>
            {
                x.Conventions.Add(new AdminAreaAuthorization("Admin", "AdminArea"));
            })
            // ���������� ������������� � asp.net core 3.0
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddSessionStateTempDataProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // !�����������: ������� �����������

            // ���� �� � ��������� ���������� (������) �.�., ��������, � ����� ��� ����������
            // ��. �������: � �������� ���������� ��� ����� ������ ��������� ���������� �� �������
            if (env.IsDevelopment())    
            {
                app.UseDeveloperExceptionPage();    // ����� �������� ��� ������������� �� ������� (������)
            }

            // ���������� ��������� ��������� ������ � ���������� (css, js � �.�. �� wwwroot)
            app.UseStaticFiles();

            app.UseRouting(); // ����������� ������� ������������� (������)

            // ���������� �������������� � �����������
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            // ������������ �������� (������)
            // ��. �������: ������������ ������ ��� �������� (endpoints)
            app.UseEndpoints(endpoints =>   
            {
                endpoints.MapControllerRoute("admin", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
