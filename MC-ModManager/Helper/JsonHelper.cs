using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MC_ModManager.Helper
{
    internal class JsonHelper
    {
        public static void CreateDynamicJson(string filePath, object data)
        {
            string jsonString = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filePath, jsonString);
        }

        public static JObject LoadDynamicJson(string filePath)
        {
            string jsonString = File.ReadAllText(filePath);
            return JObject.Parse(jsonString);
        }

        public static void UpdateDynamicJson(string filePath, string key, object newValue)
        {
            string jsonString = File.ReadAllText(filePath);
            JObject jsonObject = JObject.Parse(jsonString);
            jsonObject[key] = JToken.FromObject(newValue);
            File.WriteAllText(filePath, jsonObject.ToString(Formatting.Indented));
        }

        public static void CreateJsonFile<T>(string path, T data)
        {
            var jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(path, jsonData);
        }

        public static T? ReadJsonFile<T>(string path)
        {
            var jsonData = File.ReadAllText(path);
            var result = JsonConvert.DeserializeObject<T>(jsonData);

            return result;
        }

        public static void UpdateJsonFile<T>(string path, T data)
        {
            var jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(path, jsonData);
        }
    }
}
