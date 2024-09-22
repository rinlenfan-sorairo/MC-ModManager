using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC_ModManager.Helper
{
    internal class PathHelper
    {
        public static readonly string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        public static readonly string ExecutableDirectory = AppDomain.CurrentDomain.BaseDirectory;

        public static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        public static string FindFileInDirectories(string[] searchDirectories, string[] fileNameToFind)
        {
            foreach (var searchDirectory in searchDirectories)
            {
                try
                {
                    // ディレクトリ内のすべてのファイルを再帰的に取得
                    var files = Directory.GetFiles(searchDirectory, "*", SearchOption.AllDirectories);

                    // 一致するファイルを探索
                    foreach (var file in files)
                    {
                        foreach (var fileName in fileNameToFind)
                        {
                            if (Path.GetFileName(file).Equals(fileName, StringComparison.OrdinalIgnoreCase))
                            {
                                return file;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while searching in {searchDirectory}: {ex.Message}");
                }
            }

            // 一致するものが見つからなかった場合はnullを返す
            return null;
        }
    }
}
