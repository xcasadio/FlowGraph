namespace Logger;

public interface ILogManager
{
    void WriteLine(LogVerbosity verbose, string msg, params object?[] args);
    void WriteException(Exception e, bool writeStackTrace = true);
}