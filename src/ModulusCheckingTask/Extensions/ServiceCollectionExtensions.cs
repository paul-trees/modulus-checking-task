using System;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ModulusCheckingTask.Core.Adapters;
using ModulusCheckingTask.Core.Repositories;
using ModulusCheckingTask.Core.Services;
using ModulusCheckingTask.Core.Strategies;
using ModulusCheckingTask.Infrastructure.DbContexts;
using ModulusCheckingTask.Infrastructure.FileSystem;
using ModulusCheckingTask.Infrastructure.Mappers;
using ModulusCheckingTask.Infrastructure.Repositories;
using ModulusCheckingTask.Infrastructure.Seeders;
using Swashbuckle.AspNetCore.Swagger;

namespace ModulusCheckingTask.App.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddModulusChecking(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // Add Database Context
            services.AddDbContext<ModulusCheckingContext>(options => options.UseInMemoryDatabase("ModulusWeightsDb"));

            // Add Adapters
            services.AddScoped<IModulusWeightEntityAdapter, ModulusWeightEntityAdapter>();

            // Add Services
            services.AddScoped<IAccountDetailsValidationService, AccountDetailsValidationService>();
            services.AddScoped<IModulusCheckingService, ModulusCheckingService>();
            services.AddScoped<IModulusWeightMultiplierService, ModulusWeightMultiplierService>();

            // Add Strategies
            services.AddScoped<IModulusCheckStrategy, DoubleAlternateModulusCheckStrategy>();
            services.AddScoped<IModulusCheckStrategy, Standard10ModulusCheckStrategy>();
            services.AddScoped<IModulusCheckStrategy, Standard11ModulusCheckStrategy>();

            // Add Repositories
            services.AddScoped<IModulusWeightRepository, ModulusWeightRepository>();                       

            // Add Mappers
            services.AddSingleton<IModulusWeightEntityMapper, ModulusWeightEntityMapper>();

            // Add Seeders
            services.AddTransient<IModulusCheckingEntitySeeder, ModulusCheckingEntitySeeder>();

            // Add Utilities
            services.AddSingleton<IFileSystemAccess, FileSystemAccess>();

            return services;
        }

        public static IServiceCollection AddConfiguredSwagger(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {                    
                    Title = "Modulus Checking Task API",
                    Version = "v1",
                    Description = "Modulus Checking Technical Test for Mann Island.",
                    Contact = new Contact
                    {
                        Name = "Paul Trees",
                        Email = "paul.trees@yahoo.co.uk"
                    }                    
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });            

            return services;
        }
    }
}
