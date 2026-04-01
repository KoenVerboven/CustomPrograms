using Microsoft.Data.SqlClient;
using System.Data;
using System.Net.Mail;

namespace ConsoleLoggingApp.KoenLogger
{
    internal class KoenLog : IKoenLog
    {
        const string emailSubject = "KoenLog Message";
        const string fileNamePrefix = "KoensLog_";
        const string fileExtension = ".log";
        const string defaultPathToLogFile = @"C:\KoensCustomLoggingFolder\";
        const int minLengthFileNamePrefix = 3;

        public KoenLog()
        {
        }

        public required string ProgramNameWhoIsSendingToLog { get; set; }
        public OutputTarget OutputTarget { get; set; }
        public string PathToLogFile { get; set; }
        public string Emailserver { get; set; }
        public string EmailFrom { get; set; }
        public string EmailTo { get; set; }
        public string DatabaseConnnectionString { get; set; }



        public void Log(string logText, LogLevel logLevel)
        {
            var outputText = logLevel switch
            {
                LogLevel.Trace =>       "[Trace]       " + logText,
                LogLevel.Debug =>       "[Debug]       " + logText,
                LogLevel.Information => "[Information] " + logText,
                LogLevel.Warning =>     "[Warning]     " + logText,
                LogLevel.Error =>       "[Error]       " + logText,
                LogLevel.Critical =>    "[Critical]    " + logText,
                _ => logText,
            };

            logText = DateTime.Now.ToString("HH:mm:ss") + "  " + ProgramNameWhoIsSendingToLog + " " + outputText.Trim();

            switch (OutputTarget)
            {
                case OutputTarget.Screen:
                    LogToScreen(logText);
                    break;
                case OutputTarget.TextFile:
                    LogToTextFile(logText);
                    break;
                case OutputTarget.ExcelFile:
                    LogToExcelFile(logText);
                    break;
                case OutputTarget.Email:
                    LogToEmail(logText);
                    break;
                case OutputTarget.Database:
                    LogToDatabase(logText, logLevel);
                    break;
                default:
                    LogToTextFile(logText);
                    break;
            }

        }

        private static void LogToScreen(string logText)
        {
            Console.WriteLine($"[koenLog] {logText}");
        }

        private void LogToTextFile(string logText)
        {
            var logFileName = fileNamePrefix + DateTime.Now.ToString("yyyyMMdd") + fileExtension;

            if (string.IsNullOrEmpty(PathToLogFile)) PathToLogFile = defaultPathToLogFile;

            var path = Path.Combine(PathToLogFile, logFileName);

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
            if (fileNamePrefix.Length >= minLengthFileNamePrefix)
            {
                string[] logFiles = Directory.GetFiles(PathToLogFile, fileNamePrefix + "*" + fileExtension);
                foreach (string logFile in logFiles)
                {
                    DateTime creationTime = File.GetCreationTime(logFile);

                    if ((DateTime.Now - creationTime).TotalDays > daysToKeep)
                    {
                        File.Delete(logFile);
                    }

                }
            }
            else
            {
                Console.WriteLine($"Please set a FileNamePrefix with more than {minLengthFileNamePrefix} characters to avoid deleting unintended files.");
            }
        }

        private void LogToEmail(string logText)//todo : untested, should be tested with a email server
        {
            MailMessage message = new(EmailFrom, EmailTo)//todo : send a day overview of the log messages instead of sending an email for each log message
            {
                Subject = emailSubject,
                Body = logText
            };

            SmtpClient client = new(Emailserver)
            {
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

        private void LogToDatabase(string logText, LogLevel logLevel)
        {
            string query = "INSERT INTO Log ( CreateDate, ProgramNameWhoIsSendingToLog, LogLevel, LogText) " +
                           "VALUES (@CreateDate, ProgramNameWhoIsSendingToLog,@LogLevel, @LogText ) ";

            using (SqlConnection cn = new SqlConnection(DatabaseConnnectionString))
            using (SqlCommand cmd = new SqlCommand(query, cn))
            {
                cmd.Parameters.Add("@CreateDate", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@ProgramNameWhoIsSendingToLog", SqlDbType.VarChar, 50).Value = ProgramNameWhoIsSendingToLog;
                cmd.Parameters.Add("@LogLevel", SqlDbType.VarChar, 50).Value = logLevel.ToString();
                cmd.Parameters.Add("@LogText", SqlDbType.VarChar, 50).Value = logText;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }

        private void LogToExcelFile(string logText)
        {
            throw new NotImplementedException();
        }
    }
}
