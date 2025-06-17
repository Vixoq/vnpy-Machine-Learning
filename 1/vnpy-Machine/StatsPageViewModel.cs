using Microcharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Entry = Microcharts.ChartEntry;


namespace FinanceManager.ViewModels
{
    class StatsPageViewModel : BaseViewModel
    {

        // Top Part 
        private DateTime _currentShowDate = DateTime.Now;
        public DateTime CurrentShowDate
        {
            get { return _currentShowDate; }
            set
            {
                if (_currentShowDate != value)
                {
                    _currentShowDate = value;
                    OnPropertyChanged(nameof(CurrentShowDate));
                }
            }
        }
        
        private Style _colorNextButton = (Style)Application.Current.Resources["MainButtonUnChecked"];
        public Style ColorNextButton
        {
            get { return _colorNextButton; }
            set
            {
                if (_colorNextButton != value)
                {
                    _colorNextButton = value;
                    OnPropertyChanged(nameof(ColorNextButton));
                }
            }
        }

        private Style _colorPrevButton = (Style)Application.Current.Resources["MainButtonUnChecked"];
        public Style ColorPrevButton
        {
            get { return _colorPrevButton; }
            set
            {
                if (_colorPrevButton != value)
                {
                    _colorPrevButton = value;
                    OnPropertyChanged(nameof(ColorPrevButton));
                }
            }
        }

        // Chart Part 
        private string _currentAppliedFilter = "OverView";
        public string CurrentAppliedFilter
        {
            get { return _currentAppliedFilter; }
            set
            {
                if (_currentAppliedFilter != value)
                {
                    _currentAppliedFilter = value;
                    ApplyeOverView.Execute(value);
                    OnPropertyChanged(nameof(CurrentAppliedFilter));
                }
            }
        }

        public Chart GrafData { get; set; }

        // Info Part 
        private float _balance;
        public float Balance
        {
            get { return _balance; }
            set
            {
                if (_balance != value)
                {
                    _balance = value;
                    OnPropertyChanged(nameof(Balance));
                }
            }
        }

        private string _totalMessage;
        public string TotalMessage
        {
            get { return _totalMessage; }
            set
            {
                if (_totalMessage != value)
                {
                    _totalMessage = value;
                    OnPropertyChanged(nameof(TotalMessage));
                }
            }
        }
    
        private Color _balanceColor;
        public Color BalanceColor
        {
            get { return _balanceColor; }
            set
            {
                if (_balanceColor != value)
                    _balanceColor = value;
                OnPropertyChanged(nameof(BalanceColor));
            }
        }

        public float IncomeSum { get; set; }
        public float ExpencesSum { get; set; }

        // Commands 
        public ICommand PrevMonth { get; private set; }
        public ICommand NextMonth { get; private set; }
        public ICommand ApplyeOverView { get; private set; }

        public async void ValueChangeMethod(string grafType)
        {
            if (grafType == "OverView")
            {
                GrafData = await Services.ChartGenerator.GetOverView(CurrentShowDate);
                IncomeSum = await Services.DatabaseConnection.GetFunctionResult($"SELECT SUM(Price) FROM \"Transaction\" WHERE Type = \"Income\" ");
                ExpencesSum = await Services.DatabaseConnection.GetFunctionResult($"SELECT SUM(Price) FROM \"Transaction\" WHERE Type = \"Expense\" ");
                Balance = IncomeSum - ExpencesSum;
                if (Balance < 0)
                {
                    BalanceColor = Color.Red;
                }
                else
                {
                    BalanceColor = Color.Green;
                }
                TotalMessage = "Balance: ";
            }
            else if (grafType == "Incoms")
            {
                GrafData = await Services.ChartGenerator.GetIncomesGraf(CurrentShowDate);
                Balance = IncomeSum;
                BalanceColor = (Color)Application.Current.Resources["BackgroundDark"];
                TotalMessage = "Total for Incoms: ";
            }
            else if (grafType == "Expences")
            {
                GrafData = await Services.ChartGenerator.GetExpencesCategory(CurrentShowDate);
                Balance = ExpencesSum;
                BalanceColor = (Color)Application.Current.Resources["BackgroundDark"];

                TotalMessage = "Total for ExpencesCategory: ";
            }


            OnPropertyChanged(nameof(GrafData));
        }

        public StatsPageViewModel()
        {
            Task.Run(async () =>
            {
                GrafData = await Services.ChartGenerator.GetOverView(CurrentShowDate);
                IncomeSum = await Services.DatabaseConnection.GetFunctionResult($"SELECT SUM(Price) FROM \"Transaction\" WHERE Type = \"Income\" ");
                ExpencesSum = await Services.DatabaseConnection.GetFunctionResult($"SELECT SUM(Price) FROM \"Transaction\" WHERE Type = \"Expense\" ");
                Balance = IncomeSum - ExpencesSum;
                if (Balance < 0)
                {
                    BalanceColor = Color.Red;
                }
                else
                {
                    BalanceColor = Color.Green;
                }

                TotalMessage = "Balance: ";
            }).Wait();

            NextMonth = new Command(async =>
            {
                DateTime dt = new DateTime(DateTime.Now.Year, 12, 1);

                if (CurrentShowDate.Month != dt.Month)
                {
                    CurrentShowDate = CurrentShowDate.AddMonths(1);

                    ColorPrevButton = (Style)Application.Current.Resources["MainButtonUnChecked"];

                    if (CurrentShowDate.Month == dt.Month)
                        ColorNextButton = (Style)Application.Current.Resources["MainButtonChecked"];

                    ValueChangeMethod(CurrentAppliedFilter);
                }
            });

            PrevMonth = new Command(async =>
            {
                DateTime dt = new DateTime(DateTime.Now.Year, 1, 1);

                if (CurrentShowDate.Month != dt.Month)
                {
                    CurrentShowDate = CurrentShowDate.AddMonths(-1);
                    ColorNextButton = (Style)Application.Current.Resources["MainButtonUnChecked"];

                    if (CurrentShowDate.Month == dt.Month)
                        ColorPrevButton = (Style)Application.Current.Resources["MainButtonChecked"];
                    ValueChangeMethod(CurrentAppliedFilter);
                }
            });

            ApplyeOverView = new Command<string>(value =>
                {
                    ValueChangeMethod(value);
                });
        }
    }
}