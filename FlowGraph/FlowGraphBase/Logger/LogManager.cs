using System;
using System.Collections.Generic;
using System.Text;

namespace FlowGraphBase.Logger
{
    public sealed class LogManager
    {
        public static LogManager Instance { get; } = new LogManager();

        internal event EventHandler NbErrorChanged;
        private int _nbErrors;

        private readonly List<ILog> _loggers = new List<ILog>();

#if DEBUG
#else
        private LogVerbosity _verbosity = LogVerbosity.Info;
#endif

        public LogVerbosity Verbosity { get; set; } = LogVerbosity.Trace;

        public int NbErrors
        {
            get => _nbErrors;
            set
            {
                if (_nbErrors != value)
                {
                    _nbErrors = value;

                    NbErrorChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public void AddLogger(ILog log)
        {
            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }

            _loggers.Add(log);
        }

        public void Close()
        {
            foreach (ILog log in _loggers)
            {
                log.Close();
            }

            _loggers.Clear();
        }

        public void WriteLine(LogVerbosity verbose, string msg, params object[] args)
        {
            if (verbose == LogVerbosity.Error)
            {
                NbErrors++;
            }

            if (verbose < Verbosity)
            {
                return;
            }

            string tmp = string.Format(msg, args);

            foreach (ILog log in _loggers)
            {
                log.Write(verbose, tmp);
            }
        }

        public void WriteException(Exception e, bool writeStackTrace = true)
        {
            NbErrors++;

            if (Verbosity == LogVerbosity.None)
            {
                return;
            }

            StringBuilder errorMessage = new StringBuilder();
            Exception ex = e;

            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            errorMessage.AppendLine(ex.Message);

            if (writeStackTrace)
            {
                errorMessage.AppendLine(e.StackTrace);
            }

            errorMessage.Replace("{", "{{").Replace("}", "}}");

            WriteLine(LogVerbosity.Error, errorMessage.ToString());
        }
    }
}
