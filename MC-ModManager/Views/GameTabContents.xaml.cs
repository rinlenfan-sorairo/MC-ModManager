using MC_ModManager.Model;
using MC_ModManager.Services;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Windows.Controls;

namespace MC_ModManager.Views
{
    /// <summary>
    /// UserControl1.xaml の相互作用ロジック
    /// </summary>
    public partial class GameTabContents : UserControl
    {
        public ObservableCollection<GameDataModel> GameDataList { get; set; }

        public GameTabContents()
        {
            InitializeComponent();

            LoadGameList();
        }

        private void LoadGameList()
        {
            GameDataList = GameManagerService.LoadGameData();
            DataContext = this;
        }

        private void LunchMinecraft(object sender, RoutedEventArgs e)
        {
            try
            {
                // Processクラスを使ってEXEファイルを起動
                Process process = new Process();
                process.StartInfo.FileName = SettingsService.GetPath("MinecraftLauncher");
                process.Start();
                Console.WriteLine("MinecraftLauncherが起動しました。");
            }
            catch (Exception ex)
            {
                // エラー処理
                Console.WriteLine("エラー: " + ex.Message);
            }
        }

        private void ImportGame(object sender, RoutedEventArgs e)
        {
            //ここから
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "ZIP files (*.zip)|*.zip|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string zipFilePath = openFileDialog.FileName;
                GameDataList = GameManagerService.ImportGameData(zipFilePath, GameDataList);
            }
        }
        private void EditGame(object sender, RoutedEventArgs e)
        {
            var itemsToDelete = new ObservableCollection<GameDataModel>(GameDataList.Where(item => item.Check == true));
            MessageBox.Show("現在この機能は利用できません。");
            return;
            if (itemsToDelete.Count == 0)
            {
                // チェックのついたアイテムが無い場合
                MessageBox.Show("編集するアイテムが選択されていません。");
                return;
            }else if (itemsToDelete.Count > 1)
            {
                MessageBox.Show("編集は一括では行えません。");
                return;
            }
        }
        private void DeleteGame(object sender, RoutedEventArgs e)
        {
            var itemsToDelete = new ObservableCollection<GameDataModel>(GameDataList.Where(item => item.Check == true));
            if (itemsToDelete.Count == 0)
            {
                MessageBox.Show("削除するアイテムが選択されていません。");
                return;
            }
            var result = MessageBox.Show("選択されたアイテムを削除してもよろしいですか？", "確認", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                // DataGridからアイテムを削除
                foreach (var item in itemsToDelete)
                {
                    GameDataList.Remove(item);
                    if(Directory.Exists(item.GameDir)) Directory.Delete(item.GameDir, true);
                }
                // JSONファイルからアイテムを削除
                GameManagerService.SaveGameData(GameDataList);


                MessageBox.Show("選択されたアイテムが削除されました。");
            }
        }
    }
}
