using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModulusCheckingTask.App.Extensions;
using ModulusCheckingTask.App.Infrastructure.Middleware;
using ModulusCheckingTask.Infrastructure.Seeders;

namespace ModulusCheckingTask.App
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddConfiguredSwagger();            
            services.AddModulusChecking();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IModulusCheckingEntitySeeder modulusCheckingEntitySeeder)
        {
            app.UseMiddleware<UnhandledExceptionCatchingMiddleware>();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMvc();

            modulusCheckingEntitySeeder.Execute();
        }
    }
}
