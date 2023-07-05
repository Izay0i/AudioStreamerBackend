using AudioStreamerAPI.DTO;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace AudioStreamerAPI.Helpers
{
    public class SpeechRecognitionHelper
    {
        public static async Task<IEnumerable<CaptionItemDTO>> FromWaveFile(SpeechConfig speechConfig, string filePath)
        {
            using var audioConfigStream = AudioInputStream.CreatePushStream();
            using var audioConfig = AudioConfig.FromWavFileInput(filePath);
            using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);
            var stopRecognition = new TaskCompletionSource<int>();

            List<CaptionItemDTO> results = new();

            speechRecognizer.Recognized += (s, e) =>
            {
                if (e.Result.Reason == ResultReason.RecognizedSpeech)
                {
                    if (e.Result.Text.Length > 0)
                    {
                        var captionItem = new CaptionItemDTO
                        {
                            Timestamp = e.Result.OffsetInTicks / 10_000_000.0,
                            Duration = e.Result.Duration.TotalSeconds,
                            Message = e.Result.Text,
                        };
                        results.Add(captionItem);
                    }
                }
            };

            speechRecognizer.Canceled += (s, e) =>
            {
                Console.WriteLine($"CANCELED: Reason={e.Reason}");

                if (e.Reason == CancellationReason.Error)
                {
                    Console.WriteLine($"CANCELED: ErrorCode={e.ErrorCode}");
                    Console.WriteLine($"CANCELED: ErrorDetails={e.ErrorDetails}");
                    Console.WriteLine($"CANCELED: Did you set the speech resource key and region values?");

                    var captionItem = new CaptionItemDTO
                    {
                        Timestamp = 0.0,
                        Duration = 999.0,
                        Message = $"Error code: {e.ErrorCode}\tDetails: {e.ErrorDetails}",
                    };
                    results.Add(captionItem);
                }

                stopRecognition.TrySetResult(0);
            };

            await speechRecognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);
            // Waits for completion. Use Task.WaitAny to keep the task rooted.
            Task.WaitAny(new[] { stopRecognition.Task });
            // Make the following call at some point to stop recognition:
            await speechRecognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);

            return results;
        }
    }
}
