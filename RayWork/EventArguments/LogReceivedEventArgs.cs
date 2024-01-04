namespace RayWork.EventArguments;

public class LogReceivedEventArgs(Logger.Level logMessageLevel, string timeOfMessage, string logMessage)
    : EventArgs
{
    public readonly Logger.Level LogMessageLevel = logMessageLevel;
    public readonly string TimeOfMessage = timeOfMessage;
    public readonly string LogMessage = logMessage;
}