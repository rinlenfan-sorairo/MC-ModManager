using Microsoft.Win32;
using System.Text.Json;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Collections.Generic;
using System;

namespace MC_ModManager.Services
{
    public static class SettingsService
    {
        public static string ExePath { get; } = Assembly.GetExecutingAssembly().Location;
        public static string ExeDirectory { get; } = Path.GetDirectoryName(ExePath) ?? string.Empty;
        public static string SettingsPath { get; } = Path.Combine(ExeDirectory, "settings.json");
        public static string AppdataPath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".mcmm");

        public static void Initialize()
        {
            if (!File.Exists(SettingsPath))
            {
                var settings = new SettingsModel
                {
                    ExeDirectory_path = ExeDirectory,
                    Minecraft_path = MinecraftService.SearchMinecraftPath(),
                    Minecraft_appdata_path = MinecraftService.SearchMinecraftAppdataPath()
                };
                string jsonString = JsonSerializer.Serialize(settings, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                });
                File.WriteAllText(SettingsPath, jsonString);
            }
            Console.WriteLine();
        }
        public static SettingsModel Load()
        {
            if (!File.Exists(SettingsPath))
            {
                Initialize();
            }
                string jsonString = File.ReadAllText(SettingsPath);
            var settings = JsonSerializer.Deserialize<SettingsModel>(jsonString, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            return settings ?? new SettingsModel{};
        }
    }

    public class SettingsModel
    {
        public string? ExeDirectory_path { get; set; }
        public string? Minecraft_path { get; set; }
        public string? Minecraft_appdata_path { get; set; }
    }
}