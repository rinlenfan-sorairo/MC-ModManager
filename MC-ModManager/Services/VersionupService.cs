using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MC_ModManager.Services
{
    internal class VersionupService
    {
        public static bool VersionupCheck() {
            string LastVersionUrl = "https://api.github.com/repos/rinlenfan-sorairo/MC-ModManager/releases/latest";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("MC-ModManager/0.1.0");
                var response = client.GetStringAsync(LastVersionUrl);
                var json = JObject.Parse(response.Result);
                if("0.1.0" == json["tag_name"].ToString())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
