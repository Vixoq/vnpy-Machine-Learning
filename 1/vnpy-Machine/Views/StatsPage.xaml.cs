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
    public partial class StatsPage : ContentPage
    {
        public Button AppliedFilter { get; set; } = new Button { Text = "-" };

        public StatsPage()
        {
            InitializeComponent();

            BindingContext = new ViewModels.StatsPageViewModel();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;
           
            if (btn != null && btn.Text != AppliedFilter.Text)
            {
                btn.Style = (Style)Application.Current.Resources["MainButtonUnChecked"];
                AppliedFilter.Style = (Style)Application.Current.Resources["MainButtonChecked"];
                AppliedFilter = btn;
                (BindingContext as ViewModels.StatsPageViewModel).CurrentAppliedFilter = btn.Text;
            }
        }

        private async void settings_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }
    }
}