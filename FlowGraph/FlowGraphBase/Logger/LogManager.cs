using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlowGraphBase.Logger
{
	/// <summary>
	/// 
	/// </summary>
    public sealed class LogManager
	{
        #region Singleton

        static private LogManager m_Instance = new LogManager();

        /// <summary>
        /// Gets
        /// </summary>
        static public LogManager Instance
        {
            get
            {
                return m_Instance;
            }
        }

        #endregion //Singleton

		#region Fields

        internal event EventHandler NbErrorChanged;
        private int m_NbErrors = 0;

		private List<ILog> m_Loggers = new List<ILog>();

#if DEBUG
        private LogVerbosity m_Verbosity = LogVerbosity.Trace;
#else
        private LogVerbosity m_Verbosity = LogVerbosity.Info;
#endif

		#endregion

		#region Properties

        /// <summary>
        /// Gets/Sets
        /// </summary>
        public LogVerbosity Verbosity
        {
            get { return m_Verbosity; }
            set { m_Verbosity = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int NbErrors
        {
            get { return m_NbErrors; }
            set
            {
                if (m_NbErrors != value)
                {
                    m_NbErrors = value;

                    if (NbErrorChanged != null)
                    {
                        NbErrorChanged(this, EventArgs.Empty);
                    }
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
		/// <param name="log_"></param>
		public void AddLogger(ILog log_)
		{
			if (log_ == null)
			{
				throw new ArgumentNullException("LogManager.Instance.AddLogger() : ILog is null");
			}

			m_Loggers.Add(log_);
		}

        /// <summary>
		/// 
		/// </summary>
		/// <param name="log_"></param>
		public void Close()
		{
			foreach (ILog log in m_Loggers)
			{
				log.Close();
			}

            m_Loggers.Clear();
		}

        /// <summary>
        /// Write a line in every ILog registred
        /// </summary>
        /// <param name="verbose_"></param>
        /// <param name="args_"></param>
        public void WriteLine(LogVerbosity verbose_, string msg_, params object[] args_)
        {
            if (verbose_ == LogVerbosity.Error)
            {
                NbErrors++;
            }

            if (verbose_ < m_Verbosity)
            {
                return;
            }

            string tmp = string.Format(msg_, args_);

            foreach (ILog log in m_Loggers)
            {
                log.Write(verbose_, tmp);
            }
        }

        /// <summary>
        /// Format the message of the exception and log it
        /// </summary>
        /// <param name="e"></param>
        public void WriteException(Exception e, bool writeStackTrace = true)
        {
            NbErrors++;

            if (m_Verbosity == LogVerbosity.None)
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

            strBldr.Replace("{", "{{");
            strBldr.Replace("}", "}}");

            WriteLine(LogVerbosity.Error, strBldr.ToString());
        }

		#endregion
	}
}
