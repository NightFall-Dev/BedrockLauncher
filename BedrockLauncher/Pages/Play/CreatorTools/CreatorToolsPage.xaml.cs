using BedrockLauncher.Classes;
using BedrockLauncher.ViewModels;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BedrockLauncher.Pages.Play.CreatorTools
{
    public partial class CreatorToolsPage : Page
    {
        public CreatorToolsPage()
        {
            InitializeComponent();
        }

        private void MainPlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainDataModel.Default.PackageManager.isGameRunning) MainDataModel.Default.KillGame();
            else
            {
                var i = InstallationsList.SelectedItem as BLInstallation;
                bool KeepLauncherOpen = Properties.LauncherSettings.Default.KeepLauncherOpen;
                MainDataModel.Default.Play(ViewModels.MainDataModel.Default.Config.CurrentProfile, i, KeepLauncherOpen, true);
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e) { }
    }
}
