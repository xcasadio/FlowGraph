using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlowSimulator;
using System.Globalization;
using System.ComponentModel;
using System.Windows;
using System.Collections.ObjectModel;
using FlowGraphBase.Logger;


namespace FlowSimulator.Logger
{
    #region Log Entry

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

    #endregion

    /// <summary>
    /// 
    /// </summary>
    public class LogEditor
        : ILog
    {
        #region Fields

        #endregion // Fields

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        static public ObservableCollection<LogEntry> LogEntries { get; private set; }

        #endregion // Properties

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textBox_"></param>
        public LogEditor()
        {
            if (LogEntries == null)
            {
                LogEntries = new ObservableCollection<LogEntry>();
            }
        }

        #endregion // Constructors

        #region Methods

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
        /// <param name="args_"></param>
        public void Write(LogVerbosity verbose, string msg_)
        {
            if (Application.Current.Dispatcher.CheckAccess() == true)
            {
                LogEntries.Add(new LogEntry()
                {
                    Severity = "[" + Enum.GetName(typeof(LogVerbosity), verbose) + "]",
                    DateTime = DateTime.Now,
                    Message = msg_,
                });
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(new Action( () => Write(verbose, msg_)));
            }
        }

        #endregion // Methods
    }
}
