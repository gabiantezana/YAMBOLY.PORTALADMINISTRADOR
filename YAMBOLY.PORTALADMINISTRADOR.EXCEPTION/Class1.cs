using System;
using System.Configuration;
using System.IO;
using System.Linq;
using YAMBOLY.PORTALADMINISTRADOR.MODEL;

namespace YAMBOLY.PORTALADMINISTRADOR.EXCEPTION
{
    public class ExceptionHelper
    {
        private ExceptionHelper() { }

        public static void LogException(Exception exc)
        {

            String fileName = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            String logFile = @"D:\" + fileName;
            String ruta = ConfigurationManager.AppSettings["RUTA_LOG"];

            if (!String.IsNullOrEmpty(ruta))
                logFile = ruta.ToString() + fileName;

            if (!File.Exists(logFile))
                File.Create(logFile).Close();

            StreamWriter sw = new StreamWriter(logFile, true);
            sw.WriteLine("********** {0} **********", DateTime.Now);

            sw.Write("Exception Type: ");
            sw.WriteLine(exc.GetType().ToString());
            sw.WriteLine("Exception: " + exc.Message);
            sw.WriteLine("Stack Trace: ");
            if (exc.InnerException != null)
            {
                sw.Write("Inner Exception Type: ");
                sw.WriteLine(exc.InnerException.GetType().ToString());
                sw.Write("Inner Exception: ");
                sw.WriteLine(exc.InnerException.Message);
                sw.Write("Inner Source: ");
                sw.WriteLine(exc.InnerException.Source);
                if (exc.InnerException.StackTrace != null)
                {
                    sw.WriteLine("Inner Stack Trace: ");
                    sw.WriteLine(exc.InnerException.StackTrace);
                }
            }

            if (exc.StackTrace != null)
            {
                sw.WriteLine(exc.StackTrace);
                sw.WriteLine();
            }
            sw.Close();
        }

        public static void LogException(Exception exc, DataContext DataContext)
        {
            String ruta = ConfigurationManager.AppSettings["RUTA_LOG"];
            if (!String.IsNullOrEmpty(ruta))
                logFile = ruta.ToString() + fileName;

            if (!File.Exists(logFile))
                File.Create(logFile).Close();

            using (StreamWriter sw = new StreamWriter(logFile, true))
            {
                sw.WriteLine("******************************************* {0} *****************************************", DateTime.Now);

                sw.WriteLine("Browser: " + DataContext.Browser.Browser);
                sw.WriteLine("Browser Id: " + DataContext.Browser.Id);
                sw.WriteLine("Browser Platform: " + DataContext.Browser.Platform);
                sw.WriteLine("Browser Version: " + DataContext.Browser.Version);

                if (exc.InnerException != null)
                {
                    sw.Write("Inner Exception Type: ");
                    sw.WriteLine(exc.InnerException.GetType().ToString());
                    sw.Write("Inner Exception: ");
                    sw.WriteLine(exc.InnerException.Message);
                    sw.Write("Inner Source: ");
                    sw.WriteLine(exc.InnerException.Source);
                    if (exc.InnerException.StackTrace != null)
                    {
                        sw.WriteLine("Inner Stack Trace: ");
                        sw.WriteLine(exc.InnerException.StackTrace);
                    }
                }

                sw.WriteLine("Session Values: ");
                int sessionLength = DataContext.session.Contents.Count;
                for (int i = 0; i < sessionLength; i++)
                {
                    if (DataContext.session[i] != null)
                        sw.WriteLine("Key: " + DataContext.session.Keys[i] + " Value: " + DataContext.session[i].ToString());
                }

                sw.Write("Exception Type: ");
                sw.WriteLine(exc.GetType().ToString());
                sw.WriteLine("Exception: " + exc.Message);

                if (exc.StackTrace != null)
                {
                    sw.WriteLine(exc.StackTrace);
                    sw.WriteLine();
                }
                sw.Close();
            }
            StoreException(exc, DataContext, DateTime.Now);
            GC.Collect();
        }

        private static void StoreException(Exception ex, DataContext DataContext, DateTime timeStamp)
        {
            try
            {
                ErrorLog excepcion = new ErrorLog();
                excepcion.internalException = ex.InnerException != null ? ex.InnerException.Message : String.Empty;
                excepcion.errorMessage = ex.Message;
                excepcion.browser = DataContext.Browser.Browser;
                excepcion.stackTrace = ex.StackTrace;
                excepcion.errorType = ex.GetType().ToString();

                int sessionLength = DataContext.session.Contents.Count;
                string sessionKeys = string.Empty;
                for (int i = 0; i < sessionLength; i++)
                {
                    var dataSession = DataContext.session[i];
                    if (dataSession != null)
                    {
                        var dataToString = dataSession.ToString();
                        sessionKeys = "Key: " + DataContext.session.Keys[i] + " Value: " +
                            dataToString.Substring(0, dataToString.Length > 20 ? 20 : dataToString.Length) + " | ";
                    }
                }
                if (sessionKeys.Length > 3)
                    sessionKeys = sessionKeys.Remove(sessionKeys.Length - 3);//Solo para quitar el ultimo '|' de la cadena.
                excepcion.sessionKeys = sessionKeys;
                excepcion.errorTimeStamp = timeStamp.ToString();
                DataContext.context.ErrorLog.Add(excepcion);
                DataContext.context.SaveChanges();
            }
            catch (Exception intraEx)
            {
                LogException(intraEx);
            }
        }


        private static String fileName = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
        private static String logFile = @"C:\LOGS\YAMBOLY_PORTALADMINISTRADOR\" + fileName;
    }
}