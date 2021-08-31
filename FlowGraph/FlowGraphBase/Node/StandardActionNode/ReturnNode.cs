using System;
using System.Collections.Generic;
using System.Xml;
using FlowGraphBase.Process;

namespace FlowGraphBase.Node.StandardActionNode
{
    /// <summary>
    /// 
    /// </summary>
    [Visible(false)]
    public class ReturnNode
        : ActionNode
    {
        #region Enum

        public enum NodeSlotId : int
        {
            In,
            InputStart
        }

        #endregion

        #region Fields

        private int m_FunctionID = -1; // used when the node is loaded, in order to retrieve the function
        private SequenceFunction m_Function;

        private List<int> m_OutputIds = new List<int>();
        #endregion //Fields

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public override string Title
        {
            get 
            {
                GetFunction(); // TODO : ugly but the fast way to initialize
                return "ReturnNode"; 
            }
        }

        #endregion //Properties

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="functionID_"></param>
        public ReturnNode(SequenceFunction function_)
            : base()
        {
            m_Function = function_;
            m_Function.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(OnFuntionPropertyChanged);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public ReturnNode(XmlNode node_)
            : base(node_)
        {

        }

        #endregion //Constructors

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFunctionSlotChanged(object sender, FunctionSlotChangedEventArg e)
        {
            if (e.Type == FunctionSlotChangedType.Added)
            {
                if (e.FunctionSlot.SlotType == FunctionSlotType.Output)
                {
                    AddFunctionSlot((int)NodeSlotId.InputStart + e.FunctionSlot.ID, SlotType.VarIn, e.FunctionSlot);
                    //AddSlot((int)NodeSlotId.InputStart + e.FunctionSlot.Id, e.FunctionSlot.Name, SlotType.VarIn, typeof(int));
                }
            }
            else if (e.Type == FunctionSlotChangedType.Removed)
            {
                if (e.FunctionSlot.SlotType == FunctionSlotType.Output)
                {
                    RemoveSlotById((int)NodeSlotId.InputStart + e.FunctionSlot.ID);
                }
            }

            OnPropertyChanged("Slots");
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateNodeSlot()
        {
            GetFunction();

            foreach (SequenceFunctionSlot slot in m_Function.Outputs)
            {
                AddFunctionSlot((int)NodeSlotId.InputStart + slot.ID, SlotType.VarIn, slot);
                //AddSlot((int)NodeSlotId.InputStart + slot.Id, slot.Name, SlotType.VarIn, typeof(int));
            }

            OnPropertyChanged("Slots");
            //OnPropertyChanged("SlotVariableIn");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private SequenceFunction GetFunction()
        {
            if (m_Function == null
                && m_FunctionID != -1)
            {
                SetFunction(GraphDataManager.Instance.GetFunctionByID(m_FunctionID));
            }

            return m_Function;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void SetFunction(SequenceFunction func_)
        {
            m_Function = func_;
            m_Function.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(OnFuntionPropertyChanged);
            m_Function.FunctionSlotChanged += new EventHandler<FunctionSlotChangedEventArg>(OnFunctionSlotChanged);
            UpdateNodeSlot();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitializeSlots()
        {
            base.InitializeSlots();

            AddSlot((int)NodeSlotId.In, "", SlotType.NodeIn);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ProcessingInfo ActivateLogic(ProcessingContext context_, NodeSlot slot_)
        {
            ProcessingInfo info = new ProcessingInfo();
            info.State = LogicState.Ok;

            //TODO
            //Set output variable

            // the nodes executed after the node CallFunctionNode are already registered
            // we only have to delete the subsequence
            context_.RemoveLastSequence();

            return info;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override SequenceNode CopyImpl()
        {
            return new ReturnNode(m_Function);
        }

        #region Persistence

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        protected override void Load(XmlNode node_)
        {
            base.Load(node_);
            m_FunctionID = int.Parse(node_.Attributes["functionID"].Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public override void Save(XmlNode node_)
        {
            base.Save(node_);
            node_.AddAttribute("functionID", GetFunction().Id.ToString());
        }

        #endregion // Persistence

        #region Link with SequenceFunction

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnFuntionPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }

        #endregion // Link with SequenceFunction

        #endregion //Methods
    }
}
