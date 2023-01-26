using System.ComponentModel;
using System.Xml;

namespace FlowGraphBase.Node.StandardEventNode
{
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

        public override string Title => (GetFunction() == null ? "<null>" : _function.Name) + " function";

        public OnEnterFunctionEvent(SequenceFunction func)
        {
            _function = func;
            _function.PropertyChanged += OnFuntionPropertyChanged;
        }

        public OnEnterFunctionEvent(XmlNode node)
            : base(node)
        {

        }

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

        private SequenceFunction GetFunction()
        {
            if (_function == null
                && _functionId != -1)
            {
                SetFunction(GraphDataManager.Instance.GetFunctionById(_functionId));
            }

            return _function;
        }

        private void SetFunction(SequenceFunction func)
        {
            _function = func;
            _function.PropertyChanged += OnFuntionPropertyChanged;
            _function.FunctionSlotChanged += OnFunctionSlotChanged;
            UpdateNodeSlot();
        }

        protected override void InitializeSlots()
        {
            base.InitializeSlots();

            AddSlot((int)NodeSlotId.Out, "", SlotType.NodeOut);
        }

        protected override void TriggeredImpl(object para)
        {
            //SetValueInSlot(1, para_);
        }

        protected override void Load(XmlNode node)
        {
            base.Load(node);
            _functionId = int.Parse(node.Attributes["functionID"].Value);
        }

        public override void Save(XmlNode node)
        {
            base.Save(node);
            node.AddAttribute("functionID", GetFunction().Id.ToString());
        }

        protected override SequenceNode CopyImpl()
        {
            return new OnEnterFunctionEvent(_function);
        }

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
