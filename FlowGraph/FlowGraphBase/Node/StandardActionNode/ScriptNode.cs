using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using FlowGraphBase.Process;
using FlowGraphBase.Script;

namespace FlowGraphBase.Node.StandardActionNode
{
    /// <summary>
    /// 
    /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public override string Title => "Script " + (GetScriptElement() == null ? "<null>" : _scriptElement.Name);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="el"></param>
        public ScriptNode(ScriptElement el)
        {
            SetScriptElement(el);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public ScriptNode(XmlNode node)
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

        /// <summary>
        /// 
        /// </summary>
        private void UpdateNodeSlot()
        {
            GetScriptElement();

            foreach (SequenceFunctionSlot slot in _scriptElement.Inputs)
            {
                AddFunctionSlot((int)NodeSlotId.InputStart + slot.Id, SlotType.VarIn, slot);
            }

            foreach (SequenceFunctionSlot slot in _scriptElement.Outputs)
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private ScriptElement GetScriptElement()
        {
            if (_scriptElement == null
                && _scriptElementId != -1)
            {
                SetScriptElement(GraphDataManager.Instance.GetScriptById(_scriptElementId));
            }

            return _scriptElement;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void SetScriptElement(ScriptElement el)
        {
            _scriptElement = el;
            _scriptElement.PropertyChanged += OnFuntionPropertyChanged;
            _scriptElement.FunctionSlotChanged += OnFunctionSlotChanged;
            UpdateNodeSlot();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitializeSlots()
        {
            base.InitializeSlots();

            AddSlot((int)NodeSlotId.In, "", SlotType.NodeIn);
            AddSlot((int)NodeSlotId.Out, "Completed", SlotType.NodeOut);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot)
        {
            ProcessingInfo info = new ProcessingInfo
            {
                State = LogicState.Ok
            };

            //call script with input nodes
            List<ScriptSlotData> list = new List<ScriptSlotData>(_scriptElement.InputCount);
            foreach (NodeSlot slotVarIn in SlotVariableIn)
            {
                if (slotVarIn is NodeSlotVar varSlot)
                {
                    list.Add(new ScriptSlotData(varSlot.Text, varSlot.Id, GetValueFromSlot(varSlot.Id)));
                }
            }
            ScriptSlotDataCollection parameters = new ScriptSlotDataCollection(list);
            list.Clear();

            //            
            foreach (NodeSlot slotVarOut in SlotVariableOut)
            {
                if (slotVarOut is NodeSlotVar varSlot)
                {
                    list.Add(new ScriptSlotData(varSlot.Text, varSlot.Id, GetValueFromSlot(varSlot.Id)));
                }
            }
            ScriptSlotDataCollection returnVals = new ScriptSlotDataCollection(list);
            list.Clear();

            if (_scriptElement.Run(parameters, returnVals) == false)
            {
                info.ErrorMessage = "Some errors in the execution of the script";
                info.State = LogicState.Error;
                return info;
            }

            //set output slot value
            foreach (ScriptSlotData s in returnVals.List)
            {
                SetValueInSlot(s.Id, s.Value);
            }

            ActivateOutputLink(context, (int)NodeSlotId.Out);

            return info;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override SequenceNode CopyImpl()
        {
            return new ScriptNode(_scriptElement);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        protected override void Load(XmlNode node)
        {
            base.Load(node);
            _scriptElementId = int.Parse(node.Attributes["ScriptElementID"].Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionListNode"></param>
        /// <param name="sequence"></param>
        internal override void ResolveLinks(XmlNode connectionListNode, SequenceBase sequence)
        {
            GetScriptElement();
            base.ResolveLinks(connectionListNode, sequence);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public override void Save(XmlNode node)
        {
            base.Save(node);
            node.AddAttribute("ScriptElementID", GetScriptElement().Id.ToString());
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

    /// <summary>
    /// 
    /// </summary>
    public class ScriptSlotData
    {
        public string Text { get; }
        public int Id { get; }
        public object Value { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public ScriptSlotData(string text, int id, object value)
        {
            Text = text;
            Id = id;
            Value = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ScriptSlotDataCollection
    {
        private readonly IList<ScriptSlotData> _list;

        /// <summary>
        /// 
        /// </summary>
        internal IEnumerable<ScriptSlotData> List => _list;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        public ScriptSlotDataCollection(IEnumerable<ScriptSlotData> list)
        {
            _list = new List<ScriptSlotData>(list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ScriptSlotData Get(string name)
        {
            foreach (ScriptSlotData data in _list)
            {
                if (string.Equals(data.Text, name))
                {
                    return data;
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool SetValue(string name, object value)
        {
            for (int i = 0; i < _list.Count; i++)
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
