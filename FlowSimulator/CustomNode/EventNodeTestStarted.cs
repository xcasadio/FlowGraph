using FlowGraphBase.Node;
using System.Xml;
using FlowGraphBase;

namespace FlowSimulator.CustomNode
{
    /// <summary>
    /// 
    /// </summary>
    [Category("Event"), Name("Test Started")]
    public class EventNodeTestStarted : EventNode
    {
		#region Fields
		
		#endregion //Fields
	
		#region Properties

        /// <summary>
        /// 
        /// </summary>
        public override string Title => "Test Started Event";

        #endregion //Properties
	
		#region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public EventNodeTestStarted(XmlNode node_)
            : base(node_)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public EventNodeTestStarted()
            : base()
        {

        }

		#endregion //Constructors
	
		#region Methods

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
        /// <param name="param_"></param>
        protected override void TriggeredImpl(object param_)
        {
            SetValueInSlot(1, param_);
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

		#endregion //Methods
    }
}
