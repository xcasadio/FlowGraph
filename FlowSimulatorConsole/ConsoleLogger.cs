using System.Text;
using FlowGraph.Logger;

namespace FlowSimulatorConsole;

class ConsoleLogger : ILog
{
    public void Close() { }

    public void Write(LogVerbosity verbose, string msg)
    {
        ConsoleColor color = Console.ForegroundColor;
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("{0:HH:mm:ss.fff} [{1}] {2}", DateTime.Now, Enum.GetName(typeof(LogVerbosity), verbose), msg);

        switch (verbose)
        {
            case LogVerbosity.Trace: Console.ForegroundColor = ConsoleColor.DarkGray; break;
            case LogVerbosity.Debug: Console.ForegroundColor = ConsoleColor.Green; break;
            case LogVerbosity.Info: Console.ForegroundColor = ConsoleColor.White; break;
            case LogVerbosity.Warning: Console.ForegroundColor = ConsoleColor.DarkYellow; break;
            case LogVerbosity.Error: Console.ForegroundColor = ConsoleColor.Red; break;
        }

        Console.WriteLine(sb.ToString());
        Console.ForegroundColor = color;
    }
}