using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCWebApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // добавляем поддкржку контроллеров и представлений (mvc)
            services.AddControllersWithViews()
                // выставляем совместимость с asp.net core 3.0
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0).AddSessionStateTempDataProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // если мы в окружении разработки (дефолт) т.е., например, в проде это недоступно
            // др. слвоами: в процессе разработки нам важно видеть подробную информацию об ошибках
            if (env.IsDevelopment())    
            {
                app.UseDeveloperExceptionPage();    // хотим получать все представления об ошибках (дефолт)
            }

            app.UseRouting(); // добавляется система маршрутизации (дефолт)

            // подключаем поддержку статичных файлов в приложении (css, js и т.п. из wwwroot)
            app.UseStaticFiles();

            // используются маршруты (дефолт)
            // др. словами: регистрируем нужные нам маршруты (endpoints)
            app.UseEndpoints(endpoints =>   
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
