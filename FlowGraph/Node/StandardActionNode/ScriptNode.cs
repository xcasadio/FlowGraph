using System.ComponentModel;
using System.Xml;
using FlowGraph.Process;
using FlowGraph.Script;

namespace FlowGraph.Node.StandardActionNode
{
    [Visible(false)]
    public class ScriptNode
        : ActionNode
    {
        public enum NodeSlotId
        {
            In,
            Out,
            InputStart,
            OutputStart = 1073741823 // int.MaxValue / 2
        }

        private int _scriptElementId = -1; // used when the node is loaded, in order to retrieve the ScriptElement
        private ScriptElement _scriptElement;

        public override string? Title => "Script " + (GetScriptElement() == null ? "<null>" : _scriptElement.Name);

        public ScriptNode(ScriptElement el)
        {
            SetScriptElement(el);
        }

        public ScriptNode(XmlNode node)
            : base(node)
        {

        }

        private void OnFunctionSlotChanged(object sender, FunctionSlotChangedEventArg e)
        {
            if (e.Type == FunctionSlotChangedType.Added)
            {
                if (e.FunctionSlot.SlotType == FunctionSlotType.Input)
                {
                    AddFunctionSlot((int)NodeSlotId.InputStart + e.FunctionSlot.Id, SlotType.VarIn, e.FunctionSlot);
                }
                else if (e.FunctionSlot.SlotType == FunctionSlotType.Output)
                {
                    AddFunctionSlot((int)NodeSlotId.OutputStart + e.FunctionSlot.Id, SlotType.VarOut, e.FunctionSlot);
                }
            }
            else if (e.Type == FunctionSlotChangedType.Removed)
            {
                if (e.FunctionSlot.SlotType == FunctionSlotType.Input)
                {
                    RemoveSlotById((int)NodeSlotId.InputStart + e.FunctionSlot.Id);
                }
                else if (e.FunctionSlot.SlotType == FunctionSlotType.Output)
                {
                    RemoveSlotById((int)NodeSlotId.OutputStart + e.FunctionSlot.Id);
                }
            }

            OnPropertyChanged("Slots");
        }

        private void UpdateNodeSlot()
        {
            GetScriptElement();

            foreach (var slot in _scriptElement.Inputs)
            {
                AddFunctionSlot((int)NodeSlotId.InputStart + slot.Id, SlotType.VarIn, slot);
            }

            foreach (var slot in _scriptElement.Outputs)
            {
                AddFunctionSlot((int)NodeSlotId.OutputStart + slot.Id, SlotType.VarOut, slot);
            }

            OnPropertyChanged("Slots");
            //OnPropertyChanged("SlotVariableIn");
            //OnPropertyChanged("SlotVariableOut");

            //             SlotConnectorIn
            //             SlotConnectorOut
            //             SlotVariableIn
            //             SlotVariableOut
            //             SlotVariableInOut
        }

        private ScriptElement GetScriptElement()
        {
            if (_scriptElement == null
                && _scriptElementId != -1)
            {
                SetScriptElement(GraphDataManager.Instance.GetScriptById(_scriptElementId));
            }

            return _scriptElement;
        }

        private void SetScriptElement(ScriptElement el)
        {
            _scriptElement = el;
            _scriptElement.PropertyChanged += OnFuntionPropertyChanged;
            _scriptElement.FunctionSlotChanged += OnFunctionSlotChanged;
            UpdateNodeSlot();
        }

        protected override void InitializeSlots()
        {
            base.InitializeSlots();

            AddSlot((int)NodeSlotId.In, "", SlotType.NodeIn);
            AddSlot((int)NodeSlotId.Out, "Completed", SlotType.NodeOut);
        }

        public override ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot)
        {
            var info = new ProcessingInfo
            {
                State = LogicState.Ok
            };

            //call script with input nodes
            var list = new List<ScriptSlotData>(_scriptElement.InputCount);
            foreach (var slotVarIn in SlotVariableIn)
            {
                if (slotVarIn is NodeSlotVar varSlot)
                {
                    list.Add(new ScriptSlotData(varSlot.Text, varSlot.Id, GetValueFromSlot(varSlot.Id)));
                }
            }
            var parameters = new ScriptSlotDataCollection(list);
            list.Clear();

            //            
            foreach (var slotVarOut in SlotVariableOut)
            {
                if (slotVarOut is NodeSlotVar varSlot)
                {
                    list.Add(new ScriptSlotData(varSlot.Text, varSlot.Id, GetValueFromSlot(varSlot.Id)));
                }
            }
            var returnVals = new ScriptSlotDataCollection(list);
            list.Clear();

            if (_scriptElement.Run(parameters, returnVals) == false)
            {
                info.ErrorMessage = "Some errors in the execution of the script";
                info.State = LogicState.Error;
                return info;
            }

            //set output slot value
            foreach (var s in returnVals.List)
            {
                SetValueInSlot(s.Id, s.Value);
            }

            ActivateOutputLink(context, (int)NodeSlotId.Out);

            return info;
        }

        protected override SequenceNode CopyImpl()
        {
            return new ScriptNode(_scriptElement);
        }

        protected override void Load(XmlNode node)
        {
            base.Load(node);
            _scriptElementId = int.Parse(node.Attributes["ScriptElementID"].Value);
        }

        internal override void ResolveLinks(XmlNode connectionListNode, SequenceBase sequence)
        {
            GetScriptElement();
            base.ResolveLinks(connectionListNode, sequence);
        }

        public override void Save(XmlNode node)
        {
            base.Save(node);
            node.AddAttribute("ScriptElementID", GetScriptElement().Id.ToString());
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

    public class ScriptSlotData
    {
        public string? Text { get; }
        public int Id { get; }
        public object? Value { get; internal set; }

        public ScriptSlotData(string? text, int id, object? value)
        {
            Text = text;
            Id = id;
            Value = value;
        }
    }

    public class ScriptSlotDataCollection
    {
        private readonly IList<ScriptSlotData> _list;

        internal IEnumerable<ScriptSlotData> List => _list;

        public ScriptSlotDataCollection(IEnumerable<ScriptSlotData> list)
        {
            _list = new List<ScriptSlotData>(list);
        }

        public ScriptSlotData Get(string name)
        {
            foreach (var data in _list)
            {
                if (string.Equals(data.Text, name))
                {
                    return data;
                }
            }

            return null;
        }

        public bool SetValue(string name, object? value)
        {
            for (var i = 0; i < _list.Count; i++)
            {
                if (string.Equals(_list[i].Text, name))
                {
                    _list[i].Value = value;
                    return true;
                }
            }

            return false;
        }
    }
}
