using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonsHandBook.Domain.Interfaces;
using PersonsHandBook.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using PersonsHandBook.Infrastructure.Repository;
using Microsoft.Extensions.Localization;
using PersonsHandBook.Resources.Locallizer;
using PersonsHandBook.Domain.Models;

namespace PersonsHandBook.Services
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DatabaseConnection"));
            });

            services.AddScoped<Func<AppDbContext>>((provider) => provider.GetService<AppDbContext>);
            services.AddScoped<DbFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;

        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddScoped<IContactRepository, ContactRepository>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddLocalization();

            services.AddAutoMapper(typeof(InfrastructureExtensions));
            services.AddScoped<HandBookManager>();
            services.AddScoped<ValidationActionFilterAttribute>();

            return services;
        }


        public static IApplicationBuilder AddLocalizationMiddleware(this IApplicationBuilder app)
        {
            var supportedCultures = new[]
            {
                new CultureInfo("en"),
                new CultureInfo("ka")
            };

            var requestLocalizationOptions = new RequestLocalizationOptions()
            {
                DefaultRequestCulture = new RequestCulture("ka"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };

            app.UseRequestLocalization(requestLocalizationOptions);

            return app;
        }

    }


    public static class HelpherExtensions
    {
        public static int CalculateAge(this DateTime theDateTime)
        {
            var age = DateTime.Today.Year - theDateTime.Year;

            if (theDateTime.AddYears(age) > DateTime.Today)
                age--;

            return age;
        }
    }

    public class MinAge : ValidationAttribute
    {
        private readonly int _limit;
        public MinAge(int limit)
        {
            _limit = limit;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var bday = DateTime.Parse(value.ToString());
            var today = DateTime.Today;
            var age = today.Year - bday.Year;

            if(bday > today.AddYears(-age))
                age--;

            if(age < _limit)
            {
                var result = new ValidationResult($"Minimum Age is {_limit}");
                return result;
            }

            return null;
        }
    }

    public class NameValidation : ValidationAttribute
    {
        private readonly int _minLength;
        private readonly int _maxLength;
        public NameValidation(int minLnegth, int maxLength)
        {
            _minLength = minLnegth;
            _maxLength = maxLength;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string name = (string)value;

            if(name.Length < _minLength || name.Length > _maxLength)
            {
                return new ValidationResult(Constants.NameLength);
            }


            if (name.Any(x => (x >= 'A' && x <= 'Z') || (x >= 'a' && x <= 'z')) && name.Any(k => k >= 'ა' && k <= 'ჰ'))
            {
                return new ValidationResult(Constants.OneLanguageLetters);
            }

            return null;

        }

    }
}
