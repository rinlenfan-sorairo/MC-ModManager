using System;
using System.IO;
using IniParser;
using IniParser.Model;
using MC_ModManager.Helper;



namespace MC_ModManager.Services
{
    internal class SettingsService
    {
        private static readonly string IniFilePath = Path.Combine(PathHelper.ExecutableDirectory, "settings.ini");
        private static readonly FileIniDataParser Parser = new FileIniDataParser();

        public static IniData ReadSettings()
        {
            if (!File.Exists(IniFilePath)) CreateSettings();

            IniData data = Parser.ReadFile(IniFilePath);
            if (data["settings"]["Version"] != "0.1.0")
                data = UpdateSettings(data);

                return data;
        }

        public static void CreateSettings()
        {
            IniData data = new IniData();

            string mlpath = PathHelper.FindFileInDirectories([
                @"C:\Program Files (x86)\Minecraft Launcher",
                @"C:\Program Files\Minecraft Launcher",
                @"C:\XboxGames\Minecraft Launcher\Content",
                Path.Combine(PathHelper.AppDataPath, ".minecraft"),
                ],
                [
                "MinecraftLauncher.exe",
                "Minecraft.exe"
                ]);

            data["settings"]["Version"] = "0.1.0";
            data["path"]["MinecraftLauncher"] = mlpath;
            data["path"]["MinecraftAppData"] = Path.Combine(PathHelper.AppDataPath, ".minecraft");
            data["path"]["MC_ModManagerAppData"] = Path.Combine(PathHelper.AppDataPath, ".mcmm");


            Parser.WriteFile(IniFilePath, data);
        }

        public static IniData UpdateSettings(IniData data)
        {
            data["settings"]["Version"] = "0.1.0";
            Parser.WriteFile(IniFilePath, data);

            return data;
        }

        public static string GetPath(string key)
        {
            IniData data = ReadSettings();
            return data["path"][key];
        }
    }
}
