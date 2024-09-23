using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC_ModManager.Services
{
    internal class TempFileService
    {
        public static readonly string TempFolderPath = Path.Combine(SettingsService.GetPath("MC_ModManagerAppData"), "tmpDir");

        public static string InitTempFolder()
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

            return tmpDir;
        }

    }
}
