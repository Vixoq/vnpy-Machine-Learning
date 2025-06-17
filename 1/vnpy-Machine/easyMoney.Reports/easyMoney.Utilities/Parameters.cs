using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace easyMoney.Utilities
{
    public static class Parameters
    {
        private const String sMainWindowTop = "MainWindowTop";
        private const String sMainWindowLeft = "MainWindowLeft";
        private const String sMainWindowHeight = "MainWindowHeight";
        private const String sMainWindowWidth = "MainWindowWidth";
        private const String sMainWindowMaximized = "MainWindowMaximized";
        private const String sIntroductionShown = "IntroductionShown";
        private const String sUpdateInfoShown = "UpdateInfoShown";
        private const String sShowFilenameInTitle = "ShowFilenameInTitle";
        private const String sShowOpenDialogEachStart = "ShowOpenDialogEachStart";
        private const String sKeepArchivesDays = "KeepArchivesDays";

        private const String sBranch = @"SOFTWARE\easyMoney";
        private const String sLanguage = "Language";
        private const String sLastUpdateDate = "LastUpdateDate";
        private const String sCheckForUpdates = "CheckForUpdates";
        private const String sSearchTabSplitterPosition = "SearchTabSplitterPosition";
        
        public static int MainWindowTop { get; set; }
        public static int MainWindowLeft { get; set; }
        public static int MainWindowHeight { get; set; }
        public static int MainWindowWidth { get; set; }
        public static int SearchTabSplitterPosition { get; set; }
        public static int KeepArchivesDays { get; set; }
        public static bool MainWindowMaximized { get; set; }
        public static bool CheckForUpdates { get; set; }
        public static bool IntroductionShown { get; set; }
        public static bool ShowFilenameInTitle { get; set; }
        public static bool ShowOpenDialogEachStart { get; set; }
        public static String UpdateInfoShown { get; set; }
        public static String Language { get; set; }
        public static DateTime LastUpdateDate { get; set; }

        public static void Reset()
        {
            MainWindowTop = -1;
            MainWindowLeft = -1;
            MainWindowHeight = -1;
            MainWindowWidth = -1;
            SearchTabSplitterPosition = -1;
            KeepArchivesDays = 30;
            MainWindowMaximized = false;
            CheckForUpdates = true;
            IntroductionShown = false;
            ShowFilenameInTitle = false;
            ShowOpenDialogEachStart = false;
            UpdateInfoShown = String.Empty;
            Language = String.Empty;
            LastUpdateDate = DateTime.MinValue;
        }

        static Parameters()
        {
            try
            {
                Reset();

                RegistryKey branch = Registry.CurrentUser.OpenSubKey(sBranch);
                if (branch != null)
                {
                    MainWindowTop = (int)branch.GetValue(sMainWindowTop, -1);
                    MainWindowLeft = (int)branch.GetValue(sMainWindowLeft, -1);
                    MainWindowHeight = (int)branch.GetValue(sMainWindowHeight, -1);
                    MainWindowWidth = (int)branch.GetValue(sMainWindowWidth, -1);
                    SearchTabSplitterPosition = (int)branch.GetValue(sSearchTabSplitterPosition, -1);
                    KeepArchivesDays = (int)branch.GetValue(sKeepArchivesDays, 30);
                    MainWindowMaximized = Boolean.Parse(branch.GetValue(sMainWindowMaximized, Boolean.FalseString).ToString());
                    CheckForUpdates = Boolean.Parse(branch.GetValue(sCheckForUpdates, Boolean.TrueString).ToString());
                    IntroductionShown = Boolean.Parse(branch.GetValue(sIntroductionShown, Boolean.FalseString).ToString());
                    ShowFilenameInTitle = Boolean.Parse(branch.GetValue(sShowFilenameInTitle, Boolean.FalseString).ToString());
                    ShowOpenDialogEachStart = Boolean.Parse(branch.GetValue(sShowOpenDialogEachStart, Boolean.FalseString).ToString());
                    UpdateInfoShown = branch.GetValue(sUpdateInfoShown, String.Empty).ToString();
                    Language = branch.GetValue(sLanguage, String.Empty).ToString();
                    LastUpdateDate = DateTime.Parse(branch.GetValue(sLastUpdateDate, DateTime.MinValue.ToString()).ToString());
                }
            }
            catch (Exception e)
            {
                Log.Write(e);
            }
        }

        public static void Save()
        {
            try
            {
                RegistryKey branch = Registry.CurrentUser.OpenSubKey(sBranch, true);
                if (branch == null)
                {
                    branch = Registry.CurrentUser.CreateSubKey(sBranch);
                }

                if (branch != null)
                {
                    branch.SetValue(sMainWindowTop, MainWindowTop, RegistryValueKind.DWord);
                    branch.SetValue(sMainWindowLeft, MainWindowLeft, RegistryValueKind.DWord);
                    branch.SetValue(sMainWindowHeight, MainWindowHeight, RegistryValueKind.DWord);
                    branch.SetValue(sMainWindowWidth, MainWindowWidth, RegistryValueKind.DWord);
                    branch.SetValue(sSearchTabSplitterPosition, SearchTabSplitterPosition, RegistryValueKind.DWord);
                    branch.SetValue(sKeepArchivesDays, KeepArchivesDays, RegistryValueKind.DWord);
                    branch.SetValue(sMainWindowMaximized, MainWindowMaximized.ToString(), RegistryValueKind.String);
                    branch.SetValue(sCheckForUpdates, CheckForUpdates.ToString(), RegistryValueKind.String);
                    branch.SetValue(sIntroductionShown, IntroductionShown.ToString(), RegistryValueKind.String);
                    branch.SetValue(sShowFilenameInTitle, ShowFilenameInTitle.ToString(), RegistryValueKind.String);
                    branch.SetValue(sShowOpenDialogEachStart, ShowOpenDialogEachStart.ToString(), RegistryValueKind.String);
                    branch.SetValue(sLastUpdateDate, LastUpdateDate.ToString(), RegistryValueKind.String);
                    branch.SetValue(sUpdateInfoShown, UpdateInfoShown, RegistryValueKind.String);
                    branch.SetValue(sLanguage, Language, RegistryValueKind.String);
                }
            }
            catch (Exception e)
            {
                Log.Write(e);
            }
        }
    }
}
