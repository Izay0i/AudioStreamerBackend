using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AudioStreamerAPI.Helpers
{
    public static class JsonHelper
    {
        public static bool IsValidJson(this string str)
        {
            str = str.Trim();
            if (string.IsNullOrWhiteSpace(str)) return false;
            if ((str.StartsWith("{") && str.EndsWith("}")) || 
                (str.StartsWith("[") && str.EndsWith("]"))) 
            {
                try
                {
                    var obj = JToken.Parse(str);
                    return true;
                }
                catch (JsonReaderException ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            return false;
        }
    }
}
