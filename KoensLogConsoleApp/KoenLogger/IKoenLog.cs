
namespace ConsoleLoggingApp.KoenLogger
{
    internal interface IKoenLog
    {
        OutputTarget OutputTarget { get; set; }
        void Log(string logText, LogLevel LogLevel);
        void DeleteOldLogFiles(int daysToKeep);
    }
}
