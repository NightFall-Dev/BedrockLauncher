﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using BedrockLauncher.ViewModels;

namespace BedrockLauncher.Pages.Welcome
{
    /// <summary>
    /// Логика взаимодействия для WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : Page
    {
        public WelcomePagesSwitcher pageSwitcher = new WelcomePagesSwitcher();
        public WelcomePage()
        {
            InitializeComponent();
        }

        private void Page_Initialized(object sender, EventArgs e)
        {
            pageSwitcher.Init(this);
        }
    }

    public partial class WelcomePagesSwitcher
    {
        public static WelcomePage welcomePage;
        public static WelcomePageOne pageOne;
        public static WelcomePageTwo pageTwo;
        public static WelcomePageThree pageThree;
        public static WelcomePageFive pageFive;
        public void Init(WelcomePage page)
        {
            welcomePage = page;
        }
        public void MoveToPage(byte page, bool backup = false)
        {
            switch (page)
            {
                case 1:
                    Page1();
                    break;
                case 2:
                    Page2();
                    break;
                case 3:
                    Page3();
                    break;
                case 5:
                    Page5();
                    break;
                case 6:
                    Page6(backup);
                    break;
            }

            void Page1()
            {
                if (pageOne == null)
                {
                    pageOne = new WelcomePageOne();
                    welcomePage.WelcomePageFrame.Navigate(pageOne);
                }
                else { welcomePage.WelcomePageFrame.Navigate(pageOne); }
            }

            void Page2()
            {
                if (pageTwo == null)
                {
                    pageTwo = new WelcomePageTwo();
                    welcomePage.WelcomePageFrame.Navigate(pageTwo);
                }
                else { welcomePage.WelcomePageFrame.Navigate(pageTwo); }
            }

            void Page3()
            {
                if (MainDataModel.Default.Config.profiles.Count() != 0)
                {
                    Properties.LauncherSettings.Default.CurrentProfileUUID = MainDataModel.Default.Config.profiles.FirstOrDefault().Key;
                    Properties.LauncherSettings.Default.Save();
                    MoveToPage(5);
                }
                else
                {
                    if (pageThree == null)
                    {
                        pageThree = new WelcomePageThree();
                        welcomePage.WelcomePageFrame.Navigate(pageThree);
                    }
                    else { welcomePage.WelcomePageFrame.Navigate(pageThree); }
                }
            }



            void Page5()
            {
                if (pageFive == null)
                {
                    pageFive = new WelcomePageFive();
                    welcomePage.WelcomePageFrame.Navigate(pageFive);
                }
                else { welcomePage.WelcomePageFrame.Navigate(pageFive); }
            }

            void Page6(bool _backup)
            {
                if (_backup)
                {
                    Task.Run(Handlers.BackupHandler.BackupAllSaveData);
                }
                Task.Run(Program.OnApplicationRefresh);
                ViewModels.MainViewModel.Default.SetOverlayFrame(null, true);
                Properties.LauncherSettings.Default.IsFirstLaunch = false;
                Properties.LauncherSettings.Default.Save();
            }
        }
    }
}
