using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVCWebApp.Domain.Entities;

namespace MVCWebApp.Domain
{
    public class AppDbContext : IdentityDbContext<IdentityUser> //наследуемся от системного класса IdentityDbContext с ситемным польщователем IdentityUser
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // два класса наших TextField и ServiceItem проецируем на БД
        public DbSet<TextField> TextFields { get; set; }
        public DbSet<ServiceItem> ServiceItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // заполняем БД значениями по умолчанию

            // создаем роль для пользователей - будут на сайте админы
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "44546e06-8719-4ad8-b88a-f271ae9d6eab",    // вручную сгенерированный гуид
                Name = "admin",
                NormalizedName = "ADMIN"
            });

            // сам IdentityUser (один на момент создания БД) - сам админ
            modelBuilder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = "3b62472e-4f66-49fa-a20f-e7685b9565d8",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "my@email.com",
                NormalizedEmail = "MY@EMAIL.COM",
                EmailConfirmed = true,  // пароль сразу подтвержден, чтобы он мог входить от имени админа
                PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "superpassword"),  // хэш от слова (пароля) superpassword
                SecurityStamp = string.Empty
            });

            // промежуточная таблица, по которой связываем админа с его ролью
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = "44546e06-8719-4ad8-b88a-f271ae9d6eab",
                UserId = "3b62472e-4f66-49fa-a20f-e7685b9565d8"
            });

            // далее 3 объекта в БД - наши текстовые поля

            modelBuilder.Entity<TextField>().HasData(new TextField
            {
                Id = new Guid("63dc8fa6-07ae-4391-8916-e057f71239ce"),
                CodeWord = "PageIndex",
                Title = "Главная"
            });
            modelBuilder.Entity<TextField>().HasData(new TextField
            {
                Id = new Guid("70bf165a-700a-4156-91c0-e83fce0a277f"),
                CodeWord = "PageServices",
                Title = "Наши услуги"
            });
            modelBuilder.Entity<TextField>().HasData(new TextField
            {
                Id = new Guid("4aa76a4c-c59d-409a-84c1-06e6487a137a"),
                CodeWord = "PageContacts",
                Title = "Контакты"
            });
        }
    }
}
