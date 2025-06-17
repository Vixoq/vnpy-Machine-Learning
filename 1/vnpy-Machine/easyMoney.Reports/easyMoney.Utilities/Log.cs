using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace easyMoney.Utilities
{
    /// <summary>
    /// Logger class
    /// </summary>
    public static class Log
    {
        private const String LogError = "unable to dump object (probably, public object property thrown an exception)";
        private const String ExceptionMessageFormat = "{0} {1}";
        private readonly static String FileCreationErrorFormat = "Error creating application log!" + Environment.NewLine + "{0}";

        private static String fileName = null;
        private static object fileLock = new object();
        private static StreamWriter logWriter = null;

        static Log()
        {
            try
            {
                String logFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + 
                    Path.DirectorySeparatorChar + Consts.Application.ProfileFolder;
                if (!Directory.Exists(logFolder))
                {
                    Directory.CreateDirectory(logFolder);
                }

                fileName = logFolder + Path.DirectorySeparatorChar + Consts.Application.LogFileName;
            }
            catch (Exception e)
            {
                ErrorHelper.ShowErrorBox(String.Format(FileCreationErrorFormat, e.Message), e, true);
            }
        }

        /// <summary>
        /// Write entry to log
        /// </summary>
        /// <param name="message">Message to log</param>
        public static void Write(String message)
        {
            lock (fileLock)
            {
                openLog();
                writeLog(message);
                closeLog();
            }
        }

        /// <summary>
        /// Write entry to log
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="obj">Object to describe</param>
        public static void Write(String message, object obj)
        {
            lock (fileLock)
            {
                openLog();
                writeLog(message);
                try
                {
                    ObjectDescriptor descriptor = new ObjectDescriptor(logWriter);
                    descriptor.Describe(obj);
                }
                catch (Exception e)
                {
                    writeLog(LogError);
                    writeLog(describeException(e));
                }
                closeLog();
            }
        }

        /// <summary>
        /// Write entry to log
        /// </summary>
        /// <param name="e">Exception to describe</param>
        public static void Write(Exception e, int depth = 1)
        {
            lock (fileLock)
            {
                openLog();

                StackTrace sTrace = new StackTrace(true);
                writeLog(String.Format(Consts.UI.MethodGotExceptionFormat, sTrace.GetFrame(depth).GetMethod().DeclaringType.Name,
                    sTrace.GetFrame(depth).GetMethod().Name, describeException(e)));
                closeLog();
            }
        }

        /// <summary>
        /// Write entry to log
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="e">Exception to describe</param>
        public static void Write(String message, Exception e)
        {
            lock (fileLock)
            {
                openLog();
                writeLog(String.Format(ExceptionMessageFormat, message, describeException(e)));
                closeLog();
            }
        }

        /// <summary>
        /// Calling method name
        /// </summary>
        /// <returns>Method name</returns>
        public static String selfName()
        {
            StackTrace sTrace = new StackTrace(true);
            return String.Format(Consts.UI.MethodFormat, sTrace.GetFrame(1).GetMethod().DeclaringType.Name,
                sTrace.GetFrame(1).GetMethod().Name);
        }

        /// <summary>
        /// Returns described exception
        /// </summary>
        /// <param name="e">Exception</param>
        /// <returns>Information about exception</returns>
        private static String describeException(Exception e)
        {
            return String.Format(Consts.UI.ExceptionFullFormat, e.GetType().Name, e.Message,
                e.TargetSite.ReflectedType.FullName, e.TargetSite.Name, e.StackTrace);
        }

        /// <summary>
        /// Open log file
        /// </summary>
        private static void openLog()
        {
            logWriter = File.AppendText(fileName);
        }

        /// <summary>
        /// Write entry to log file
        /// </summary>
        /// <param name="message">Message to log</param>
        private static void writeLog(String message)
        {
            logWriter.WriteLine(Consts.UI.LogMessageFormat, DateTime.Now, message);
        }

        /// <summary>
        /// Close log file
        /// </summary>
        private static void closeLog()
        {
            logWriter.Close();
            logWriter = null;
        }
    }
}
