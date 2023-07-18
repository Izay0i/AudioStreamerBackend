using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AudioStreamerWebJobs
{
    public class Disrupt
    {
        [Singleton]
        public static void TimerTick([TimerTrigger("0 */5 * * * *")] TimerInfo _, ILogger logger)
        {
            Console.WriteLine("5 minutes have passed, waking up...");
        } 
    }
}
