using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlowGraphBase;
using FlowGraphBase.Node;
using System.Xml;
using FlowGraphBase.Logger;
using FlowGraphBase.Process;


namespace FlowGraphBase.Node.StandardActionNode
{
    /// <summary>
    /// 
    /// </summary>
    [Category("Time"), Name("Delay")]
    public class DelayNode
        : ActionNode
    {
        #region Enum

        public enum NodeSlotId
        {
            In,
            Out,
            Delay
        }

        #endregion

		#region Fields

        //TimeSpan m_StartTime = TimeSpan.Zero;

		#endregion //Fields
	
		#region Properties

        public override string Title { get { return "Delay"; } }

		#endregion //Properties
	
		#region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public DelayNode(XmlNode node_)
            : base(node_)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public DelayNode()
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

            AddSlot((int)NodeSlotId.In, "", SlotType.NodeIn);
            AddSlot((int)NodeSlotId.Out, "Completed", SlotType.NodeOut);

            AddSlot((int)NodeSlotId.Delay, "Duration (ms)", SlotType.VarIn, typeof(int));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ProcessingInfo ActivateLogic(ProcessingContext context_, NodeSlot slot_)
        {
            ProcessingInfo info = new ProcessingInfo();
            info.State = ActionNode.LogicState.Ok;

            object intVal = GetValueFromSlot((int)NodeSlotId.Delay);

            if (intVal == null)
            {
                info.State = ActionNode.LogicState.Ok;
                info.ErrorMessage = "Please connect a integer variable node";

                LogManager.Instance.WriteLine(LogVerbosity.Error,
                    "{0} : {1}.",
                    Title, info.ErrorMessage);

                return info;
            }

            MemoryStackItem memoryItem = context_.CurrentFrame.GetValueFromID(Id);

            if (memoryItem == null)
            {
                memoryItem = context_.CurrentFrame.Allocate(Id, TimeSpan.Zero);
            }

            TimeSpan startTime = (TimeSpan)memoryItem.Value;

            int delay = (int)intVal;
            double delayDouble = ((double)delay) / 1000.0;
            TimeSpan t = DateTime.Now.TimeOfDay.Subtract(startTime);
            double totalSecs = t.TotalSeconds;

            //this is the first time, so we set the current time
            if (startTime == TimeSpan.Zero)
            {
                totalSecs = 0.0;
                startTime = DateTime.Now.TimeOfDay;
                memoryItem.Value = startTime;
            }
            else if (totalSecs >= delayDouble)
            {
                startTime = TimeSpan.Zero;
                memoryItem.Value = startTime;
                ActivateOutputLink(context_, (int)NodeSlotId.Out);
                CustomText = String.Empty;
                return info;
            }

            CustomText = string.Format("Delay ({0:0.000} seconds left)", delayDouble - totalSecs);

            context_.RegisterNextExecution(GetSlotById((int)NodeSlotId.In));
            return info;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override SequenceNode CopyImpl()
        {
            return new DelayNode();
        }

		#endregion //Methods
    }
}
