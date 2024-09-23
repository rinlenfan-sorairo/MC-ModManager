using MC_ModManager.Helper;
using MC_ModManager.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace MC_ModManager.Services
{
    internal class ClientTabService
    {
        private static readonly string ClientGameListPath = SettingsService.GetPath("MC_ModManagerAppData") + "gamedata.json";

        public static ObservableCollection<ClientGameModel> LoadClientGameList()
        {
            ObservableCollection<ClientGameModel> clientGameList = JsonHelper.ReadJsonFile<ObservableCollection<ClientGameModel>>(ClientGameListPath);
            if (clientGameList == null)
            {
                clientGameList = new ObservableCollection<ClientGameModel>();
                SaveClientGameList(clientGameList);
            }
            return clientGameList;
        }

        public static void SaveClientGameList(ObservableCollection<ClientGameModel> clientGameList)
        {
            JsonHelper.CreateJsonFile(ClientGameListPath, clientGameList);
        }

        public static ObservableCollection<ClientGameModel> ImportGameData(string gamedataZipPath)
        {

            string tmpDir = TempFileService.InitTempFolder();
            ZipFile.ExtractToDirectory(gamedataZipPath, tmpDir);
            string tmpImportGameFolder = Path.Combine(tmpDir, Path.GetFileNameWithoutExtension(gamedataZipPath));
            string gamedataJsonPath = Path.Combine(tmpImportGameFolder, "game_data.json");
            JObject gameData = JsonHelper.LoadDynamicJson(gamedataJsonPath);
            Guid myUUId = Guid.NewGuid();
            string ID = myUUId.ToString();

            string newGameDir = Path.Combine(SettingsService.GetPath("MC_ModManagerAppData"), "games", ID);

            Directory.CreateDirectory(newGameDir);
            if (Directory.Exists(Path.Combine(tmpImportGameFolder, "mods")))
                Directory.Move(Path.Combine(tmpImportGameFolder, "mods"), Path.Combine(newGameDir, "mods"));

            gameData.Add("ID", ID);
            gameData.Add("GameDir", newGameDir);

            return AddClientGameList(gameData);

        }

        public static ObservableCollection<ClientGameModel> AddClientGameList(JObject gameData)
        {
            ObservableCollection<ClientGameModel> clientGameList = LoadClientGameList();

            ClientGameModel newClientGameData = new ClientGameModel
            {
                ID = gameData["ID"].ToString(),
                Name = gameData["Name"].ToString(),
                GameDir = gameData["GameDir"].ToString(),
                VersionId = gameData["VersionId"].ToString(),
            };
            if (gameData.ContainsKey("Icon"))
                newClientGameData.Icon = gameData["Icon"]?.ToString();
            else
                newClientGameData.Icon = "Furnace";
            if (gameData.ContainsKey("JavaArgs"))
                newClientGameData.JavaArgs = gameData["JavaArgs"]?.ToString();

            clientGameList.Add(newClientGameData);
            SaveClientGameList(clientGameList);

            DateTime now = DateTime.UtcNow;
            string formattedDate = now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

            MinecraftProfileModel newProfile = new MinecraftProfileModel
            {
                id = newClientGameData.ID,
                created = formattedDate,
                gameDir = newClientGameData.GameDir,
                lastUsed = formattedDate,
                lastVersionId = newClientGameData.VersionId,
                name = newClientGameData.Name,
                type = "custom",
            };
            //"-Xmx2G -XX:+UseConcMarkSweepGC -XX:+CMSIncrementalMode -XX:-UseAdaptiveSizePolicy -Xmn128M";
            if (newClientGameData.Icon != null)
                newProfile.icon = newClientGameData.Icon;
            else
                newProfile.icon = "Furnace";

            if (newClientGameData.JavaArgs != null)
                newProfile.javaArgs = newClientGameData.JavaArgs;
            MinecraftService.AddMinecraftProfiles(newProfile);

            return clientGameList;
        }

        public static ObservableCollection<ClientGameModel> DeleteClientGameList(ClientGameModel gameData)
        {
            ObservableCollection<ClientGameModel> clientGameList = LoadClientGameList();
            MinecraftService.RemoveMinecraftProfiles(gameData.ID);
            Directory.Delete(gameData.GameDir, true);
            clientGameList.Remove(gameData);
            SaveClientGameList(clientGameList);
            return clientGameList;
        }
    }
}
