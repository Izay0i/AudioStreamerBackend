using AudioStreamerAPI.Constants;
using Azure.Storage;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;

namespace AudioStreamerAPI.Helpers
{
    public class MediaHelper
    {
        public static async Task<string> UploadMediaAsync(int id, IFormFile file, string containerName)
        {
            try
            {
                var fName = FileHelper._GenerateFileName(file.FileName, id.ToString());
                var fileUrl = string.Empty;

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
                                MaximumTransferSize = AzureConstants.MAX_FILE_SIZE_IN_BYTES,
                            }
                        });
                    }
                    fileUrl = blob.Uri.AbsoluteUri;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                return fileUrl;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static async Task<OperationalStatus> DeleteMediaAsync(string url, string containerName)
        {
            try
            {
                var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                var connectionString = config.GetSection("Azure")["URL"];

                BlobContainerClient client = new(connectionString, containerName);
                await client.GetBlobClient(url).DeleteAsync();

                return OperationalStatus.SUCCESS;
            }
            catch
            {
                return OperationalStatus.FAILURE;
            }
        }
    }
}
