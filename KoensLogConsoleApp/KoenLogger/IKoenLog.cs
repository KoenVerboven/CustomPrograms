
namespace KoensLogConsoleApp.KLogger
{
    internal interface IKoenLog
    {
        OutputTarget OutputTarget { get; set; }
        void Log(string logText, OutputType outputType);
    }
}
