using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MC_ModManager.Model
{
    public class GameDataModel
    {
        public required string Name { get; set; } //名前
        public required string Description { get; set; } //説明
        public required string Version { get; set; } //バージョン
        public required string ModLoader { get; set; } //Modローダー
        public List<string>? Mods { get; set; } //Modリスト
        public string? GameDir { get; set; } //ゲームディレクトリ
        public string? URL { get; set; } //URL

        [JsonIgnore]
        public bool Check { get; set; } //チェックボックスのチェック状態
    }
}
