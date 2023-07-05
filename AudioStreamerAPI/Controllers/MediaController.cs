using AudioStreamerAPI.Attributes;
using AudioStreamerAPI.Constants;
using AudioStreamerAPI.Helpers;
using Azure.Storage;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CognitiveServices.Speech;

namespace AudioStreamerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediaController : ControllerBase
    {
        [HttpGet("transcribe")]
        public async Task<IActionResult> TranscribeFromWaveFile(string src, string lang = "en-US")
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var connectionString = config.GetSection("Azure")["URL"];

            const string containerName = "media";
            var storageUri = config.GetSection("Azure")["StorageURI"];
            var blobName = FileHelper.GetBlobFromUri(storageUri, src, containerName);

            BlobServiceClient blobServiceClient = new(connectionString);
            BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blob = blobContainerClient.GetBlobClient(blobName);

            var speechKey = config.GetSection("Azure")["SpeechKey"];
            var speechRegion = config.GetSection("Azure")["SpeechRegion"];
            var tempWaveFilePath = await FileHelper.CreateTempWaveFilePath(blob);

            var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
            speechConfig.SpeechRecognitionLanguage= lang;
            speechConfig.EnableDictation();

            var result = await SpeechRecognitionHelper.FromWaveFile(speechConfig, tempWaveFilePath);
            return Ok(new OperationalStatus
            {
                StatusCode = OperationalStatusEnums.Ok,
                Message = "Transcribing complete.",
                Objects = new object[] { result },
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetMedia(string src, string containerName, string contentType)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var connectionString = config.GetSection("Azure")["URL"];
            
            var storageUri = config.GetSection("Azure")["StorageURI"];
            var blobName = FileHelper.GetBlobFromUri(storageUri, src, containerName);

            BlobServiceClient blobServiceClient = new(connectionString);
            BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blob = blobContainerClient.GetBlobClient(blobName);

            var options = new StorageTransferOptions
            {
                InitialTransferSize = AzureConstants.INITIAL_TRANSFER_SIZE,
                MaximumConcurrency = AzureConstants.MAX_CONCURRENCY,
                MaximumTransferSize = AzureConstants.MAX_TRANSFER_SIZE,
            };

            var stream = new MemoryStream();
            await blob.DownloadToAsync(stream, null, options);
            stream.Position = 0;
            return File(stream, contentType, true);
        }

        [HttpPost("upload")]
        [RequestSizeLimit(AzureConstants.MAX_FILE_SIZE)]
        [RequestFormLimits(MultipartBodyLengthLimit = AzureConstants.MAX_FILE_SIZE)]
        public async Task<IActionResult> UploadChunksAsync(
            int id, 
            [AllowedExtensions(new string[] { ".webm", ".ogg", ".mp3", ".wav", ".webp", ".jpeg", ".jpg", ".png", ".gif" })] IFormFile file, 
            string containerName)
        {
            try
            {
                var uri = await MediaHelper.UploadChunksAsync(id, file, containerName);
                return Ok(new OperationalStatus
                {
                    StatusCode = OperationalStatusEnums.Ok,
                    Objects = new object[] { uri },
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new OperationalStatus
                {
                    StatusCode = OperationalStatusEnums.BadRequest,
                    Message = ex.Message,
                });
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteMedia(string url, string containerName)
        {
            try
            {
                var result = await MediaHelper.DeleteMediaAsync(url, containerName);
                return StatusCode((int)result.StatusCode, result);
            }
            catch  (Exception ex) 
            {
                return BadRequest(new OperationalStatus
                {
                    StatusCode = OperationalStatusEnums.BadRequest,
                    Message = ex.Message,
                });
            }
        }
    }
}
