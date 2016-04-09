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
        #region Singleton

        private static readonly LogManager _Instance = new LogManager();

        /// <summary>
        /// Gets
        /// </summary>
        public static LogManager Instance
        {
            get { return _Instance; }
        }

        #endregion //Singleton

		#region Fields

        internal event EventHandler NbErrorChanged;
        private int _nbErrors = 0;

		private readonly List<ILog> _loggers = new List<ILog>();

#if DEBUG
        private LogVerbosity _verbosity = LogVerbosity.Trace;
#else
        private LogVerbosity _verbosity = LogVerbosity.Info;
#endif

		#endregion

		#region Properties

        /// <summary>
        /// Gets/Sets
        /// </summary>
        public LogVerbosity Verbosity
        {
            get { return _verbosity; }
            set { _verbosity = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int NbErrors
        {
            get { return _nbErrors; }
            set
            {
                if (_nbErrors != value)
                {
                    _nbErrors = value;

                    if (NbErrorChanged != null) NbErrorChanged.Invoke(this, EventArgs.Empty);
                }
            }
        }

		#endregion

		#region Constructors

		#endregion

		#region Methods

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

            if (verbose < _verbosity)
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

            if (_verbosity == LogVerbosity.None)
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

            if (writeStackTrace == true)
            {
                strBldr.AppendLine(e.StackTrace);
            }

            strBldr.Replace("{", "{{").Replace("}", "}}");

            WriteLine(LogVerbosity.Error, strBldr.ToString());
        }

		#endregion
	}
}
