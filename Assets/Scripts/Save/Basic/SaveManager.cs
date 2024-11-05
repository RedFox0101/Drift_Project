using Newtonsoft.Json;
using UnityEngine;

namespace Game.Save
{
    public class SaveManager
    {
        private const string Extension = ".db";
        
        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
        };
        
        public void SetData(string key, object data)
        {
            PlayerPrefs.SetString(key + Extension, JsonConvert.SerializeObject(data, _jsonSerializerSettings));
        }
        
        public T GetData<T>(string key)
        {
            var prefs = PlayerPrefs.GetString(key + Extension);
            
            return JsonConvert.DeserializeObject<T>(prefs, _jsonSerializerSettings);
        }

        public bool TryGetData<T>(string key, out T result)
        {
            if (!PlayerPrefs.HasKey(key + Extension))
            {
                result = default;
                return false;
            }
            result = GetData<T>(key);
            
            return result != null;
        }
    }
}