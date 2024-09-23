using MC_ModManager.Helper;
using MC_ModManager.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC_ModManager.Services
{
    internal class MinecraftService
    {
        private static readonly string MinecraftProfilePath = SettingsService.GetPath("MinecraftAppData") + "launcher_profiles.json";

        private static int? MinecraftProfilesVersion;
        private static JObject? MinecraftProfilesSettings;
        public static void LaunchMinecraft()
        {
            Process process = new Process();
            process.StartInfo.FileName = SettingsService.GetPath("MinecraftLauncher");
            process.Start();
        }

        public static ObservableCollection<MinecraftProfileModel> LoadMinecraftProfiles()
        {
            ObservableCollection<MinecraftProfileModel> profiles = new ObservableCollection<MinecraftProfileModel>();

            try
            {
                string jsonContent = File.ReadAllText(MinecraftProfilePath);
                JObject jsonObject = JObject.Parse(jsonContent);

                if (jsonObject.ContainsKey("settings"))
                    MinecraftProfilesSettings = (JObject)jsonObject["settings"];
                else
                {
                    throw new ArgumentException("キー 'settings' が存在しません。");
                }
                if (jsonObject.ContainsKey("version"))
                    MinecraftProfilesVersion = 3;
                else
                {
                    throw new ArgumentException("キー 'version' が存在しません。");
                }
                if (jsonObject.ContainsKey("profiles"))
                {
                    JObject profilesObject = (JObject)jsonObject["profiles"];
                    foreach (KeyValuePair<string, JToken?> profile in profilesObject)
                    {
                        string profileKey = profile.Key;
                        JObject? profileValue = profile.Value as JObject;
                        if (profileValue != null)
                        {
                            MinecraftProfileModel profileModel = new MinecraftProfileModel(profile.Key, profileValue);
                            profiles.Add(profileModel);
                        }
                    }
                    return profiles;
                }
                else
                {
                    throw new ArgumentException("キー 'profiles' が存在しません。");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("エラー: " + ex.Message);
                return null;
            }
        }


        public static void SaveMinecraftProfiles(ObservableCollection<MinecraftProfileModel> profiles)
        {
            JObject profilesJson = new JObject();
            foreach (MinecraftProfileModel profile in profiles)
            {
                JObject profileJson = new JObject
                {
                    { "gameDir", profile.gameDir },
                    { "icon", profile.icon },
                    { "lastVersionId", profile.lastVersionId },
                    { "name", profile.name },
                    { "type", profile.type }
                };
                if (profile.created != null)
                    profileJson.Add("created", profile.created);
                if (profile.javaArgs != null)
                    profileJson.Add("javaArgs", profile.javaArgs);
                if (profile.lastUsed != null)
                    profileJson.Add("lastUsed", profile.lastUsed);
                profilesJson.Add(profile.id, profileJson);
            }

            JObject MinecraftProfilesJson = new JObject
            {
                { "profiles", profilesJson },
                { "settings", MinecraftProfilesSettings },
                { "version", MinecraftProfilesVersion }
            };


            JsonHelper.CreateDynamicJson(MinecraftProfilePath, MinecraftProfilesJson);
        }

        public static ObservableCollection<MinecraftProfileModel> AddMinecraftProfiles(MinecraftProfileModel newProfile)
        {
            ObservableCollection<MinecraftProfileModel> profiles = LoadMinecraftProfiles();
            profiles.Add(newProfile);
            SaveMinecraftProfiles(profiles);
            return profiles;
        }

        public static ObservableCollection<MinecraftProfileModel> RemoveMinecraftProfiles(string id)
        {
            ObservableCollection<MinecraftProfileModel> profiles = LoadMinecraftProfiles();
            var profileToRemove = profiles.FirstOrDefault(p => p.id == id);
            if (profileToRemove != null)
            {
                profiles.Remove(profileToRemove);
                SaveMinecraftProfiles(profiles);
            }
            return profiles;
        }
    }
}
