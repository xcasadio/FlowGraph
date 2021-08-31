using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using FlowGraphBase.Process;

namespace FlowGraphBase.Node
{
    /// <summary>
    /// 
    /// </summary>
    public enum SlotAvailableFlag
    {
        None = 0,
        NodeIn = 1 << 1,
        NodeOut = 1 << 2,
        VarOut = 1 << 3,
        VarIn = 1 << 4,

        DefaultFlagEvent = NodeOut | VarOut,
        DefaultFlagVariable = VarIn | VarOut,
        DefaultFlagAction = NodeIn | NodeOut,
        All = NodeIn | NodeOut | VarIn | VarOut
    }

    /// <summary>
    /// 
    /// </summary>
    public enum SlotType
    {
        NodeIn,
        NodeOut,
        VarOut,
        VarIn,
        VarInOut, // special case for variable node which can be in/out at the same time
    }

    /// <summary>
    /// A node slot contains all links to the other nodes
    /// </summary>
    public class NodeSlot : INotifyPropertyChanged
    {
        public event EventHandler Activated;

        private string _text;
        private Type _variableType;
        private VariableControlType _controlType;

        public int Id { get; }
        public SequenceNode Node { get; }
        public virtual SlotType ConnectionType { get; }
        public object Tag { get; }
        public List<NodeSlot> ConnectedNodes { get; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Text
        {
            get => _text;
            set
            {
                if (string.Equals(_text, value) == false)
                {
                    _text = value;
                    OnPropertyChanged("Text");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Type VariableType
        {
            get => _variableType;
            set
            {
                if (_variableType != value)
                {
                    _variableType = value;
                    OnPropertyChanged("VariableType");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual VariableControlType ControlType
        {
            get => _controlType;
            set
            {
                if (_controlType != value)
                {
                    _controlType = value;
                    OnPropertyChanged("ControlType");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slotId"></param>
        /// <param name="node"></param>
        /// <param name="connectionType"></param>
        /// <param name="controlType"></param>
        /// <param name="tag"></param>
        protected NodeSlot(int slotId, SequenceNode node, SlotType connectionType,
            VariableControlType controlType = VariableControlType.ReadOnly,
            object tag = null)
        {
            ConnectedNodes = new List<NodeSlot>();

            Id = slotId;
            Node = node;
            ConnectionType = connectionType;
            ControlType = controlType;
            Tag = tag;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slotId"></param>
        /// <param name="node"></param>
        /// <param name="text"></param>
        /// <param name="connectionType"></param>
        /// <param name="type"></param>
        /// <param name="controlType"></param>
        /// <param name="tag"></param>
        public NodeSlot(int slotId, SequenceNode node, string text,
            SlotType connectionType, Type type = null,
            VariableControlType controlType = VariableControlType.ReadOnly,
            object tag = null) :
            this(slotId, node, connectionType, controlType, tag)
        {
            Text = text;
            VariableType = type;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dst"></param>
        public bool ConnectTo(NodeSlot dst)
        {
            if (dst.Node == Node)
            {
                throw new InvalidOperationException("Try to connect itself");
            }

            foreach (NodeSlot s in ConnectedNodes)
            {
                if (s.Node == dst.Node) // already connected
                {
                    return true;
                    //throw new InvalidOperationException("");
                }
            }

            switch (ConnectionType)
            {
                case SlotType.NodeIn:
                case SlotType.NodeOut:
                    if (dst.Node is VariableNode)
                    {
                        return false;
                    }
                    break;

                case SlotType.VarIn:
                case SlotType.VarOut:
                case SlotType.VarInOut:
                    if (dst.Node is VariableNode == false
                        && dst is NodeSlotVar == false)
                    {
                        return false;
                    }
                    break;
            }

            ConnectedNodes.Add(dst);

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slot"></param>
        public bool DisconnectFrom(NodeSlot slot)
        {
            ConnectedNodes.Remove(slot);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveAllConnections()
        {
            ConnectedNodes.Clear();
        }

        /// <summary>
        /// Used to activate the nodes in the next step, see Sequence.Run()
        /// </summary>
        /// <param name="context"></param>
        public void RegisterNodes(ProcessingContext context)
        {
            foreach (NodeSlot slot in ConnectedNodes)
            {
                if (slot.Node is ActionNode)
                {
                    context.RegisterNextExecution(slot);
                }
            }

            Activated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public virtual void Save(XmlNode node)
        {
            const int version = 1;
            node.AddAttribute("version", version.ToString());
            node.AddAttribute("index", Id.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public virtual void Load(XmlNode node)
        {
            int version = int.Parse(node.Attributes["version"].Value);
            //Don't load Id, it is set manually inside the constructor
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// A node slot contains all links to the other nodes
    /// </summary>
    public class NodeSlotVar : NodeSlot
    {
        private readonly ValueContainer _value;
        private readonly bool _saveValue;

        /// <summary>
        /// Used as nested link with a variable node
        /// </summary>
        public object Value
        {
            get => _value.Value;
            set { _value.Value = value; OnPropertyChanged("Value"); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slotId"></param>
        /// <param name="node"></param>
        /// <param name="text"></param>
        /// <param name="connectionType"></param>
        /// <param name="type"></param>
        /// <param name="controlType"></param>
        /// <param name="tag"></param>
        /// <param name="saveValue"></param>
        public NodeSlotVar(int slotId, SequenceNode node, string text,
            SlotType connectionType, Type type = null,
            VariableControlType controlType = VariableControlType.ReadOnly,
            object tag = null, bool saveValue = true) :
            base(slotId, node, text, connectionType, type, controlType, tag)
        {
            _saveValue = saveValue;

            object val = null;

            if (type == typeof(bool))
            {
                val = true;
            }
            else if (type == typeof(sbyte)
                || type == typeof(char)
                || type == typeof(short)
                || type == typeof(int)
                || type == typeof(long)
                || type == typeof(byte)
                || type == typeof(ushort)
                || type == typeof(uint)
                || type == typeof(ulong)
                || type == typeof(float)
                || type == typeof(double))
            {
                val = Convert.ChangeType(0, type);
            }
            else if (type == typeof(string))
            {
                val = string.Empty;
            }

            _value = new ValueContainer(type, val);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public override void Save(XmlNode node)
        {
            base.Save(node);

            node.AddAttribute("saveValue", _saveValue.ToString());

            if (_saveValue)
            {
                _value.Save(node);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public override void Load(XmlNode node)
        {
            base.Load(node);

            if (_saveValue)
            {
                _value.Load(node);
            }
        }
    }

    /// <summary>
    /// Specific node for all nodes linked with a SequenceFunction.
    /// NodeFunctionSlot reflects all changes made in real time.
    /// </summary>
    public class NodeFunctionSlot : NodeSlotVar
    {
        private readonly SequenceFunctionSlot _funcSlot;

        /// <summary>
        /// 
        /// </summary>
        public override string Text
        {
            get => _funcSlot == null ? string.Empty : _funcSlot.Name;
            set
            {
                if (_funcSlot != null)
                {
                    _funcSlot.Name = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override Type VariableType
        {
            get => _funcSlot?.VariableType;
            set
            {
                if (_funcSlot != null)
                {
                    _funcSlot.VariableType = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slotId"></param>
        /// <param name="node"></param>
        /// <param name="connectionType"></param>
        /// <param name="slot"></param>
        /// <param name="controlType"></param>
        /// <param name="tag"></param>
        /// <param name="saveValue"></param>
        public NodeFunctionSlot(
            int slotId,
            SequenceNode node,
            SlotType connectionType,
            SequenceFunctionSlot slot,
            VariableControlType controlType = VariableControlType.ReadOnly,
            object tag = null,
            bool saveValue = true) :

                base(slotId,
                    node,
                    slot.Name,
                    connectionType,
                    slot.VariableType,
                    controlType,
                    tag,
                    saveValue)
        {
            _funcSlot = slot;
            _funcSlot.PropertyChanged += OnFunctionSlotPropertyChanged;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnFunctionSlotPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Name":
                    OnPropertyChanged("Text");
                    break;

                case "VariableType":
                    OnPropertyChanged("VariableType");
                    break;
                    //IsArray
            }
        }
    }
}
