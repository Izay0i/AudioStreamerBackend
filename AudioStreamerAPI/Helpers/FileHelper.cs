using Azure.Storage.Blobs;
using NAudio.Wave;
using System.Text.RegularExpressions;

namespace AudioStreamerAPI.Helpers
{
    public class FileHelper
    {
        public static string SanitizeFileName(string fileName)
        {
            //Hope i didn't miss any, cause that would be painful :D
            //In order (from left to right): \ / ( ) [ ] # ' & , | { } ; : " < > ? ! @ $ % ^ * = + ~ .
            // \ counts as a \ in verbatim string
            fileName = Regex.Replace(fileName, @"[\\\/\(\)\[\]\#\'\&\,\|\{\}\;\:\""\<\>\?\!\@\$\%\^\*\=\+\~\. ]+", "_");
            return string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));
        }

        public static string GenerateFileName(string fileName, string directory)
        {
            try
            {
                string fName = string.Empty;
                string[] strName = fileName.Split(".");

                fName = string.Format("{0}_{1}_{2}.{3}", 
                    directory, 
                    SanitizeFileName(strName[0]), 
                    DateTime.Now.ToUniversalTime().ToString("yyyyMMdd\\THHmmssfff"), 
                    strName[strName.Length - 1]);
                return fName;
            }
            catch
            {
                return fileName;
            }
        }

        //I am a menace to society
        public static string GetBlobFromUri(string storageUri, string fileUri, string containerName)
        {
            var relativeUri = new Uri(storageUri).MakeRelativeUri(new Uri(fileUri)).ToString();
            var blob = relativeUri.Split($"{containerName}/");
            return blob[^1];
        }

        public static async Task<string> CreateTempWaveFilePath(BlobClient blobClient)
        {
            using var stream = new MemoryStream();
            await blobClient.DownloadToAsync(stream);
            stream.Seek(0, SeekOrigin.Begin);

            var tempFileName = Path.GetTempFileName();
            var tempFilePath = Path.GetTempPath();
            var tempFileDirectory = Path.Combine(tempFilePath, tempFileName);
            using (var fs = new FileStream(tempFileDirectory, FileMode.OpenOrCreate))
            {
                stream.CopyTo(fs);
            }

            const string tempWaveFile = "TEMP.wav";
            var tempWaveDirectory = Path.Combine(tempFilePath, tempWaveFile);
            var waveFormat = new WaveFormat(16000, 16, 1);
            using (var resampler = new MediaFoundationResampler(new AudioFileReader(tempFileName), waveFormat))
            {
                //Highest quality
                resampler.ResamplerQuality = 60;
                WaveFileWriter.CreateWaveFile(tempWaveDirectory, resampler);
            }

            return tempWaveDirectory;
        }
    }
}
