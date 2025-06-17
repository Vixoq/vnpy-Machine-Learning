using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FinanceManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExpencesPage : ContentPage
    {
        public ObservableCollection<string> BillListFilters { get; set; } = new ObservableCollection<string>
        {
            "Income",
            "Expenses",
            "Global",
        };

        public ObservableCollection<Models.Transaction> TransactionsList { get; set; }

        public Button CurrentCheck { get; set; } = new Button { Text = "-" };

        public ExpencesPage()
        {
            InitializeComponent();
            BindingContext = this;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (CurrentCheck == null)
            {
                TransactionsList = new ObservableCollection<Models.Transaction>(await Services.DatabaseConnection.GetGlobalTransactions());
                expensesList.ItemsSource = TransactionsList;
            }
        }

        private async void Filter_Clicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            if (btn != null && btn.Text != CurrentCheck.Text)
            {
                btn.Style = (Style)Application.Current.Resources["MainButtonUnChecked"];
                CurrentCheck.Style = (Style)Application.Current.Resources["MainButtonChecked"];
                CurrentCheck = btn;

                switch (btn.Text)
                {
                    case "Income":
                        TransactionsList = new ObservableCollection<Models.Transaction>(await Services.DatabaseConnection.GetIncomeTransactions());
                        break;
                    case "Expenses":
                        TransactionsList = new ObservableCollection<Models.Transaction>(await Services.DatabaseConnection.GetExpensesTransactions());
                        break;
                    default:
                        TransactionsList = new ObservableCollection<Models.Transaction>(await Services.DatabaseConnection.GetGlobalTransactions());
                        break;
                }

                expensesList.ItemsSource = TransactionsList;
                selectedFilter.Text = btn.Text;

                CurrentCheck = btn;
            }
        }

        private async void addTransaction_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddTransactionPage());
        }

        private void expemcesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void hideFilters_Clicked(object sender, EventArgs e)
        {
            if (filtersList.IsVisible)
            {
                filtersList.IsVisible = false;
                selectedFilter.IsVisible = true;
            }
            else
            {
                filtersList.IsVisible = true;
                selectedFilter.IsVisible = false;
            }
        }
    }
}