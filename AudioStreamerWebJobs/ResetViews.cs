using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace AudioStreamerWebJobs
{
    public class ResetViews
    {
        [Singleton]
        public static void TimerTick([TimerTrigger("0 0 0 * * *")] TimerInfo _, ILogger logger)
        {
            using (var client = new HttpClient())
            {
                var uri = "https://audiostreamer.azurewebsites.net/api/track/views/reset";
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), uri);
                var response = client.SendAsync(request).Result;
                //Console.WriteLine($"{response}");
                logger.LogInformation($"{response}");
            }
        }
    }
}
