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
        public enum NodeSlotId
        {
            Out,
            OutputStart
        }

        private int _functionId = -1; // used when the node is loaded, in order to retrieve the function
        private SequenceFunction _function;

        /// <summary>
        /// 
        /// </summary>
        public override string Title => (GetFunction() == null ? "<null>" : _function.Name) + " function";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public OnEnterFunctionEvent(SequenceFunction func)
        {
            _function = func;
            _function.PropertyChanged += OnFuntionPropertyChanged;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public OnEnterFunctionEvent(XmlNode node)
            : base(node)
        {

        }

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
                    AddFunctionSlot((int)NodeSlotId.OutputStart + e.FunctionSlot.Id, SlotType.VarOut, e.FunctionSlot);
                    //AddSlot((int)NodeSlotId.OutputStart + e.FunctionSlot.Id, e.FunctionSlot.Name, SlotType.VarOut, typeof(int));
                }
            }
            else if (e.Type == FunctionSlotChangedType.Removed)
            {
                if (e.FunctionSlot.SlotType == FunctionSlotType.Input)
                {
                    RemoveSlotById((int)NodeSlotId.OutputStart + e.FunctionSlot.Id);
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

            foreach (SequenceFunctionSlot slot in _function.Inputs)
            {
                AddFunctionSlot((int)NodeSlotId.OutputStart + slot.Id, SlotType.VarOut, slot);
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
            if (_function == null
                && _functionId != -1)
            {
                SetFunction(GraphDataManager.Instance.GetFunctionById(_functionId));
            }

            return _function;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void SetFunction(SequenceFunction func)
        {
            _function = func;
            _function.PropertyChanged += OnFuntionPropertyChanged;
            _function.FunctionSlotChanged += OnFunctionSlotChanged;
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
        protected override void TriggeredImpl(object para)
        {
            //SetValueInSlot(1, para_);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        protected override void Load(XmlNode node)
        {
            base.Load(node);
            _functionId = int.Parse(node.Attributes["functionID"].Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public override void Save(XmlNode node)
        {
            base.Save(node);
            node.AddAttribute("functionID", GetFunction().Id.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override SequenceNode CopyImpl()
        {
            return new OnEnterFunctionEvent(_function);
        }

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
    }
}
