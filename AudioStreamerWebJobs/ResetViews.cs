using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace AudioStreamerWebJobs
{
    public class ResetViews
    {
        [Singleton]
        public static async Task TimerTick([TimerTrigger("0 0 0 * * *", RunOnStartup = true)] TimerInfo _, ILogger logger)
        {
            logger.LogInformation("Starting job...");
            using (var client = new HttpClient())
            {
                var uri = "https://audiostreamer.azurewebsites.net/api/track/views/reset";
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), uri);
                var response = await client.SendAsync(request);
                using (var content = response.Content)
                {
                    var result = await content.ReadAsStringAsync();
                    logger.LogInformation($"{result}");
                }
            }
            logger.LogInformation("Ending job...");
        }
    }
}
