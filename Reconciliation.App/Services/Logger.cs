namespace Reconciliation.App.Services;

public enum LogLevel { Info, Warn, Error }

public class Logger
{
    private readonly string _filePath;
    private readonly string _runId;

    public Logger(string filePath)
    {
        _filePath = filePath;
        _runId = Guid.NewGuid().ToString().Substring(0, 8);
        File.WriteAllText(_filePath, $"--- Log démarré RunID={_runId} ---{Environment.NewLine}");
    }

    private void Write(LogLevel level, string message)
    {
        string logLine = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} | {_runId} | {level} | {message}";
        File.AppendAllText(_filePath, logLine + Environment.NewLine);
    }

    public void Info(string message) => Write(LogLevel.Info, message);
    public void Warn(string message) => Write(LogLevel.Warn, message);
    public void Error(string message) => Write(LogLevel.Error, message);
    public string RunId => _runId;
}