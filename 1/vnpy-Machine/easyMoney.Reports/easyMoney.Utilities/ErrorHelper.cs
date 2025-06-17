using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace easyMoney.Utilities
{
    public static class ErrorHelper
    {
        public static class Errors
        {
            public const String PlanTemplateIsNull = "#1001, data error: template for plan is null";
            public const String PlanDestinationAccountIsNull = "#1002, data error: according to template there should be a destination account, but it is not";
            public const String UnknownSearchResult = "#1003, data error: unknown data type in search results";
            public const String PlanWithoutTemplate = "#1004, data error: data schema model was upgraded, this planned transaction is not supported, please delete this planned transaction and create it from scratch";
            public const String InvalidValidation = "#1005, data error: invalid validation criteria/rule";
            public const String InvalidTransaction = "#1006, data error: invalid combination of transaction type/template";
            public const String InvalidPlan = "#1007, data error: invalid combination of planned transaction type/template";
            public const String InvalidCallToValidate = "#2001, application error: invalid data passed to Validate()";
            public const String ErrorReadingData = "#2002, application error: problems reading user data";
            public const String ErrorEncryptedData = "#2005, application error: problems reading encrypted user data";
            public const String ErrorSavingData = "#2004, application error: problems saving user data";
            public const String ErrorShowingIntroduction = "#2003, application error: problems showing introduction";
            public const String UnhandledApplicationError = "#2999, application error: unhanded exception";
        }

        public static void ShowErrorBox(String message = null, Exception e = null, bool terminateApplication = false)
        {
            String msg = String.IsNullOrEmpty(message) ? String.Empty : message;
            String exMsg = (e == null) ? String.Empty : describeException(e);
            if (message != null)
            {
                Log.Write(message);
            }
            if (e != null)
            {
                Log.Write(e, 2);
            }
            MessageBox.Show(String.Format(Resources.Labels.ErrorMessageFormat, msg, exMsg), 
                Resources.Labels.ErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (terminateApplication)
            {
                Application.Exit();
            }
        }

        private static String describeException(Exception e)
        {
            return String.Format(Consts.UI.ExceptionFormat, e.GetType().Name, e.Message, 
                e.TargetSite.ReflectedType.FullName, e.TargetSite.Name);       
        }
    }
}
