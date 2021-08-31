using System;
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

        private static LogFile _singleton;

        #endregion //Singleton

        #region Fields

        private readonly bool _async;
        private readonly StreamWriter _writer;
        private readonly StringBuilder _stringBuilder = new StringBuilder(500);
        private readonly Task _task;
        private volatile bool _isAlive;
        private volatile bool _streamClose = false;

        #endregion //Fields

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        private static LogVerbosity Verbosity
        {
            get;
            set;
        }

        #endregion //Properties

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        private LogFile(string file, bool async)
        {
            _async = async;
            Verbosity = LogVerbosity.Info;
            _isAlive = true;
            _writer = File.CreateText(file);

            if (_async)
            {
                _task = new Task(() =>
                {
                    while (_isAlive)
                    {
                        Monitor.Enter(_stringBuilder);
                        try
                        {
                            if (_stringBuilder.Length == 0) Monitor.Wait(_stringBuilder);
                            else WriteInFile();
                        }
                        finally
                        {
                            Monitor.Exit(_stringBuilder);
                        }
                    }
                });
                _task.Start();
            }

            _stringBuilder.AppendLine("-------------------------------------------------------------------------------------------");
            _stringBuilder.AppendFormat("Events Log -  Date : {0}\n", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            _stringBuilder.AppendFormat("Hostmane : {0}  -  UserName : {1}\n", Environment.MachineName, Environment.UserName);
            _stringBuilder.AppendFormat("OS Version : {0}  -  CLR Version : {1}\n", Environment.OSVersion, Environment.Version);
            _stringBuilder.AppendLine("-------------------------------------------------------------------------------------------");
            WriteInFile();
        }

        #endregion //Constructors

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public static void Initialize(string file, bool async)
        {
            if (_singleton != null)
            {
                return;
            }

            _singleton = new LogFile(file, async);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Destroy()
        {
            if (_singleton == null)
            {
                return;
            }

            _singleton._streamClose = true;
            _singleton._isAlive = false;
            if (_singleton._task != null) _singleton._task.Wait();
            _singleton._writer.Close();
            _singleton = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pre"></param>
        /// <param name="log"></param>
        /// <param name="args"></param>
        void Write(string pre, string log, params object[] args)
        {
            if (_streamClose)
            {
                return;
            }

            if (_async)
            {
                Monitor.Enter(_stringBuilder);
                try
                {
                    _stringBuilder.AppendFormat("{0}({1}){2}> {3}\n",
                        pre,
                        Thread.CurrentThread.GetHashCode(),
                        DateTime.Now.ToString("HH:mm:ss.fff"),
                        log);
                    Monitor.PulseAll(_stringBuilder);
                }
                finally
                {
                    Monitor.Exit(_stringBuilder);
                }
            }
            else
            {
                lock (_stringBuilder)
                {
                    _stringBuilder.AppendFormat("{0}({1}){2}> {3}\n",
                        pre,
                        Thread.CurrentThread.GetHashCode(),
                        DateTime.Now.ToString("HH:mm:ss.fff"),
                        log);
                    WriteInFile();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void WriteInFile()
        {
            if (_streamClose)
            {
                return;
            }

            _writer.Write(_stringBuilder.ToString());
            _stringBuilder.Clear();
            _writer.Flush();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="args"></param>
        public static void OnTrace(string str, params object[] args)
        {
            if (_singleton == null
                || Verbosity < LogVerbosity.Trace)
            {
                return;
            }

            _singleton.Write("[TRACE]: ", str, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="args"></param>
        public static void OnDebug(string str, params object[] args)
        {
            if (_singleton == null
                || Verbosity < LogVerbosity.Debug)
            {
                return;
            }

            _singleton.Write("[DEBUG]: ", str, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="args"></param>
        public static void OnInfo(string str, params object[] args)
        {
            if (_singleton == null
                || Verbosity < LogVerbosity.Info)
            {
                return;
            }

            _singleton.Write("[INFO]: ", str, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="args"></param>
        public static void OnWarning(string str, params object[] args)
        {
            if (_singleton == null
                || Verbosity < LogVerbosity.Warning)
            {
                return;
            }

            _singleton.Write("[WARNING]: ", str, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="args"></param>
        public static void OnError(string str, params object[] args)
        {
            if (_singleton == null
                || Verbosity < LogVerbosity.Error)
            {
                return;
            }

            _singleton.Write("[ERROR]: ", str, args);
        }

        #endregion //Methods
    }
}
