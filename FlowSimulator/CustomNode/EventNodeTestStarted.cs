using System.Xml;
using FlowGraphBase;
using FlowGraphBase.Node;

namespace FlowSimulator.CustomNode
{
    /// <summary>
    /// 
    /// </summary>
    [Category("Event"), Name("Test Started")]
    public class EventNodeTestStarted : EventNode
    {
        /// <summary>
        /// 
        /// </summary>
        public override string Title => "Test Started Event";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public EventNodeTestStarted(XmlNode node)
            : base(node)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public EventNodeTestStarted()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitializeSlots()
        {
            base.InitializeSlots();

            AddSlot(0, "Started", SlotType.NodeOut);
            AddSlot(1, "Task name", SlotType.VarOut, typeof(string));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="para_"></param>
        protected override void TriggeredImpl(object para)
        {
            SetValueInSlot(1, para);
        }

        /*protected override void Load(XmlNode node_)
        {
            base.Load();
        }*/

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override SequenceNode CopyImpl()
        {
            return new EventNodeTestStarted();
        }
    }
}
