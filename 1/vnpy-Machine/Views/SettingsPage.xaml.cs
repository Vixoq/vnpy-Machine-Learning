using FinanceManager.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FinanceManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        Button CheckButton { get; set; }
        public SettingsPage()
        {
            InitializeComponent();
            switch (Settings.Theme)
            {
                case 0:
                    defaultTheme.Style = (Style)Application.Current.Resources["MainButtonUnChecked"];
                    CheckButton = defaultTheme;
                    break;
                case 1:
                    lightTheme.Style = (Style)Application.Current.Resources["MainButtonUnChecked"];
                    CheckButton = lightTheme;
                    break;
                case 2:
                    darkTheme.Style = (Style)Application.Current.Resources["MainButtonUnChecked"];
                    CheckButton = darkTheme;
                    break;
            }
        }

        private void defaultTheme_Clicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            if (btn == null)
                return;
            if (CheckButton.Text == btn.Text)
                return;

            CheckButton.Style = (Style)Application.Current.Resources["MainButtonChecked"];
            switch (btn.Text)
            {
                case "Default":
                    defaultTheme.Style = (Style)Application.Current.Resources["MainButtonUnChecked"];
                    Settings.Theme = 0;
                    CheckButton = defaultTheme;
                    break;
                case "Light":
                    lightTheme.Style = (Style)Application.Current.Resources["MainButtonUnChecked"];
                    Settings.Theme = 1;
                    CheckButton = lightTheme;
                    break;
                case "Dark":
                    darkTheme.Style = (Style)Application.Current.Resources["MainButtonUnChecked"];
                    Settings.Theme = 2;
                    CheckButton = darkTheme;
                    break;
            }

            Settings.SetTheme();
        }
    }
}