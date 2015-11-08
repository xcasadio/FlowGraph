using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace FlowSimulator.Logger
{
    /// <summary>
    /// 
    /// </summary>
    public class LogFile
    {
        /// <summary>
        /// 
        /// </summary>
        public enum LogVerbosity
        {
            Trace = 5,
            Debug = 4,
            Info = 3,
            Warning = 2,
            Error = 1,
            None = 0,
        }

        #region Singleton

        private static LogFile ms_Singleton;

        #endregion //Singleton

        #region Fields

        private bool m_Async;
        private StreamWriter m_Writer;
        private StringBuilder m_StringBuilder = new StringBuilder(500);
        private Task m_Task;
        private volatile bool m_IsAlive;
        private volatile bool m_StreamClose = false;

        #endregion //Fields

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        static public LogVerbosity Verbosity
        {
            get;
            set;
        }

        #endregion //Properties

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        private LogFile(string file_, bool async_)
        {
            m_Async = async_;
            Verbosity = LogVerbosity.Info;
            m_IsAlive = true;
            m_Writer = File.CreateText(file_);

            if (m_Async == true)
            {
                m_Task = new Task(new Action(() =>
                {
                    while (m_IsAlive)
                    {
                        Monitor.Enter(m_StringBuilder);
                        try
                        {
                            if (m_StringBuilder.Length == 0) Monitor.Wait(m_StringBuilder);
                            else WriteInFile();
                        }
                        finally
                        {
                            Monitor.Exit(m_StringBuilder);
                        }
                    }
                }));
                m_Task.Start();
            }

            m_StringBuilder.AppendLine("-------------------------------------------------------------------------------------------");
            m_StringBuilder.AppendFormat("Events Log -  Date : {0}\n", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            m_StringBuilder.AppendFormat("Hostmane : {0}  -  UserName : {1}\n", System.Environment.MachineName, System.Environment.UserName);
            m_StringBuilder.AppendFormat("OS Version : {0}  -  CLR Version : {1}\n", System.Environment.OSVersion, System.Environment.Version);
            m_StringBuilder.AppendLine("-------------------------------------------------------------------------------------------");
            WriteInFile();
        }

        #endregion //Constructors

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        static public void Initialize(string file_, bool async_)
        {
            if (ms_Singleton != null)
            {
                return;
            }

            ms_Singleton = new LogFile(file_, async_);
        }

        /// <summary>
        /// 
        /// </summary>
        static public void Destroy()
        {
            if (ms_Singleton == null)
            {
                return;
            }

            ms_Singleton.m_StreamClose = true;
            ms_Singleton.m_IsAlive = false;
            if (ms_Singleton.m_Task != null) ms_Singleton.m_Task.Wait();
            ms_Singleton.m_Writer.Close();
            ms_Singleton = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pre_"></param>
        /// <param name="log_"></param>
        /// <param name="args_"></param>
        void Write(string pre_, string log_, params object[] args_)
        {
            if (m_StreamClose == true)
            {
                return;
            }

            if (m_Async)
            {
                Monitor.Enter(m_StringBuilder);
                try
                {
                    m_StringBuilder.AppendFormat("{0}({1}){2}> {3}\n",
                        pre_,
                        Thread.CurrentThread.GetHashCode(),
                        DateTime.Now.ToString("HH:mm:ss.fff"),
                        log_);
                    Monitor.PulseAll(m_StringBuilder);
                }
                finally
                {
                    Monitor.Exit(m_StringBuilder);
                }
            }
            else
            {
                lock (m_StringBuilder)
                {
                    m_StringBuilder.AppendFormat("{0}({1}){2}> {3}\n",
                        pre_,
                        Thread.CurrentThread.GetHashCode(),
                        DateTime.Now.ToString("HH:mm:ss.fff"),
                        log_);
                    WriteInFile();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private void WriteInFile()
        {
            if (m_StreamClose == true)
            {
                return;
            }

            m_Writer.Write(m_StringBuilder.ToString());
            m_StringBuilder.Clear();
            m_Writer.Flush();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str_"></param>
        /// <param name="args_"></param>
        public static void OnTrace(string str_, params object[] args_)
        {
            if (ms_Singleton == null
                || Verbosity < LogVerbosity.Trace)
            {
                return;
            }

            ms_Singleton.Write("[TRACE]: ", str_, args_);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str_"></param>
        /// <param name="args_"></param>
        public static void OnDebug(string str_, params object[] args_)
        {
            if (ms_Singleton == null
                || Verbosity < LogVerbosity.Debug)
            {
                return;
            }

            ms_Singleton.Write("[DEBUG]: ", str_, args_);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str_"></param>
        /// <param name="args_"></param>
        public static void OnInfo(string str_, params object[] args_)
        {
            if (ms_Singleton == null
                || Verbosity < LogVerbosity.Info)
            {
                return;
            }

            ms_Singleton.Write("[INFO]: ", str_, args_);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str_"></param>
        /// <param name="args_"></param>
        public static void OnWarning(string str_, params object[] args_)
        {
            if (ms_Singleton == null
                || Verbosity < LogVerbosity.Warning)
            {
                return;
            }

            ms_Singleton.Write("[WARNING]: ", str_, args_);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str_"></param>
        /// <param name="args_"></param>
        public static void OnError(string str_, params object[] args_)
        {
            if (ms_Singleton == null
                || Verbosity < LogVerbosity.Error)
            {
                return;
            }

            ms_Singleton.Write("[ERROR]: ", str_, args_);
        }

        #endregion //Methods
    }
}
