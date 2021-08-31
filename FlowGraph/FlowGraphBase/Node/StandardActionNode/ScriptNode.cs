using System;
using System.Collections.Generic;
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
        #region Enum

        public enum NodeSlotId : int
        {
            In,
            Out,
            InputStart,
            OutputStart = 1073741823 // int.MaxValue / 2
        }

        #endregion

        #region Fields

        private int _ScriptElementID = -1; // used when the node is loaded, in order to retrieve the ScriptElement
        private ScriptElement _ScriptElement;

        #endregion //Fields

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public override string Title => "Script " + (GetScriptElement() == null ? "<null>" : _ScriptElement.Name);

        #endregion //Properties

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="el_"></param>
        public ScriptNode(ScriptElement el_)
            : base()
        {
            SetScriptElement(el_);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public ScriptNode(XmlNode node_)
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
                    AddFunctionSlot((int)NodeSlotId.InputStart + e.FunctionSlot.ID, SlotType.VarIn, e.FunctionSlot);
                }
                else if (e.FunctionSlot.SlotType == FunctionSlotType.Output)
                {
                    AddFunctionSlot((int)NodeSlotId.OutputStart + e.FunctionSlot.ID, SlotType.VarOut, e.FunctionSlot);
                }
            }
            else if (e.Type == FunctionSlotChangedType.Removed)
            {
                if (e.FunctionSlot.SlotType == FunctionSlotType.Input)
                {
                    RemoveSlotById((int)NodeSlotId.InputStart + e.FunctionSlot.ID);
                }
                else if (e.FunctionSlot.SlotType == FunctionSlotType.Output)
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
            GetScriptElement();

            foreach (SequenceFunctionSlot slot in _ScriptElement.Inputs)
            {
                AddFunctionSlot((int)NodeSlotId.InputStart + slot.ID, SlotType.VarIn, slot);
            }

            foreach (SequenceFunctionSlot slot in _ScriptElement.Outputs)
            {
                AddFunctionSlot((int)NodeSlotId.OutputStart + slot.ID, SlotType.VarOut, slot);
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
            if (_ScriptElement == null
                && _ScriptElementID != -1)
            {
                SetScriptElement(GraphDataManager.Instance.GetScriptByID(_ScriptElementID));
            }

            return _ScriptElement;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void SetScriptElement(ScriptElement el_)
        {
            _ScriptElement = el_;
            _ScriptElement.PropertyChanged += OnFuntionPropertyChanged;
            _ScriptElement.FunctionSlotChanged += OnFunctionSlotChanged;
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
        public override ProcessingInfo ActivateLogic(ProcessingContext context_, NodeSlot slot_)
        {
            ProcessingInfo info = new ProcessingInfo();
            info.State = LogicState.Ok;

            //call script with input nodes
            List<ScriptSlotData> list = new List<ScriptSlotData>(_ScriptElement.InputCount);
            foreach (NodeSlot slot in SlotVariableIn)
            {
                if (slot is NodeSlotVar)
                {
                    NodeSlotVar varSlot = slot as NodeSlotVar;
                    list.Add(new ScriptSlotData(varSlot.Text, varSlot.ID, GetValueFromSlot(varSlot.ID)));
                }
            }
            ScriptSlotDataCollection parameters = new ScriptSlotDataCollection(list);
            list.Clear();

            //            
            foreach (NodeSlot slot in SlotVariableOut)
            {
                if (slot is NodeSlotVar)
                {
                    NodeSlotVar varSlot = slot as NodeSlotVar;
                    list.Add(new ScriptSlotData(varSlot.Text, varSlot.ID, GetValueFromSlot(varSlot.ID)));
                }
            }
            ScriptSlotDataCollection returnVals = new ScriptSlotDataCollection(list);
            list.Clear();

            if (_ScriptElement.Run(parameters, returnVals) == false)
            {
                info.ErrorMessage = "Some errors in the execution of the script";
                info.State = LogicState.Error;
                return info;
            }

            //set output slot value
            foreach (ScriptSlotData s in returnVals.List)
            {
                SetValueInSlot(s.ID, s.Value);
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
            return new ScriptNode(_ScriptElement);
        }

        #region Persistence

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        protected override void Load(XmlNode node_)
        {
            base.Load(node_);
            _ScriptElementID = int.Parse(node_.Attributes["ScriptElementID"].Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionListNode_"></param>
        /// <param name="sequence_"></param>
        internal override void ResolveLinks(XmlNode connectionListNode_, SequenceBase sequence_)
        {
            GetScriptElement();
            base.ResolveLinks(connectionListNode_, sequence_);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public override void Save(XmlNode node_)
        {
            base.Save(node_);
            node_.AddAttribute("ScriptElementID", GetScriptElement().ID.ToString());
        }

        #endregion // Persistence

        #region Link with ScriptElement

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnFuntionPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Name":
                    OnPropertyChanged("Title");
                    break;
            }
        }

        #endregion // Link with SequenceScriptElement

        #endregion //Methods
    }

    /// <summary>
    /// 
    /// </summary>
    public class ScriptSlotData
    {
        public string Text { get; private set; }
        public int ID { get; private set; }
        public object Value { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text_"></param>
        /// <param name="id_"></param>
        /// <param name="value_"></param>
        public ScriptSlotData(string text_, int id_, object value_)
        {
            Text = text_;
            ID = id_;
            Value = value_;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ScriptSlotDataCollection
    {
        private IList<ScriptSlotData> _List;

        /// <summary>
        /// 
        /// </summary>
        internal IEnumerable<ScriptSlotData> List => _List;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list_"></param>
        public ScriptSlotDataCollection(IEnumerable<ScriptSlotData> list_)
        {
            _List = new List<ScriptSlotData>(list_);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name_"></param>
        /// <returns></returns>
        public ScriptSlotData Get(string name_)
        {
            foreach (ScriptSlotData data in _List)
            {
                if (string.Equals(data.Text, name_))
                {
                    return data;
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name_"></param>
        /// <returns></returns>
        public bool SetValue(string name_, object value_)
        {
            for (int i = 0; i < _List.Count; i++)
            {
                if (string.Equals(_List[i].Text, name_))
                {
                    _List[i].Value = value_;
                    return true;
                }
            }

            return false;
        }
    }
}
