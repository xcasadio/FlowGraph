using System.ComponentModel;
using System.Xml;
using FlowGraph.Process;

namespace FlowGraph.Node
{
    [Flags]
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

    public enum SlotType
    {
        NodeIn,
        NodeOut,
        VarOut,
        VarIn,
        VarInOut, // special case for variable node which can be in/out at the same time
    }

    public class NodeSlot : INotifyPropertyChanged
    {
        public event EventHandler? Activated;

        private string? _text;
        private Type _variableType;
        private VariableControlType _controlType;

        public int Id { get; }
        public SequenceNode Node { get; }
        public SlotType ConnectionType { get; }
        public object? Tag { get; }
        public List<NodeSlot> ConnectedNodes { get; }

        public virtual string? Text
        {
            get => _text;
            set
            {
                if (string.Equals(_text, value, StringComparison.InvariantCultureIgnoreCase) == false)
                {
                    _text = value;
                    OnPropertyChanged("Text");
                }
            }
        }

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

        public VariableControlType ControlType
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

        protected NodeSlot(int slotId, SequenceNode node, SlotType connectionType, VariableControlType controlType = VariableControlType.ReadOnly, object? tag = null)
        {
            ConnectedNodes = new List<NodeSlot>();

            Id = slotId;
            Node = node;
            ConnectionType = connectionType;
            ControlType = controlType;
            Tag = tag;
        }

        public NodeSlot(int slotId, SequenceNode node, string? text, SlotType connectionType, Type type,
            VariableControlType controlType = VariableControlType.ReadOnly, object? tag = null) :
            this(slotId, node, connectionType, controlType, tag)
        {
            Text = text;
            VariableType = type;
        }

        public bool ConnectTo(NodeSlot dst)
        {
            if (dst.Node == Node)
            {
                throw new InvalidOperationException("Try to connect itself");
            }

            foreach (var s in ConnectedNodes)
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

        public bool DisconnectFrom(NodeSlot slot)
        {
            ConnectedNodes.Remove(slot);
            return true;
        }

        public void RemoveAllConnections()
        {
            ConnectedNodes.Clear();
        }

        public void RegisterNodes(ProcessingContext context)
        {
            foreach (var slot in ConnectedNodes)
            {
                if (slot.Node is ActionNode)
                {
                    context.RegisterNextExecution(slot);
                }
            }

            Activated?.Invoke(this, EventArgs.Empty);
        }

        public virtual void Save(XmlNode node)
        {
            const int version = 1;
            node.AddAttribute("version", version.ToString());
            node.AddAttribute("index", Id.ToString());
        }

        public virtual void Load(XmlNode node)
        {
            //var version = int.Parse(node.Attributes["version"].Value);
            //Don't load Id, it is set manually inside the constructor
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
