using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;

namespace MC_ModManager.Model
{
    public class MinecraftProfileModel
    {
        public string? id { get; set; }
        public string? created { get; set; }
        public string? gameDir { get; set; }
        public string? icon { get; set; }
        public string? javaArgs { get; set; }
        public string? lastUsed { get; set; }
        public string? lastVersionId { get; set; }
        public string? name { get; set; }
        public string? type { get; set; }

        public MinecraftProfileModel() { }
        public MinecraftProfileModel(string id, string gameDir, string icon, string lastVersionId, string name, string type)
        {
            this.id = id;
            this.gameDir = gameDir;
            this.icon = icon;
            this.lastVersionId = lastVersionId;
            this.name = name;
            this.type = type;
        }
        public MinecraftProfileModel(string id, Dictionary<string, string> profileDict)
        {
            try
            {
                string[] requiredKeys = { "gameDir", "icon", "lastVersionId", "name", "type" };
                foreach (string key in requiredKeys)
                {
                    if (!profileDict.ContainsKey(key))
                    {
                        throw new ArgumentException($"キー '{key}' が存在しません。");
                    }
                }
                this.id = id;
                gameDir = profileDict["gameDir"];
                icon = profileDict["icon"];
                lastVersionId = profileDict["lastVersionId"];
                name = profileDict["name"];
                type = profileDict["type"];
                if (profileDict.ContainsKey("created"))
                    created = profileDict["created"];
                if (profileDict.ContainsKey("javaArgs"))
                    javaArgs = profileDict["javaArgs"];
                if (profileDict.ContainsKey("lastUsed"))
                    lastUsed = profileDict["lastUsed"];
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("エラーが発生しました。\nプロファイルデータが予期しない形です。");
            }
        }
        public MinecraftProfileModel(string id, JObject profileData)
        {
            try
            {
                string[] requiredKeys = { "gameDir", "icon", "lastVersionId", "name", "type" };
                foreach (string key in requiredKeys)
                {
                    if (!profileData.ContainsKey(key))
                    {
                        throw new ArgumentException($"キー '{key}' が存在しません。");
                    }
                }
                this.id = id;
                gameDir = profileData["gameDir"].ToString();
                icon = profileData["icon"].ToString();
                lastVersionId = profileData["lastVersionId"].ToString();
                name = profileData["name"].ToString();
                type = profileData["type"].ToString();
                if (profileData.ContainsKey("created"))
                    created = profileData["created"].ToString();
                if (profileData.ContainsKey("javaArgs"))
                    javaArgs = profileData["javaArgs"].ToString();
                if (profileData.ContainsKey("lastUsed"))
                    lastUsed = profileData["lastUsed"].ToString();
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("エラーが発生しました。\nプロファイルデータが予期しない形です。");
            }
        }
    }


}
