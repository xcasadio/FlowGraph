using System.Xml;
using FlowGraphBase;
using FlowGraphBase.Logger;
using FlowGraphBase.Node;
using FlowGraphBase.Process;

namespace FlowSimulator.CustomNode
{
    /// <summary>
    /// 
    /// </summary>
    [Category("Action"), Name("Log")]
    public class LogMessageNode : ActionNode
    {
        #region Enum

        public enum NodeSlotId
        {
            In,
            Out,
            Message
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public override string Title => "Log Message";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public LogMessageNode(XmlNode node_)
            : base(node_)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public LogMessageNode()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitializeSlots()
        {
            base.InitializeSlots();

            AddSlot((int)NodeSlotId.In, "", SlotType.NodeIn);
            AddSlot((int)NodeSlotId.Out, "", SlotType.NodeOut);

            AddSlot((int)NodeSlotId.Message, "Message", SlotType.VarIn, typeof(string));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ProcessingInfo ActivateLogic(ProcessingContext context_, NodeSlot slot_)
        {
            ProcessingInfo info = new ProcessingInfo
            {
                State = LogicState.Ok
            };
            object val = GetValueFromSlot((int)NodeSlotId.Message);

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
                LogManager.Instance.WriteLine(LogVerbosity.Info, val.ToString());
            }

            ActivateOutputLink(context_, (int)NodeSlotId.Out);

            return info;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override SequenceNode CopyImpl()
        {
            return new LogMessageNode();
        }
    } // LogMessageNode
}
