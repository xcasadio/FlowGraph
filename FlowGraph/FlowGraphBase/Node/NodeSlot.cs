using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using FlowGraphBase.Process;
using System.Xml;

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
		#region Fields

        public event EventHandler Activated;

        private string m_Text;
        private Type m_VariableType;
        private VariableControlType m_ControlType;

		#endregion //Fields
	
		#region Properties

        public int ID { get; private set; }
        public SequenceNode Node { get; private set; }
        public virtual SlotType ConnectionType { get; private set; }
        public object Tag { get; private set; }
        public List<NodeSlot> ConnectedNodes { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Text
        {
            get { return m_Text; }
            set 
            {
                if (string.Equals(m_Text, value) == false)
                {
                    m_Text = value;
                    OnPropertyChanged("Text");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Type VariableType
        {
            get { return m_VariableType; }
            set
            {
                if (m_VariableType != value)
                {
                    m_VariableType = value;
                    OnPropertyChanged("VariableType");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual VariableControlType ControlType
        {
            get { return m_ControlType; }
            set
            {
                if (m_ControlType != value)
                {
                    m_ControlType = value;
                    OnPropertyChanged("ControlType");
                }
            }
        }

		#endregion //Properties
	
		#region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slotId_"></param>
        /// <param name="node_"></param>
        /// <param name="connectionType_"></param>
        /// <param name="controlType_"></param>
        /// <param name="tag_"></param>
        protected NodeSlot(int slotId_, SequenceNode node_, SlotType connectionType_,
            VariableControlType controlType_ = VariableControlType.ReadOnly,
            object tag_ = null)
        {
            ConnectedNodes = new List<NodeSlot>();

            ID = slotId_;
            Node = node_;
            ConnectionType = connectionType_;
            ControlType = controlType_;
            Tag = tag_;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slotId_"></param>
        /// <param name="node_"></param>
        /// <param name="text_"></param>
        /// <param name="connectionType_"></param>
        /// <param name="type_"></param>
        /// <param name="controlType_"></param>
        /// <param name="tag_"></param>
        public NodeSlot(int slotId_, SequenceNode node_, string text_,
            SlotType connectionType_, Type type_ = null,
            VariableControlType controlType_ = VariableControlType.ReadOnly,
            object tag_ = null) :
            this(slotId_, node_, connectionType_, controlType_, tag_)
        {
            Text = text_;
            VariableType = type_;
        }

		#endregion //Constructors
	
		#region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dst_"></param>
        public bool ConnectTo(NodeSlot dst_)
        {
            if (dst_.Node == Node)
            {
                throw new InvalidOperationException("Try to connect itself");
            }

            foreach (NodeSlot s in ConnectedNodes)
            {
                if (s.Node == dst_.Node) // already connected
                {
                    return true;
                    //throw new InvalidOperationException("");
                }
            }

            switch (ConnectionType)
            {
                case SlotType.NodeIn:
                case SlotType.NodeOut:
                    if ((dst_.Node is VariableNode) == true)
                    {
                        return false;
                    }
                    break;

                case SlotType.VarIn:
                case SlotType.VarOut:
                case SlotType.VarInOut:
                    if ((dst_.Node is VariableNode) == false
                        && (dst_ is NodeSlotVar) == false)
                    {
                        return false;
                    }
                    break;
            }

            ConnectedNodes.Add(dst_);

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slot_"></param>
        public bool DisconnectFrom(NodeSlot slot_)
        {
            ConnectedNodes.Remove(slot_);
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
        /// <param name="context_"></param>
        public void RegisterNodes(ProcessingContext context_)
        {
            foreach (NodeSlot slot in ConnectedNodes)
            {
                if (slot.Node is ActionNode)
                {
                    context_.RegisterNextExecution(slot);
                }
            }

            if (Activated != null)
            {
                Activated.Invoke(this, EventArgs.Empty);
            }
        }

        #region Persistence

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public virtual void Save(XmlNode node_)
        {
            const int version = 1;
            node_.AddAttribute("version", version.ToString());
            node_.AddAttribute("index", ID.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public virtual void Load(XmlNode node_)
        {
            int version = int.Parse(node_.Attributes["version"].Value);
            //Don't load ID, it is set manually inside the constructor
        }

        #endregion // Persistence

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion // INotifyPropertyChanged

		#endregion //Methods
    }

    /// <summary>
    /// A node slot contains all links to the other nodes
    /// </summary>
    public class NodeSlotVar : NodeSlot
    {
        #region Fields

        private ValueContainer m_Value;
        private bool m_SaveValue;

        #endregion //Fields

        #region Properties

        /// <summary>
        /// Used as nested link with a variable node
        /// </summary>
        public object Value
        {
            get { return m_Value.Value; }
            set { m_Value.Value = value; OnPropertyChanged("Value"); }
        }

        #endregion //Properties

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slotId_"></param>
        /// <param name="node_"></param>
        /// <param name="text_"></param>
        /// <param name="connectionType_"></param>
        /// <param name="type_"></param>
        /// <param name="controlType_"></param>
        /// <param name="tag_"></param>
        public NodeSlotVar(int slotId_, SequenceNode node_, string text_,
            SlotType connectionType_, Type type_ = null,
            VariableControlType controlType_ = VariableControlType.ReadOnly,
            object tag_ = null, bool saveValue_ = true) :
            base(slotId_, node_, text_, connectionType_, type_, controlType_, tag_)
        {
            m_SaveValue = saveValue_;

            object val = null;

            if (type_ == typeof(bool))
            {
                val = true;
            }
            else if (type_ == typeof(sbyte)
                || type_ == typeof(char)
                || type_ == typeof(short)
                || type_ == typeof(int)
                || type_ == typeof(long)
                || type_ == typeof(byte)
                || type_ == typeof(ushort)
                || type_ == typeof(uint)
                || type_ == typeof(ulong)
                || type_ == typeof(float)
                || type_ == typeof(double))
            {
                val = Convert.ChangeType(0, type_);
            }
            else if (type_ == typeof(string))
            {
                val = string.Empty;
            }

            m_Value = new ValueContainer(type_, val);
        }

        #endregion //Constructors

        #region Methods

        #region Persistence

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public override void Save(XmlNode node_)
        {
            base.Save(node_);

            node_.AddAttribute("saveValue", m_SaveValue.ToString());

            if (m_SaveValue == true)
            {
                m_Value.Save(node_);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public override void Load(XmlNode node_)
        {
            base.Load(node_);

            if (m_SaveValue == true)
            {
                m_Value.Load(node_);
            }
        }

        #endregion // Persistence

        #endregion //Methods
    }

    /// <summary>
    /// Specific node for all nodes linked with a SequenceFunction.
    /// NodeFunctionSlot reflects all changes made in real time.
    /// </summary>
    public class NodeFunctionSlot : NodeSlotVar
    {
        #region Fields

        private SequenceFunctionSlot m_FuncSlot;

        #endregion //Fields

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public override string Text
        {
            get { return m_FuncSlot == null ? string.Empty : m_FuncSlot.Name; }
            set 
            {
                if (m_FuncSlot != null)
                {
                    m_FuncSlot.Name = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override Type VariableType
        {
            get { return m_FuncSlot == null ? null : m_FuncSlot.VariableType; }
            set
            {
                if (m_FuncSlot != null)
                {
                    m_FuncSlot.VariableType = value;
                }
            }
        }

        #endregion //Properties

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slotId_"></param>
        /// <param name="node_"></param>
        /// <param name="connectionType_"></param>
        /// <param name="slot_"></param>
        /// <param name="controlType_"></param>
        /// <param name="tag_"></param>
        /// <param name="saveValue_"></param>
        public NodeFunctionSlot(
            int slotId_, 
            SequenceNode node_, 
            SlotType connectionType_, 
            SequenceFunctionSlot slot_,
            VariableControlType controlType_ = VariableControlType.ReadOnly,
            object tag_ = null, 
            bool saveValue_ = true) :

                base(slotId_, 
                    node_, 
                    slot_.Name,
                    connectionType_, 
                    slot_.VariableType,
                    controlType_,
                    tag_, 
                    saveValue_)
        {
            m_FuncSlot = slot_;
            m_FuncSlot.PropertyChanged += new PropertyChangedEventHandler(OnFunctionSlotPropertyChanged);
        }

        #endregion //Constructors

        #region Methods

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

        #endregion //Methods
    }
}
