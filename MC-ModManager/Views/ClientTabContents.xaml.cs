using MC_ModManager.Model;
using MC_ModManager.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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

namespace MC_ModManager.Views
{
    /// <summary>
    /// UserControl1.xaml の相互作用ロジック
    /// </summary>
    public partial class ClientTabContents : UserControl
    {
        public ObservableCollection<ClientGameModel> ClientGameList { get; set; }
        public ClientTabContents()
        {
            InitializeComponent();
        }

        private void LunchMinecraft(object sender, RoutedEventArgs e)
        {
            MinecraftService.LaunchMinecraft();
        }

        private void ImportGame(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "ZIP files (*.zip)|*.zip|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string zipFilePath = openFileDialog.FileName;
                ClientGameList = ClientTabService.ImportGameData(zipFilePath);

            }
        }

        private void DeleteGame(object sender, RoutedEventArgs e)
        {
            var itemsToDelete = new ObservableCollection<ClientGameModel>(ClientGameList.Where(item => item.Check == true));
            if (itemsToDelete.Count == 0)
            {
                MessageBox.Show("削除するアイテムが選択されていません。");
                return;
            }
        }

        private void EditGame(object sender, RoutedEventArgs e)
        {

            MessageBox.Show("まだこの機能は実装されていません。");
        }
    }
}
