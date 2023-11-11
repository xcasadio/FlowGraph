using System.Xml;
using FlowGraph;
using FlowGraph.Attributes;
using FlowGraph.Logger;
using FlowGraph.Nodes;
using FlowGraph.Process;
using Logger;

namespace CustomNode
{
    [Category("Action"), Name("Log")]
    public class LogMessageNode : ActionNode
    {
        private enum NodeSlotId
        {
            In,
            Out,
            Message
        }

        public override string Title => "Log Message";

        public LogMessageNode(XmlNode node)
            : base(node)
        {

        }

        public LogMessageNode()
        {

        }

        protected override void InitializeSlots()
        {
            base.InitializeSlots();

            AddSlot((int)NodeSlotId.In, "", SlotType.NodeIn);
            AddSlot((int)NodeSlotId.Out, "", SlotType.NodeOut);

            AddSlot((int)NodeSlotId.Message, "Message", SlotType.VarIn, typeof(string));
        }

        public override ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot)
        {
            var info = new ProcessingInfo
            {
                State = LogicState.Ok
            };

            var val = GetValueFromSlot((int)NodeSlotId.Message);

            if (val == null)
            {
                info.State = LogicState.Warning;
                info.ErrorMessage = "Please connect a string variable node";

                LogManager.Instance.WriteLine(LogVerbosity.Warning,
                    "{0} : No message display because no variable node connected. {1}.",
                    Title, info.ErrorMessage);
            }
            else
            {
                LogManager.Instance.WriteLine(LogVerbosity.Info, val.ToString()!);
            }

            ActivateOutputLink(context, (int)NodeSlotId.Out);

            return info;
        }

        protected override SequenceNode CopyImpl()
        {
            return new LogMessageNode();
        }
    }
}
