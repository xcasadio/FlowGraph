using System;
using System.Text;
using System.Xml;
using FlowGraphBase;
using FlowGraphBase.Logger;
using FlowGraphBase.Process;
using FlowSimulator.CustomNode;

namespace FlowGraphConsole
{
    /// <summary>
    /// 
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ProcessLauncher.Instance.StartLoop();

                LogManager.Instance.AddLogger(new ConsoleLogger());
                LogManager.Instance.Verbosity = LogVerbosity.Trace;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("../../../../Test.xml");

                // TODO : how load NamedVariableList and Sequence
                // if the structure of the xml is not defined in the library ?
                // It is the user who build the xml
                XmlNode rootNode = xmlDoc.FirstChild.NextSibling;
                NamedVariableManager.Instance.Load(rootNode);
                GraphDataManager.Instance.Load(rootNode);

                Sequence seq = new Sequence("test");
                seq.Load(xmlDoc.SelectSingleNode("FlowSimulator/GraphList/Graph[@name='test']"));

                ProcessLauncher.Instance.LaunchSequence(seq, typeof(EventNodeTestStarted), 0, "test");
                ProcessLauncher.Instance.StopLoop();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            Console.WriteLine("Press any keys...");
            Console.ReadKey();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    class ConsoleLogger : ILog
    {
        /// <summary>
        /// 
        /// </summary>
        public void Close() {}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="verbose_"></param>
        /// <param name="msg_"></param>
        public void Write(LogVerbosity verbose, string msg)
        {
            ConsoleColor color = Console.ForegroundColor;
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0} [{1}] {2}", 
                DateTime.Now.ToString("HH:mm:ss.fff"),
                Enum.GetName(typeof(LogVerbosity), verbose),
                msg);

            switch (verbose)
            {
                case LogVerbosity.Trace:    Console.ForegroundColor = ConsoleColor.DarkGray; break;
                case LogVerbosity.Debug:    Console.ForegroundColor = ConsoleColor.Green; break;
                case LogVerbosity.Info:     Console.ForegroundColor = ConsoleColor.White; break;
                case LogVerbosity.Warning:  Console.ForegroundColor = ConsoleColor.DarkYellow; break;
                case LogVerbosity.Error:    Console.ForegroundColor = ConsoleColor.Red; break;
            }
            
            Console.WriteLine(sb.ToString());
            Console.ForegroundColor = color;
        }
    }
}
