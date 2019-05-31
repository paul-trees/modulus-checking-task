using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ModulusCheckingTask.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseHttpSys()
                .UseUrls("http://localhost:5032")
                .UseStartup<Startup>();
    }
}
