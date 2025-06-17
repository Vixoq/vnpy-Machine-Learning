using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Entry = Microcharts.ChartEntry;

namespace FinanceManager.Services
{
    public static class ChartGenerator
    {

        public static List<string> Categorys { get; set; } = new List<string>
        {
            "Shopping",
            "Utilities",
            "Services",
            "Food",
            "Gadgets"
        };

        public static List<string> ChartColors { get; set; } = new List<string>
        {
            "ChartColor1",
            "ChartColor2",
            "ChartColor7",
            "ChartColor8",
            "ChartColor9",
            "ChartColor10",
            "ChartColor3",
            "ChartColor4",
            "ChartColor5",
            "ChartColor6",
        };


        public static async Task<Chart> GetOverView(DateTime fromPeriod)
        {
            // "SELECT SUM(Price) FROM \"Transaction\" WHERE Type = \"Income\"" get sum from Income
            // "SELECT SUM(Price) FROM \"Transaction\" WHERE Type = \"Expense\"" get sum from Expense

            var incomeSum = await Services.DatabaseConnection.GetFunctionResult($"SELECT SUM(Price) FROM \"Transaction\" WHERE Type = \"Income\" ");
            var expenseSum = await Services.DatabaseConnection.GetFunctionResult($"SELECT SUM(Price) FROM \"Transaction\" WHERE Type = \"Expense\" ");
            Color color1 = (Color)Application.Current.Resources["ChartColor2"];
            Color color2 = (Color)Application.Current.Resources["ChartColor1"];

            List<Entry> entrys = new List<Entry>
            {
                new Entry(incomeSum)
                {
                    Color = SKColor.Parse(color1.ToHex()),
                    ValueLabelColor = SKColor.Parse(color1.ToHex()),
                    Label = "Income",
                    ValueLabel = $"{string.Format("{0:N2}", incomeSum)}Lei"
                },
                new Entry(expenseSum)
                {
                    Color = SKColor.Parse(color2.ToHex()),
                    ValueLabelColor = SKColor.Parse(color2.ToHex()),
                    Label = "Expenses",
                    ValueLabel = $"{string.Format("{0:N2}", expenseSum)}Lei"
                }
            };

            return new BarChart
            {
                Entries = entrys,
                LabelTextSize = 40f,
                MaxValue = 800,
                Margin = 50,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                BackgroundColor = SKColor.Parse(Color.Transparent.ToHex())
            };
        }


        public static async Task<Chart> GetIncomesGraf(DateTime fromPeriod)
        {
            var listOfIncom = await Services.DatabaseConnection.GetIncomeTransactions();
            Entry entry;
            List<Entry> entrys = new List<Entry>();
            Color color;
            Random rand = new Random();
            int i = 0;
            foreach (var incom in listOfIncom)
            {
                color = (Color)Application.Current.Resources[ChartColors[i]];
                entry = new Entry(incom.Price)
                {
                    Color = SKColor.Parse(color.ToHex()),
                    ValueLabelColor = SKColor.Parse(color.ToHex()),
                    Label = $"{incom.Name}",
                    ValueLabel = $"{string.Format("{0:N2}", incom.Price)}Lei"
                };

                entrys.Add(entry);
                i++;
            }

            return new RadialGaugeChart { Entries = entrys, LabelTextSize = 40f, BackgroundColor = SKColor.Parse(Color.Transparent.ToHex()) };
        }

        public static async Task<Chart> GetExpencesCategory(DateTime fromPeriod)
        {
            Entry entry;
            List<Entry> entrys = new List<Entry>();
            Color color;
            Random rand = new Random();
            int i = 0;
            foreach (string cateory in Categorys)
            {
                var categorySum = await Services.DatabaseConnection.GetFunctionResult($"SELECT SUM(Price) FROM \"Transaction\" WHERE Category = \"{cateory}\" ");
                if (categorySum > 0)
                {
                    color = (Color)Application.Current.Resources[ChartColors[i]];
                    entry = new Entry(categorySum)
                    {
                        Color = SKColor.Parse(color.ToHex()),
                        ValueLabelColor = SKColor.Parse(color.ToHex()),
                        Label = $"{cateory}",
                        ValueLabel = $"{string.Format("{0:N2}", categorySum)}Lei"
                    };

                    entrys.Add(entry);
                    i++;
                }
            }

            return new BarChart
            {
                Entries = entrys,
                LabelTextSize = 40f,
                Margin = 50,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                BackgroundColor = SKColor.Parse(Color.Transparent.ToHex())
            };
        }
    }
}
