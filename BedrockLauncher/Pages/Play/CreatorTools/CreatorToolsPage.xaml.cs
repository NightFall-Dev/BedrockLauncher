using BedrockLauncher.Classes;
using BedrockLauncher.Pages.Play.Installations.Components;
using BedrockLauncher.UpdateProcessor.Enums;
using BedrockLauncher.ViewModels;
using PostSharp.Patterns.Model;
using System;
using System.ComponentModel;
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
            InstallationsList.SelectionChanged += CheckEditorCompatility;
            ((INotifyPropertyChanged)MainDataModel.Default.ProgressBarState).PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(MainDataModel.Default.ProgressBarState.AllowPlaying))
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        CheckEditorCompatility(s, e);
                    });
            }; ;
        }

        private void CheckEditorCompatility(object _, EventArgs __)
        {
            BLInstallation selectedInstallation = InstallationsList.SelectedItem as BLInstallation;
            if (MainDataModel.Default.PackageManager.isGameRunning)
                EditorPlayButton.IsEnabled = true;
            else if (selectedInstallation?.Version is null)
                EditorPlayButton.IsEnabled = MainDataModel.Default.ProgressBarState.AllowPlaying;
            else
                EditorPlayButton.IsEnabled = MainDataModel.Default.ProgressBarState.AllowPlaying && selectedInstallation.Version?.Compare(Constants.GetMinimumEditorVersion(selectedInstallation.VersionType)) <= 0;
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
