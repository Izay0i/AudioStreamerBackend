using AudioStreamerAPI.Constants;
using AudioStreamerAPI.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

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
        public async Task<IActionResult> GetMedia(string src, string contentType)
        {
            var client = _httpClientFactory.CreateClient();
            var contentInBytes = await client.GetByteArrayAsync(src);
            Console.WriteLine("Content-Range: {0}", Request.Headers.Range.ToString());
            return File(contentInBytes, contentType, true);
        }

        [HttpGet("stream")]
        public async Task<IActionResult> GetMediaStream(string src, string contentType)
        {
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, src);
            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            var stream = await response.Content.ReadAsStreamAsync();
            //vvv This guy right here doesn't support seeking
            //Why isn't it possible
            //It's just not
            //Why not you stupid bast*rd
            return File(stream, contentType, true);
        }

        [RequestSizeLimit(AzureConstants.MAX_FILE_SIZE_IN_BYTES)]
        [RequestFormLimits(MultipartBodyLengthLimit = AzureConstants.MAX_FILE_SIZE_IN_BYTES)]
        [HttpPost]
        public async Task<IActionResult> UploadMedia(int id, IFormFile file, string containerName)
        {
            try
            {
                var uri = await MediaHelper.UploadMediaAsync(id, file, containerName);
                return Ok(uri);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMedia(string url, string containerName)
        {
            try
            {
                await MediaHelper.DeleteMediaAsync(url, containerName);
                return Ok();
            }
            catch  (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
