using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MC_ModManager.Model
{
    public class ClientGameModel
    {
        public string? ID { get; set; } //ID
        public string? Name { get; set; } //表示名
        public string? Version { get; set; } //マイクラバージョン
        public string? Description { get; set; } //簡単な説明
        public string? ModLoader { get; set; } //Modローダー
        public string? VersionId { get; set; } //バージョンID
        public string? Icon { get; set; } //アイコン
        public string? GameDir { get; set; } //ゲームディレクトリ

        public string? JavaArgs { get; set; } //Java引数

        [JsonIgnore]
        public bool Check { get; set; } //チェックボックスのチェック状態

        public ClientGameModel() { }

        public ClientGameModel(string id, string name, string version, string description, string modLoader, string versionId, string icon, string gameDir)
        {
            ID = id;
            Name = name;
            Version = version;
            Description = description;
            ModLoader = modLoader;
            VersionId = versionId;
            Icon = icon;
            GameDir = gameDir;
            Check = false;
        }

        public ClientGameModel(Dictionary<string, string> gameDict)
        {
            try
            {
                string[] requiredKeys = { "name", "version", "description", "modLoader", "versionId", "icon", "gameDir" };
                foreach (string key in requiredKeys)
                {
                    if (!gameDict.ContainsKey(key))
                    {
                        throw new ArgumentException($"キー '{key}' が存在しません。");
                    }
                }
                ID = gameDict["id"];
                Name = gameDict["name"];
                Version = gameDict["version"];
                Description = gameDict["description"];
                ModLoader = gameDict["modLoader"];
                VersionId = gameDict["versionId"];
                Icon = gameDict["icon"];
                GameDir = gameDict["gameDir"];
                Check = false;
            }
            catch (Exception e)
            {
                throw new ArgumentException($"ゲームデータの読み込みに失敗しました。", e);
            }
        }
        public ClientGameModel(JObject gameData)
        {
            try
            {
                string[] requiredKeys = { "name", "version", "description", "modLoader", "versionId", "icon", "gameDir" };
                foreach (string key in requiredKeys)
                {
                    if (!gameData.ContainsKey(key))
                    {
                        throw new ArgumentException($"キー '{key}' が存在しません。");
                    }
                }
                ID = gameData["id"].ToString();
                Name = gameData["name"].ToString();
                Version = gameData["version"].ToString();
                Description = gameData["description"].ToString();
                ModLoader = gameData["modLoader"].ToString();
                VersionId = gameData["versionId"].ToString();
                Icon = gameData["icon"].ToString();
                GameDir = gameData["gameDir"].ToString();
                Check = false;
            }
            catch (Exception e)
            {
                throw new ArgumentException($"ゲームデータの読み込みに失敗しました。", e);
            }
        }
    }
}
