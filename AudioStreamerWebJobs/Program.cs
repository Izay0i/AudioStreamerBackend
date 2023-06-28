using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AudioStreamerWebJobs
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    internal class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            var builder = new HostBuilder().ConfigureWebJobs(webJobs =>
            {
                webJobs.AddTimers();
                webJobs.AddAzureStorageBlobs();
            }).ConfigureServices(services =>
            {
                services.AddTransient<Functions>();
                services.AddTransient<ResetViews>();
            }).Build();

            builder.Run();
        }
    }
}
