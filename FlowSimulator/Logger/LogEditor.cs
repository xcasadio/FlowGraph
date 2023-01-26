using System.Collections.ObjectModel;
using System.Windows;
using FlowGraph.Logger;

namespace FlowSimulator.Logger
{
    /// <summary>
    /// 
    /// </summary>
    public class LogEntry : PropertyChangedBase
    {
        public DateTime DateTime { get; set; }
        public string Severity { get; set; }
        public string Message { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CollapsibleLogEntry : LogEntry
    {
        public List<LogEntry> Contents { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class LogEditor
        : ILog
    {
        /// <summary>
        /// 
        /// </summary>
        public static ObservableCollection<LogEntry> LogEntries { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public LogEditor()
        {
            if (LogEntries == null)
            {
                LogEntries = new ObservableCollection<LogEntry>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Close()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="verbose"></param>
        /// <param name="msg"></param>
        public void Write(LogVerbosity verbose, string msg)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                LogEntries.Add(new LogEntry
                {
                    Severity = "[" + Enum.GetName(typeof(LogVerbosity), verbose) + "]",
                    DateTime = DateTime.Now,
                    Message = msg,
                });
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => Write(verbose, msg)));
            }
        }
    }
}
