using System.ComponentModel;
using System.Xml;

namespace FlowGraphBase.Node.StandardEventNode
{
    /// <summary>
    /// 
    /// </summary>
    [Visible(false)]
    internal class OnEnterFunctionEvent : EventNode
    {
        #region Enum

        public enum NodeSlotId
        {
            Out,
            OutputStart
        }

        #endregion

		#region Fields

        private int _FunctionID = -1; // used when the node is loaded, in order to retrieve the function
        private SequenceFunction _Function;

		#endregion //Fields
	
		#region Properties

        /// <summary>
        /// 
        /// </summary>
        public override string Title => (GetFunction() == null ? "<null>" : _Function.Name) + " function";

        #endregion //Properties
	
		#region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public OnEnterFunctionEvent(SequenceFunction func_)
        {
            _Function = func_;
            _Function.PropertyChanged += OnFuntionPropertyChanged;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public OnEnterFunctionEvent(XmlNode node_)
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
                if (e.FunctionSlot.SlotType == FunctionSlotType.Input)
                {
                    AddFunctionSlot((int)NodeSlotId.OutputStart + e.FunctionSlot.ID, SlotType.VarOut, e.FunctionSlot);
                    //AddSlot((int)NodeSlotId.OutputStart + e.FunctionSlot.Id, e.FunctionSlot.Name, SlotType.VarOut, typeof(int));
                }
            }
            else if (e.Type == FunctionSlotChangedType.Removed)
            {
                if (e.FunctionSlot.SlotType == FunctionSlotType.Input)
                {
                    RemoveSlotById((int)NodeSlotId.OutputStart + e.FunctionSlot.ID);
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

            foreach (SequenceFunctionSlot slot in _Function.Inputs)
            {
                AddFunctionSlot((int)NodeSlotId.OutputStart + slot.ID, SlotType.VarOut, slot);
                //AddSlot((int)NodeSlotId.OutputStart + slot.Id, slot.Name, SlotType.VarOut, typeof(int));
            }

            OnPropertyChanged("Slots");
            //OnPropertyChanged("SlotVariableOut");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private SequenceFunction GetFunction()
        {
            if (_Function == null
                && _FunctionID != -1)
            {
                SetFunction(GraphDataManager.Instance.GetFunctionByID(_FunctionID));
            }

            return _Function;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void SetFunction(SequenceFunction func_)
        {
            _Function = func_;
            _Function.PropertyChanged += OnFuntionPropertyChanged;
            _Function.FunctionSlotChanged += OnFunctionSlotChanged;
            UpdateNodeSlot();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitializeSlots()
        {
            base.InitializeSlots();

            AddSlot((int) NodeSlotId.Out, "", SlotType.NodeOut);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="para_"></param>
        protected override void TriggeredImpl(object para_)
        {
            //SetValueInSlot(1, para_);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        protected override void Load(XmlNode node_)
        {
            base.Load(node_);
            _FunctionID = int.Parse(node_.Attributes["functionID"].Value);
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override SequenceNode CopyImpl()
        {
            return new OnEnterFunctionEvent(_Function);
        }

        #region Link with SequenceFunction

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnFuntionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Name":
                    OnPropertyChanged("Title");
                    break;
            }
        }

        #endregion // Link with SequenceFunction

        #endregion //Methods

    }
}
