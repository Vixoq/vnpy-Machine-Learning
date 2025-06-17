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
    public partial class AddAccountPage : ContentPage
    {
        public string Name { get; set; }
        public float InitialBalance { get; set; } = 0;

        public AddAccountPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (Name == null || Name == "" || InitialBalance == null)
            {
                await DisplayAlert("Alert!", "Fill all fields", "Ok");
                return;
            }

            if (await Services.DatabaseConnection.VerifyIfAccExist(Name))
            {
                await Services.DatabaseConnection.AddAccount(
                    new Models.Account
                    {
                        Name = Name,
                        Balance = InitialBalance
                    });
                await DisplayAlert("Success!", "Account Added", "Ok");

            }
            else
                await DisplayAlert("Alert!", "Account Exist, try another Name", "Ok");

            accountName.Text = "";
            accountBalance.Text = "0";

            Name = "";
            InitialBalance = 0;
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainTabbedPage());
        }
    }
}