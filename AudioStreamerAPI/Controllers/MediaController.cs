using AudioStreamerAPI.Constants;
using AudioStreamerAPI.Helpers;
using Azure.Storage;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;

namespace AudioStreamerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MediaController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MediaController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> GetMedia(string src, string containerName, string contentType)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var connectionString = config.GetSection("Azure")["URL"];
            
            var storageUri = config.GetSection("Azure")["StorageURI"];
            var relativeUri = FileHelper.GetBlobFromUri(storageUri, src, containerName);

            BlobServiceClient blobServiceClient = new(connectionString);
            BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blob = blobContainerClient.GetBlobClient(relativeUri);

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

        [RequestSizeLimit(AzureConstants.MAX_FILE_SIZE)]
        [RequestFormLimits(MultipartBodyLengthLimit = AzureConstants.MAX_FILE_SIZE)]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadChunksAsync(int id, IFormFile file, string containerName)
        {
            try
            {
                var uri = await MediaHelper.UploadChunksAsync(id, file, containerName);
                return Ok(uri);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteMedia(string url, string containerName)
        {
            try
            {
                var result = await MediaHelper.DeleteMediaAsync(url, containerName);
                return StatusCode((int)result.StatusCode, result.Message);
            }
            catch  (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
