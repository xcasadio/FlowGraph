using System;
using System.Collections.Generic;
using System.Text;

namespace FlowGraphBase.Logger
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class LogManager
	{
        /// <summary>
        /// Gets
        /// </summary>
        public static LogManager Instance { get; } = new LogManager();

        internal event EventHandler NbErrorChanged;
        private int _nbErrors;

		private readonly List<ILog> _loggers = new List<ILog>();

#if DEBUG
#else
        private LogVerbosity _verbosity = LogVerbosity.Info;
#endif

        /// <summary>
        /// Gets/Sets
        /// </summary>
        public LogVerbosity Verbosity { get; set; } = LogVerbosity.Trace;

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
		/// 
		/// </summary>
		/// <param name="log"></param>
		public void AddLogger(ILog log)
		{
			if (log == null)
			{
				throw new ArgumentNullException("LogManager.Instance.AddLogger() : ILog is null");
			}

			_loggers.Add(log);
		}

        /// <summary>
        /// 
        /// </summary>
        public void Close()
		{
			foreach (ILog log in _loggers)
			{
				log.Close();
			}

            _loggers.Clear();
		}

        /// <summary>
        /// Write a line in every ILog registred
        /// </summary>
        /// <param name="verbose"></param>
        /// <param name="msg"></param>
        /// <param name="args"></param>
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

        /// <summary>
        /// Format the message of the exception and log it
        /// </summary>
        /// <param name="e"></param>
        /// <param name="writeStackTrace"></param>
        public void WriteException(Exception e, bool writeStackTrace = true)
        {
            NbErrors++;

            if (Verbosity == LogVerbosity.None)
            {
                return;
            }

            StringBuilder strBldr = new StringBuilder();
            Exception ex = e;

            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            strBldr.AppendLine(ex.Message);

            if (writeStackTrace)
            {
                strBldr.AppendLine(e.StackTrace);
            }

            strBldr.Replace("{", "{{").Replace("}", "}}");

            WriteLine(LogVerbosity.Error, strBldr.ToString());
        }
    }
}
