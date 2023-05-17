namespace AudioStreamerAPI.Helpers
{
    public class FileHelper
    {
        internal static string _SanitizeFileName(string fileName)
        {
            
            return string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));
        }

        internal static string _GenerateFileName(string fileName, string directory)
        {
            try
            {
                string fName = string.Empty;
                string[] strName = fileName.Split(".");

                fName = string.Format("{0}/{1}-{2}.{3}", 
                    directory, 
                    _SanitizeFileName(strName[0]), 
                    DateTime.Now.ToUniversalTime().ToString("yyyyMMdd\\THHmmssfff"), 
                    strName[1]);
                return fName;
            }
            catch
            {
                return fileName;
            }
        }
    }
}
