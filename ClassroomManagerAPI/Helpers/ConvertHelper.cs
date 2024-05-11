using Newtonsoft.Json;

namespace ClassroomManagerAPI.Helpers
{
    public static class ConvertHelper
    {
        public static TEnum? EnumParse<TEnum>(this string? data) where TEnum : Enum
        {
            if (Enum.TryParse(typeof(TEnum), data, ignoreCase: false, out object result))
            {
                return (TEnum)result;
            }

            return default(TEnum);
        }

        public static IList<TEnum> EnumToList<TEnum>() where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();
        }

        public static IList<string> EnumToListStr<TEnum>() where TEnum : Enum
        {
            return (from x in EnumToList<TEnum>()
                    select x.ToString()).ToList();
        }
        
        public static T? ToObject<T>(string jsonString)
        {
            try
            {
                if (string.IsNullOrEmpty(jsonString))
                {
                    return default(T);
                }
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch
            {
                return default(T);
            }
        }

        public static string ObjectToString(dynamic json)
        {
            try
            {
                return JsonConvert.SerializeObject(json);
            }
            catch(Exception e)
            {
                return string.Empty;
            }
        }
    }
}
