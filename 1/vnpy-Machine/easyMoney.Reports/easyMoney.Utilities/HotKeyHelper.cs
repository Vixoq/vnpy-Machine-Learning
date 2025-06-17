using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace easyMoney.Utilities
{
    public static class HotKeyHelper
    {
        public static Keys lastKey = Keys.None;
        public static KeyShortcut GetShortcut(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                return KeyShortcut.Cancel;
            }

            if (e.Control)
            {
                if (e.Alt)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.D:
                            return KeyShortcut.NewDebitAccount;

                        case Keys.C:
                            return KeyShortcut.NewCreditAccount;

                        case Keys.I:
                            return KeyShortcut.NewIncomeTransaction;

                        case Keys.P:
                            return KeyShortcut.NewPaymentTransaction;

                        case Keys.M:
                            return KeyShortcut.ShowMonthBalance;
                    }
                }
                else
                {
                    switch (e.KeyCode)
                    {
                        case Keys.S:
                            return KeyShortcut.Save;

                        case Keys.E:
                            return KeyShortcut.Edit;

                        case Keys.L:
                            return KeyShortcut.Cancel;

                        case Keys.D:
                            return KeyShortcut.Delete;

                        case Keys.T:
                            return KeyShortcut.ShowTransactions;

                        case Keys.A:
                            return KeyShortcut.ShowAccounts;

                        case Keys.R:
                            return KeyShortcut.ShowReports;

                        case Keys.P:
                            return KeyShortcut.ShowPlans;

                        case Keys.G:
                            return KeyShortcut.Search;
                    }
                }
            }
            e.SuppressKeyPress = false;
            return KeyShortcut.Undefined;
        }

    }
}
