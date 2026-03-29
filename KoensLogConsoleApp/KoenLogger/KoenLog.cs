using ConsoleLoggingApp.KoenLogger;
using System.Net.Mail;

namespace KoensLogConsoleApp.Models
{
    internal class KoenLog : IKoenLog
    {
        private string _emailServer = "smtp.contoso.com";
        private string _emailFrom = "testFrom@test.com";
        private string _emailTo = "testTo@test.com";
        private string _emailAddress = "";
        private string _emailSubject = "KoenLog Message";
        private string _connectionString = "";
        private string _pathToLogFile = @"C:\Users\koenv\source\repos\KoensLogConsoleApp\KoensLogConsoleApp\logFiles\";//KoensLogConsoleApp aanpassen
        const string _fileNamePrefix = "KoensLog_";
        const string _fileExtension = ".log";

        public KoenLog()
        {
        }

        public OutputTarget OutputTarget { get; set; }

        public void Log(string logText, OutputType outputType)
        {
            var outputText = outputType switch
            {
                OutputType.Info    => "[Info]    " + logText,
                OutputType.Warning => "[Warning] " + logText,
                OutputType.Error   => "[Error]   " + logText,
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
            var logFileName = _fileNamePrefix + DateTime.Now.ToString("yyyyMMdd") + _fileExtension;
            var path = Path.Combine(_pathToLogFile, logFileName);

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

        public void DeleteOldLogFiles(int daysToKeep)
        {
            var minLengthFileNamePrefix = 4;
            if ( _fileNamePrefix.Length > minLengthFileNamePrefix)
            {
                string[] logFiles = Directory.GetFiles(_pathToLogFile, _fileNamePrefix + "*" + _fileExtension);
                foreach (string logFile in logFiles)
                {
                    DateTime creationTime = File.GetCreationTime(logFile);
                    if ((DateTime.Now - creationTime).TotalDays > daysToKeep)
                    {
                        File.Delete(logFile);
                    }
                }
            }
            else {  
                Console.WriteLine($"Please set a FileNamePrefix with more than {minLengthFileNamePrefix} characters to avoid deleting unintended files.");
            }
        }

        private void LogToEmail(string logText)//todo : untested, should be tested with a email server
        {
            MailMessage message = new (_emailFrom, _emailTo)
            {
                Subject = _emailSubject,
                Body = logText
            };

            SmtpClient client = new (_emailServer)
            {
                // Credentials are necessary if the server requires the client
                // to authenticate before it will send email on the client's behalf.
                UseDefaultCredentials = true
            };

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in LogToEmail: {0}",
                    ex.ToString());
            }
        }
        
        private void LogToDatabase(string logText)
        {
            throw new NotImplementedException();
        }

    }
}
