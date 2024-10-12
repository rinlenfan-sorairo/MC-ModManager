using MC_ModManager.Services;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Path = System.IO.Path;

namespace MC_ModManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Initialize();
        }

        private void Initialize()
        {
            SettingsService.Initialize();
            Directory.CreateDirectory(SettingsService.AppdataPath);
            Directory.CreateDirectory(Path.Combine(SettingsService.AppdataPath, "gamelist"));
            var settings = SettingsService.Load();
        }
    }
}