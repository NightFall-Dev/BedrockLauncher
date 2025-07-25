﻿using System;
using System.Collections.Generic;
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
using BedrockLauncher.Classes;
using BedrockLauncher.ViewModels;
using System.Collections.ObjectModel;
using BedrockLauncher.UpdateProcessor.Extensions;

namespace BedrockLauncher.Pages.Preview.Installation
{


    public partial class EditInstallationScreen : Page
    {

        private bool IsEditMode = false;

        public EditInstallationsPageViewModel ViewModel { get; set; } = new EditInstallationsPageViewModel();


        public EditInstallationScreen(BLInstallation i = null)
        {
            this.DataContext = ViewModel;
            InitializeComponent();
            if (i != null) UpdateEditingFields(i);
            else UpdateAddingFields();
        }
        private void UpdateAddingFields()
        {
            InstallationIconSelect.Init();
        }

        private void UpdateEditingFields(BLInstallation i)
        {
            IsEditMode = true;

            ViewModel.SelectedVersionUUID = i.VersionUUID;
            ViewModel.InstallationName = i.DisplayName;
            ViewModel.SelectedUUID = i.InstallationUUID;

            InstallationIconSelect.Init(i);

            Header.SetResourceReference(TextBlock.TextProperty, "EditInstallationScreen_AltTitle");
            CreateButton.SetResourceReference(Button.ContentProperty, "EditInstallationScreen_AltCreateButton");
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModels.MainViewModel.Default.SetOverlayFrame(null);
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsEditMode) UpdateInstallation();
            else CreateInstallation();
        }

        private MCVersion GetVersion(string uuid)
        {
            return MainDataModel.Default.Versions.Where(x => x.UUID == uuid).FirstOrDefault();
        }

        private void UpdateInstallation()
        {
            MainDataModel.Default.Config.Installation_Edit(ViewModel.SelectedUUID, ViewModel.InstallationName, GetVersion(ViewModel.SelectedVersionUUID), ViewModel.InstallationName, InstallationIconSelect.IconPath, InstallationIconSelect.IsIconCustom);
            MainViewModel.Default.SetOverlayFrame(null);
        }

        private void CreateInstallation()
        {
            MainDataModel.Default.Config.Installation_Create(ViewModel.InstallationName, GetVersion(ViewModel.SelectedVersionUUID), ViewModel.InstallationName, InstallationIconSelect.IconPath, InstallationIconSelect.IsIconCustom);
            MainViewModel.Default.SetOverlayFrame(null);
        }



        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModels.MainViewModel.Default.SetOverlayFrame(null);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            if (e.Item is MCVersion)
            {
                var version = (e.Item as MCVersion);
                if (VersionDbExtensions.DoesVerionArchMatch(Constants.CurrentArchitecture, version.Architecture)) e.Accepted = true;
                else if (ViewModel.SelectedVersionUUID == version.UUID) e.Accepted = true;
                else e.Accepted = false;
            }
            else e.Accepted = false;
        }

        private void RefreshVersions()
        {
            Dispatcher.Invoke(() =>
            {
                Handlers.FilterSortingHandler.Refresh(InstallationVersionSelect.ItemsSource);
            });
        }

        private async void MoreVersionsButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new EditInstallationVersionSelectScreen();
            ViewModels.MainViewModel.Default.SetDialogFrame(window);
            string result = await window.GetVersionUUID();
            if (!string.IsNullOrWhiteSpace(result))
            {
                ViewModel.SelectedVersionUUID = result;
                RefreshVersions();
                InstallationVersionSelect.SelectedValue = result;
            }
        }
    }
}
