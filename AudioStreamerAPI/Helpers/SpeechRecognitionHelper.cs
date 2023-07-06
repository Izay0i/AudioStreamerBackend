using AudioStreamerAPI.DTO;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace AudioStreamerAPI.Helpers
{
    public class SpeechRecognitionHelper
    {
        public static async Task FromWaveFile(SpeechConfig speechConfig, string filePath, Action<CaptionItemDTO> caption)
        {
            using var audioConfigStream = AudioInputStream.CreatePushStream();
            using var audioConfig = AudioConfig.FromWavFileInput(filePath);
            using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);
            var stopRecognition = new TaskCompletionSource<int>();

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
                        caption(captionItem);
                    }
                }
            };

            speechRecognizer.Canceled += (s, e) =>
            {
                Console.WriteLine($"CANCELED: Reason={e.Reason}");

                if (e.Reason == CancellationReason.Error)
                {
                    var captionItem = new CaptionItemDTO
                    {
                        Timestamp = 0.0,
                        Duration = 999.0,
                        Message = $"Error code: {e.ErrorCode}\tDetails: {e.ErrorDetails}",
                    };
                    caption(captionItem);
                }

                stopRecognition.TrySetResult(0);
            };

            await speechRecognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);
            // Waits for completion. Use Task.WaitAny to keep the task rooted.
            Task.WaitAny(new[] { stopRecognition.Task });
            // Make the following call at some point to stop recognition:
            await speechRecognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
        }
    }
}
