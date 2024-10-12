using MC_ModManager.Model;
using MC_ModManager.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MC_ModManager.Services
{
    public class MinecraftService
    {
        public static string SearchMinecraftPath()
        {
            if (File.Exists("C:\\XboxGames\\Minecraft Launcher\\Content\\Minecraft.exe"))
                return "C:\\XboxGames\\Minecraft Launcher\\Content\\Minecraft.exe";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Exe files (*.exe)|*.exe|All files (*.*)|*.*";
            MessageBox.Show("MinecraftLauncherの実行ファイルが見つかりませんでした。\nMinecraftLauncherの実行ファイル(*.exe)を選択してください。");
            if (openFileDialog.ShowDialog() == true)
            {
                // 選択されたファイルのパスを取得
                string filePath = openFileDialog.FileName;
                return filePath;
            }
            MessageBox.Show("MinecraftLauncherのパスが見つかりませんでした。\n一部機能を制限します。");
            return "enpty";
        }

        public static string SearchMinecraftAppdataPath()
        {
            if (Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft")))
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft");
            OpenFolderDialog openFolderDialog = new OpenFolderDialog();
            openFolderDialog.Title = "MinecraftのAppDataフォルダを選択してください。";
            openFolderDialog.Multiselect = false;
            MessageBox.Show("MinecraftのAppDataフォルダが見つかりませんでした。\nMinecraftのAppDataフォルダを選択してください。");
            if (openFolderDialog.ShowDialog() == true)
            {
                // 選択されたフォルダのパスを取得
                string FolderPath = openFolderDialog.FolderName;
                return FolderPath;
            }
            MessageBox.Show("MinecraftのAppDataフォルダが見つかりませんでした。\n機能を制限します。");
            return "enpty";
        }
        public static bool StartMinecraft()
        {
            try
            {
                var settings = SettingsService.Load();
                if (settings != null)
                {
                    if (settings.Minecraft_path != "enpty")
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo
                        {
                            FileName = settings.Minecraft_path,
                            UseShellExecute = true
                        };
                        Process process = Process.Start(startInfo);
                        return true;
                    }
                }
                MessageBox.Show("この機能は制限されています。");
                return false;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Minecraftの起動に失敗しました。");
                Console.WriteLine($"プロセスの開始中にエラーが発生しました: {ex.Message}");
                return false;
            }
        }
        public static MinecraftProfilesModel LoadMinecraftProfiles()
        {
            var settings = SettingsService.Load();
            string profilePath = Path.Combine(settings.Minecraft_appdata_path, "launcher_profiles.json");
            MessageBox.Show(profilePath);
            string fileContent = File.ReadAllText(profilePath);
            MessageBox.Show(fileContent);
            return JsonSerializer.Deserialize<MinecraftProfilesModel>(fileContent);
        }
        public static void SaveMinecraftProfiles(MinecraftProfilesModel profiles)
        {
            var settings = SettingsService.Load();
            string profilePath = Path.Combine(settings.Minecraft_appdata_path, "launcher_profiles.json");
            File.WriteAllText(profilePath, JsonSerializer.Serialize(profiles, new JsonSerializerOptions
            {
                WriteIndented = true
            }));
        }

        public static void AddMinecraftProfile(string uuid, MinecraftProfile addProfile)
        {
            var profiles= LoadMinecraftProfiles();
            profiles.Profiles.Add(uuid, addProfile);
            SaveMinecraftProfiles(profiles);
        }
        public static void DeleteMinecraftProfile(string uuid)
        {
            var profiles = LoadMinecraftProfiles();
            profiles.Profiles.Remove(uuid);
            SaveMinecraftProfiles(profiles);
        }
    }

    public class MinecraftProfilesModel
    {
        [JsonPropertyName("profiles")]
        public required Dictionary<string, MinecraftProfile> Profiles { get; set; }
        [JsonPropertyName("version")]
        public required int Version { get; set; }
    }
    public class MinecraftProfilesSettingsMode
    {
        [JsonPropertyName("crashAssistance")]
        public required bool CrashAssistance { get; set; }

        [JsonPropertyName("enableAdvanced")]
        public required bool EnableAdvanced { get; set; }
        [JsonPropertyName("enableAnalytics")]
        public required bool EnableAnalytics { get; set; }
        [JsonPropertyName("enableHistorical")]
        public required bool EnableHistorical { get; set; }
        [JsonPropertyName("enableReleases")]
        public required bool EnableReleases { get; set; }
        [JsonPropertyName("EnableSnapshots")]
        public required bool enableSnapshots { get; set; }
        [JsonPropertyName("keepLauncherOpen")]
        public required bool KeepLauncherOpen { get; set; }
        [JsonPropertyName("profileSorting")]
        public required string ProfileSorting { get; set; }
        [JsonPropertyName("showGameLog")]
        public required bool ShowGameLog { get; set; }
        [JsonPropertyName("showMenu")]
        public required bool ShowMenu { get; set; }
        [JsonPropertyName("soundOn")]
        public required bool SoundOn { get; set; }
    }
}
