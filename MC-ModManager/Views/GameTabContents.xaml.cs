using MC_ModManager.Models;
using MC_ModManager.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static MaterialDesignThemes.Wpf.Theme.ToolBar;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Path = System.IO.Path;

namespace MC_ModManager.Views
{
    /// <summary>
    /// GameTabContents.xaml の相互作用ロジック
    /// </summary>
    public partial class GameTabContents : UserControl
    {
        public ObservableCollection<GameProfile> GameList { get; set; } = new ObservableCollection<GameProfile>();
        public GameTabContents()
        {
            InitializeComponent();

            var settings = SettingsService.Load();
            string jsonContent = File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".mcmm", "game_profile_list.json"));
            ObservableCollection<GameProfile> gameProfileList = JsonSerializer.Deserialize<ObservableCollection<GameProfile>>(jsonContent);
            foreach (var gameProfile in gameProfileList)
            {
                gameProfile.Check = false;
            }

            GameList = gameProfileList;

            this.DataContext = this;
        }

        private void LunchMinecraft(object sender, RoutedEventArgs e)
        {
            MinecraftService.StartMinecraft();
        }

        private void ImportGame(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Zip files (*.zip)|*.zip|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                // 選択されたファイルのパスを取得
                string filePath = openFileDialog.FileName;
                Directory.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".mcmm", "tmp"), true);
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".mcmm", "tmp"));
                ZipFile.ExtractToDirectory(filePath, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".mcmm", "tmp"));
                string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".mcmm", "tmp", Path.GetFileNameWithoutExtension(filePath));
                MessageBox.Show(folderPath);
                string gameProfileContent = File.ReadAllText(Path.Combine(folderPath, "mmod.json"));
                GameProfile loadGameProfile = JsonSerializer.Deserialize<GameProfile>(gameProfileContent);
                foreach (var gameProfile in GameList)
                {
                    if(gameProfile.UUID == loadGameProfile.UUID)
                    {
                        if(gameProfile.ProfileVersion == loadGameProfile.ProfileVersion)
                        {
                            MessageBox.Show("すでにインポートされています。");
                            return;
                        }
                        else
                        {
                            gameProfile.ProfileVersion = loadGameProfile.ProfileVersion;
                            gameProfile.MinecraftVersion = loadGameProfile.MinecraftVersion;
                            gameProfile.Name = loadGameProfile.Name;
                            gameProfile.Description = loadGameProfile.Description;
                            loadGameProfile.Profile.GameDir = gameProfile.Profile.GameDir;
                            gameProfile.Profile = loadGameProfile.Profile;
                            if (Directory.Exists(Path.Combine(gameProfile.Profile.GameDir, "mods")))
                            {
                                Directory.Delete(Path.Combine(gameProfile.Profile.GameDir, "mods"), true);
                            }
                            ZipFile.ExtractToDirectory(Path.Combine(folderPath, "mods"), gameProfile.Profile.GameDir);
                            MinecraftService.AddMinecraftProfile(gameProfile.UUID, gameProfile.Profile);

                            MessageBox.Show("バージョンの変更を行いました。");
                            return;
                        }
                    }
                }
                string gameDirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".mcmm", "games", loadGameProfile.UUID);
                Directory.CreateDirectory(gameDirPath);
                ZipFile.ExtractToDirectory(Path.Combine(folderPath, "mods.zip"), gameDirPath);
                loadGameProfile.Profile.GameDir = gameDirPath;
                MinecraftService.AddMinecraftProfile(loadGameProfile.UUID, loadGameProfile.Profile);
                GameList.Add(loadGameProfile);
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true // 読みやすくするためにインデントを追加
                };
                string jsonString = JsonSerializer.Serialize(GameList, options);
                File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".mcmm", "game_profile_list.json"), jsonString);
            }
            else
            {
                MessageBox.Show("選択されませんでした。");
            }
        }

        private void EditGame(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("この機能は現在使用できません。");
        }

        private void DeleteGame(object sender, RoutedEventArgs e)
        {
            var itemsToDelete = new ObservableCollection<GameProfile>(GameList.Where(item => item.Check));
            if (itemsToDelete.Count == 0)
            {
                MessageBox.Show("削除するアイテムが選択されていません。");
                return;
            }

            foreach (var item in itemsToDelete)
            {
                if (Directory.Exists(item.Profile.GameDir))
                {
                    Directory.Delete(item.Profile.GameDir, true);
                }
                MinecraftService.DeleteMinecraftProfile(item.UUID);
                GameList.Remove(item);
            }
        }
    }
}
