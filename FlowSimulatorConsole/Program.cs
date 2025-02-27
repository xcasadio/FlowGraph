using CustomNode;
using FlowGraph;
using FlowGraph.Logger;
using FlowGraph.Process;
using Logger;
using Newtonsoft.Json.Linq;

namespace FlowSimulatorConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ProcessLauncher.Instance.StartLoop();

                LogManager.Instance.AddLogger(new ConsoleLogger());
                LogManager.Instance.Verbosity = LogVerbosity.Trace;

                JObject jsonDocument = JObject.Parse(File.ReadAllText("../../../../Test.xml"));

                // TODO : how load NamedVariableList and Sequence
                // if the structure of the xml is not defined in the library ?
                // It is the user who build the xml
                NamedVariableManager.Instance.Load(jsonDocument);
                var flowGraphManager = new FlowGraphManager();
                flowGraphManager.Load(jsonDocument);

                Sequence seq = new Sequence("test");
                throw new NotImplementedException();
                //seq.Load(jsonDocument.SelectSingleNode("FlowSimulator/GraphList/Graph[@name='test']"));

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
}
