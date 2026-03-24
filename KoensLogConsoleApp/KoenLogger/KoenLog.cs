using KoensLogConsoleApp.KLogger;

namespace KoensLogConsoleApp.Models
{
    internal class KoenLog : IKoenLog
    {
        private string _emailAddress = "";
        private string _connectionString = "";
        private string _pathToLogFile = @"C:\Users\koenv\source\repos\KoensLogConsoleApp\KoensLogConsoleApp\logFiles\";//KoensLogConsoleApp aanpassen
        private string _fileNamePrefix = "KoensLog_";
        private string _logFileName = "";
        private int _fileDeleteAfterDays = 0;
        private string outputText = "";

        public KoenLog()
        {
        }

        public OutputTarget OutputTarget { get; set; }

        public void Log(string logText, OutputType outputType)
        {
            outputText = outputType switch
            {
                OutputType.Error => "[ERROR] " + logText,
                OutputType.Info => "[Info] " + logText,
                _ => logText,
            };

            logText = DateTime.Now.ToString("HH:mm:ss") + "  "  + outputText.Trim();
            
            switch (OutputTarget)
            {
                case OutputTarget.Screen:
                    LogToScreen(logText);
                    break;
                case OutputTarget.File:
                    LogToFile(logText);
                    break;
                case OutputTarget.Email:
                    LogToEmail(logText);
                    break;
                case OutputTarget.Database:
                    LogToDatabase(logText);
                    break;
                default:
                    LogToFile(logText);
                    break;
            }

        }

        private static void LogToScreen(string logText)
        {
            Console.WriteLine($"[koenLog] {logText}");
        }

        private void LogToFile(string logText)
        {
            _logFileName = _fileNamePrefix + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            string path = Path.Combine(_pathToLogFile, _logFileName);

            if (!File.Exists(path))
            {
                using StreamWriter sw = File.CreateText(path);
                sw.WriteLine(logText);
            }
            else
            {
                using StreamWriter sw = File.AppendText(path);
                sw.WriteLine(logText);
            }
           
        }

        private void LogToEmail(string logText)
        {
            throw new NotImplementedException();
        }
        
        private void LogToDatabase(string logText)
        {
            throw new NotImplementedException();
        }

    }
}
