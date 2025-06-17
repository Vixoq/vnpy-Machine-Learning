using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace easyMoney.Utilities
{
    public abstract class Consts
    {
        public abstract class Application
        {
            public const String Version = "0.9";
            public const int SchemaVersion = 2;
            public const int SchemaVersionEncrypted = 2;
            public const String VersionCodeName = "Kraz";
            public const String UpdateCheckURL = "http://sites.google.com/site/dtazetdinov/easymoney/current-release.txt";
            public const int AutoUpdatePeriod = 14;
            public const String SiteURL = "http://easymoney-manager.googlecode.com";
            public const String TrackerURL = "http://code.google.com/p/easymoney-manager/issues/list";
            public const String ProfileFolder = "easyMoney";
            public const String LogFileName = "easyMoney.log";
            public const String XmlDataFileName = "easyMoney-data.xml";
            public const String DefaultExtension = "xemdf";
            public const String DefaultFileName = "easyMoney-data." + DefaultExtension;
            public const String BackupFileNameFormat = "easyMoney-data-{0:yyyy-MM-dd}." + DefaultExtension;
            public const String BackupFileSearchPattern = "easyMoney-data-*" + DefaultExtension;
        }

        public abstract class ImageKeys
        {
            public const String TabWelcome = "Welcome";
            public const String TabTransactions = "Transactions";
            public const String TabPlans = "Plans";
            public const String TabSearch = "Search";
            public const String TabAccounts = "Accounts";
            public const String TabSettings = "Settings";
            public const String TabReports = "Reports";
            public const String TabAbout = "About";
        }

        public abstract class UI
        {
            public const String ImportedTransactionsListFormat = "{0}[{1:d}] {2}";
            public const String EarnedFormat = "+ {0:C}";
            public const String SpentFormat = "- {0:C}";
            public const String RecentTransactionToolTipFormat = "{0}{1}{2}: {3}";
            public const String FullVersionFormat = "{0} ({1})";
            public const String NumericFormat = "N2";
            public const String CurrencyFormat = "C";
            public const String DateTimeFormat = "G";
            public const String DateFormat = "d";
            public const String YearMonthFormat = "Y";
            public const String MonthFormat = "MMMM";
            public const int MonthBalanceForecastPeriod = 6;
            public const String CurrencyWithAccountFormat = "{0} - {1}";
            public const String PlanToolTipFormat = "[{0}] {1}, {2}{3}; {4}";
            public static readonly String DefaultChartToolTipFormat = "#VALX" + Environment.NewLine + "{0}: #VALY{{N2}}";
            public static readonly String SmallChartToolTip = "#VALX" + Environment.NewLine + "#VALY{N2}";
            public static readonly String ChartWithPercentageToolTip = "#VALX" + Environment.NewLine + "#VALY{N2} (#PERCENT)";

            public const int TagsMaxFontIncrease = 16;

            public const String ExceptionFormat = "{0}: {1} (thrown by {2}.{3})";
            public const String MethodFormat = "{0}.{1}()";
            public const String MethodGotExceptionFormat = "{0}.{1}() got {2}";
            public const String LogMessageFormat = "[{0:s}] {1}";
            public static readonly String ExceptionFullFormat = "{0}: {1} (thrown by {2}.{3})" + Environment.NewLine + "{4}";
            
            public const String EnumerableSeparator = ", ";
            public static readonly String[] WordDividers = { " ", ",", ".", ";" };
        }

        public abstract class Reports
        {
            public const double DifferenceWarningThreshold = 0.2;
            public const double DifferenceErrorThreshold = 0.5;
        }

        public abstract class Updater
        {
            public static readonly String[] FieldDivider = { "|" };
            public const String URLPathDivider = "/";
            public const String ErrorFormat ="UpdateHelper.UpdateDownloaded: URL=[{0}], LocalFile=[{1}]";
            public const String ExceptionPrefix = "UpdateHelper.UpdateDownloaded: AsyncCompletedEventArgs object contents:";
        }

        public abstract class Keeper
        {
            public const int MaxHistoryEntries = 1024;
            public const String HistorySearchID = "SEARCH";
            public const String TransactionTitleHistoryIDFormat = "TRANSACTIONTITLE{0}";
            public const String AccountTitleHistoryID = "ACCOUNTTITLE";
            public const String UserMetadataPrefix = "USR-";
            public const double DefaultExchangeRate = 0.01;
            public const int AmountRoundingDigits = 2;

            public const String RoubleCulture = "ru-RU";
            public const String USDollarCulture = "en-US";
            public const String EuroCulture = "fr-FR";
        }

        public abstract class Language
        {
            public const String SystemDefault = "system";
            public const String English = "en";
            public const String Russian = "ru";

            public static readonly CultureInfo RussianCulture = new CultureInfo(Consts.Language.Russian);
            public static readonly CultureInfo EnglishCulture = new CultureInfo(Consts.Language.English);
        }
    }
}
