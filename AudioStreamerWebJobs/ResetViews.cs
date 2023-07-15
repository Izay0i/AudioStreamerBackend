using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AudioStreamerWebJobs
{
    public class ResetViews
    {
        [Singleton]
        public static async Task TimerTick([TimerTrigger("0 0 0 * * *")] TimerInfo _, ILogger logger)
        {
            logger.LogInformation("Starting job...");
            Console.WriteLine("Starting job...");
            using (var client = new HttpClient())
            {
                var uri = "https://audiostreamer.azurewebsites.net/api/track/views/reset";
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), uri);
                var response = await client.SendAsync(request);
                var result = await response.Content.ReadAsStringAsync();
                logger.LogInformation($"{result}");
                Console.WriteLine($"{result}");
            }
            logger.LogInformation("Ending job...");
            Console.WriteLine("Ending job...");
        }
    }
}
