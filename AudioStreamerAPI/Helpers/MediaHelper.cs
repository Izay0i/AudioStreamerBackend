using AudioStreamerAPI.Constants;
using Azure.Storage.Blobs;

namespace AudioStreamerAPI.Helpers
{
    public class MediaHelper
    {
        public static async Task<string> UploadMediaAsync(int id, IFormFile file, string containerName)
        {
            try
            {
                var fName = FileHelper.GenerateFileName(file.FileName, id.ToString());
                var fileUri = string.Empty;

                var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                var connectionString = config.GetSection("Azure")["URL"];

                BlobContainerClient client = new(connectionString, containerName);
                try
                {
                    //B.L.O.B!! Do something!
                    BlobClient blob = client.GetBlobClient(fName);
                    using (Stream stream = file.OpenReadStream())
                    {
                        await blob.UploadAsync(stream, options: new()
                        {
                            TransferOptions = new()
                            {
                                MaximumTransferSize = AzureConstants.MAX_FILE_SIZE,
                            }
                        });
                    }
                    fileUri = blob.Uri.AbsoluteUri;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                return fileUri;
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
