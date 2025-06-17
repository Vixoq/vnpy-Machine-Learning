using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace easyMoney.Utilities
{
    public static class LocalizationHelper
    {
        public static void SetThreadLocale()
        {
            if (Parameters.Language.Equals(Consts.Language.English))
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = Consts.Language.EnglishCulture;
            }
            else if (Parameters.Language.Equals(Consts.Language.Russian))
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = Consts.Language.RussianCulture;
            }
            else
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            }
        }

    }
}
