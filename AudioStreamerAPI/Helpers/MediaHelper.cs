using AudioStreamerAPI.Constants;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using System.Text;

namespace AudioStreamerAPI.Helpers
{
    public class MediaHelper
    {
        public static async Task<string> UploadChunksAsync(int id, IFormFile file, string containerName)
        {
            var fName = FileHelper.GenerateFileName(file.FileName, id.ToString());

            //Temporarily saving file to disk
            var tempFileName = Path.GetTempFileName();
            using (var stream = File.Create(tempFileName)) 
            { 
                await file.CopyToAsync(stream);
            }

            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var connectionString = config.GetSection("Azure")["URL"];

            BlobContainerClient blobContainerClient = new(connectionString, containerName);
            try
            {
                var blockBlobClient = blobContainerClient.GetBlockBlobClient(fName);

                long blockSize = AzureConstants.MAX_TRANSFER_SIZE;
                int offset = 0;
                int counter = 0;

                List<string> blockIds = new();

                using (var fs = File.OpenRead(tempFileName))
                {
                    var bytesRemaining = fs.Length;
                    do
                    {
                        var dataToRead = Math.Min(bytesRemaining, blockSize);
                        byte[] data = new byte[dataToRead];
                        var dataRead = fs.Read(data, offset, (int)dataToRead);
                        bytesRemaining -= dataRead;

                        if (dataRead > 0)
                        {
                            var blockId = Convert.ToBase64String(Encoding.UTF8.GetBytes(counter.ToString("D6")));
                            await blockBlobClient.StageBlockAsync(blockId, new MemoryStream(data));

                            //Console.WriteLine(string.Format("Block {0} uploaded successfully.", counter.ToString("d6")));

                            blockIds.Add(blockId);
                            counter++;
                        }
                    }
                    while (bytesRemaining > 0);

                    await blockBlobClient.CommitBlockListAsync(blockIds);
                }

                return blockBlobClient.Uri.AbsoluteUri;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<OperationalStatus> DeleteMediaAsync(string uri, string containerName)
        {
            try
            {
                var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                var connectionString = config.GetSection("Azure")["URL"];

                var storageUri = config.GetSection("Azure")["StorageURI"];
                var relativeUri = FileHelper.GetBlobFromUri(storageUri, uri, containerName);

                BlobContainerClient client = new(connectionString, containerName);
                await client.GetBlobClient(relativeUri).DeleteAsync();

                return new OperationalStatus
                {
                    StatusCode = OperationalStatusEnums.Ok,
                    Message = $"Successfully deleted media inside container: {containerName}",
                };
            }
            catch
            {
                return new OperationalStatus
                {
                    StatusCode = OperationalStatusEnums.NotFound,
                    Message = $"Failed to find: {uri} inside container: {containerName}",
                };
            }
        }
    }
}
