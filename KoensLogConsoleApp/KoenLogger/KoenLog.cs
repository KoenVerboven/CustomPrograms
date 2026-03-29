using System.Net.Mail;

namespace ConsoleLoggingApp.KoenLogger
{
    internal class KoenLog : IKoenLog
    {
        const string _emailSubject = "KoenLog Message";
        const string _fileNamePrefix = "KoensLog_";
        const string _fileExtension = ".log";
        private string _databaseConnectionString = "";

        public KoenLog()
        {
        }

        public OutputTarget OutputTarget { get; set; }
        public string? PathToLogFile { get; set; } // todo : verlplicht maken ????
        public string? Emailserver { get; set; }
        public string? EmailFrom { get; set; }
        public string? EmailTo { get; set; }

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
            var path = Path.Combine(PathToLogFile, logFileName); // todo : check if path is not null

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
            const int minLengthFileNamePrefix = 3;
            if ( _fileNamePrefix.Length >= minLengthFileNamePrefix)
            {
                string[] logFiles = Directory.GetFiles(PathToLogFile, _fileNamePrefix + "*" + _fileExtension);
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
            MailMessage message = new (EmailFrom, EmailTo)//todo : send a day overview of the log messages instead of sending an email for each log message
            {
                Subject = _emailSubject,
                Body = logText
            };

            SmtpClient client = new (Emailserver)
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
                Console.WriteLine("Exception caught in LogToEmail: {0}", ex.ToString());
            }
        }
        
        private void LogToDatabase(string logText)//todo : custome may choose a specitic database (sql, mysql, oracle, etc) and should provide a connection string
        {
            throw new NotImplementedException();
        }

    }
}
