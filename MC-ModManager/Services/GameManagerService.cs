using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Xml.Linq;
using MC_ModManager.Helper;
using MC_ModManager.Model;
using MC_ModManager.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Path = System.IO.Path;

namespace MC_ModManager.Services
{
    internal class GameManagerService
    {
        public static ObservableCollection<GameDataModel> LoadGameData()
        {
            string gamedataJsonPath = Path.Combine(SettingsService.GetPath("MC_ModManagerAppData"), "gamedata_list.json");
            if (!File.Exists(gamedataJsonPath)) CreateGameData();

            var GameDataList = JsonHelper.ReadJsonFile<ObservableCollection<GameDataModel>>(gamedataJsonPath);

            if (GameDataList != null)
            {
                foreach (var gameData in GameDataList)
                {
                    gameData.Check = false;
                }
            }

            return GameDataList;
        }

        public static void CreateGameData()
        {
            string gamedataJsonPath = Path.Combine(SettingsService.GetPath("MC_ModManagerAppData"), "gamedata_list.json");
            var GameDataList = new ObservableCollection<GameDataModel>();

            JsonHelper.CreateJsonFile(gamedataJsonPath, GameDataList);
        }
        public static void SaveGameData(ObservableCollection<GameDataModel> gameDataList)
        {
            string gamedataJsonPath = Path.Combine(SettingsService.GetPath("MC_ModManagerAppData"), "gamedata_list.json");

            var json = JsonConvert.SerializeObject(gameDataList, Formatting.Indented);
            File.WriteAllText(gamedataJsonPath, json);
        }

        public static ObservableCollection<GameDataModel> AddGameData(GameDataModel gameData, ObservableCollection<GameDataModel> GameDataList)
        {
            GameDataList.Add(gameData);
            SaveGameData(GameDataList);
            return GameDataList;
        }

        public static ObservableCollection<GameDataModel> ImportGameData(string zipPath, ObservableCollection<GameDataModel> GameDataList)
        {
            string tmpDir = Path.Combine(SettingsService.GetPath("MC_ModManagerAppData"), "tmpDir");
            if (!Directory.Exists(tmpDir))
            {
                Directory.CreateDirectory(tmpDir);
            }

            foreach (var file in Directory.GetFiles(tmpDir))
            {
                File.Delete(file);
            }

            foreach (var dir in Directory.GetDirectories(tmpDir))
            {
                Directory.Delete(dir, true);
            }

            ZipFile.ExtractToDirectory(zipPath, tmpDir);
            var tmpGameFolder = Path.Combine(tmpDir, Path.GetFileNameWithoutExtension(zipPath));
            string jsonData = Path.Combine(tmpGameFolder, "game_data.json");
            var newGameData = JsonHelper.ReadJsonFile<GameDataModel>(jsonData);
            JObject newGameJson = JsonHelper.LoadDynamicJson(jsonData);
            Guid myUUId = Guid.NewGuid();
            string convertedUUID = myUUId.ToString();
            string gameDir = Path.Combine(SettingsService.GetPath("MC_ModManagerAppData"), "games", convertedUUID);
            Directory.CreateDirectory(gameDir);
            if(Directory.Exists(Path.Combine(tmpGameFolder, "mods"))){
                Directory.Move(Path.Combine(tmpGameFolder, "mods"), Path.Combine(gameDir, "mods"));
            }
            newGameData.GameDir = gameDir;
            GameDataList = AddGameData(newGameData, GameDataList);

            return GameDataList;
        }
    }
}
