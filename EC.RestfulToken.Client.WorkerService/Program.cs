
using EC.RestfulToken.Client.WorkerService.Services;

namespace EC.RestfulToken.Client.WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddMemoryCache();
            builder.Services.AddHostedService<Worker>();
            builder.Services.AddHttpClient<IHttpService, HttpService>();
            builder.Services.AddSingleton<IRestfulTokenServerService, RestfulTokenServerService>();


            var host = builder.Build();
            host.Run();
        }
    }
}