
namespace ConsoleLoggingApp.KoenLogger
{
    internal interface IKoenLog
    {
        OutputTarget OutputTarget { get; set; }
        void Log(string logText, OutputType outputType);
        void DeleteOldLogFiles(int daysToKeep);
    }
}
