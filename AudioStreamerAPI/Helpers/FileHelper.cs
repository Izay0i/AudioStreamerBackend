namespace AudioStreamerAPI.Helpers
{
    public class FileHelper
    {
        public static string SanitizeFileName(string fileName)
        {
            fileName = fileName.Replace(" ", "_");
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
    }
}
