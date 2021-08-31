using System;
using System.Collections.Generic;
using System.Xml;
using System.ComponentModel;

namespace FlowGraphBase.Node
{
    /// <summary>
    /// 
    /// </summary>
    partial class SequenceNode
    {
        //static private readonly int UndefinedID = -1;
        private static int _freeId = 0;

        #region Fields

        //Used to convert a SequenceNode to a Node (graphic node)
        protected List<NodeSlot> _Slots = new List<NodeSlot>();

        private string _CustomText;

        private bool _IsProcessing = false;

        #endregion // Fields

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public abstract NodeType NodeType
        {
            get;
        }

        /// <summary>
        /// Gets which slot can be added into this node
        /// </summary>
        [Browsable(false)]
        public SlotAvailableFlag SlotFlag
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public abstract string Title
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Comment
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string CustomText
        {
            get => _CustomText;
            set 
            { 
                _CustomText = value;
                OnPropertyChanged("CustomText");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public NodeSlot[] Slots => _Slots.ToArray();

        /// <summary>
        /// 
        /// </summary>
        public bool IsProcessing
        {
            get => _IsProcessing;
            set 
            {
                _IsProcessing = value;
                OnPropertyChanged("IsProcessing");
            }
        }

        #region Creation

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public NodeSlot SlotConnectorIn
        {
            get
            {
                foreach (var slot in _Slots)
                {
                    if (slot.ConnectionType == SlotType.NodeIn)
                    {
                        return slot;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public IEnumerable<NodeSlot> SlotConnectorOut
        {
            get
            {
                foreach (var slot in _Slots)
                {
                    if (slot.ConnectionType == SlotType.NodeOut)
                    {
                        yield return slot;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public int SlotConnectorOutCount
        {
            get
            {
                int i = 0;

                foreach (var slot in _Slots)
                {
                    if (slot.ConnectionType == SlotType.NodeOut)
                    {
                        i++;
                    }
                }

                return i;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public IEnumerable<NodeSlot> SlotVariableIn
        {
            get
            {
                foreach (var slot in _Slots)
                {
                    if (slot.ConnectionType == SlotType.VarIn)
                    {
                        yield return slot;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public int SlotVariableInCount
        {
            get
            {
                int i = 0;

                foreach (var slot in _Slots)
                {
                    if (slot.ConnectionType == SlotType.VarIn)
                    {
                        i++;
                    }
                }

                return i;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public IEnumerable<NodeSlot> SlotVariableOut
        {
            get
            {
                foreach (var slot in _Slots)
                {
                    if (slot.ConnectionType == SlotType.VarOut)
                    {
                        yield return slot;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public int SlotVariableOutCount
        {
            get
            {
                int i = 0;

                foreach (var slot in _Slots)
                {
                    if (slot.ConnectionType == SlotType.VarOut)
                    {
                        i++;
                    }
                }

                return i;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public IEnumerable<NodeSlot> SlotVariableInOut
        {
            get
            {
                foreach (var slot in _Slots)
                {
                    if (slot.ConnectionType == SlotType.VarInOut)
                    {
                        yield return slot;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public int SlotVariableInOutCount
        {
            get
            {
                int i = 0;

                foreach (var slot in _Slots)
                {
                    if (slot.ConnectionType == SlotType.VarInOut)
                    {
                        i++;
                    }
                }

                return i;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public bool HasSlotConnectorIn => (SlotFlag & SlotAvailableFlag.NodeIn) == SlotAvailableFlag.NodeIn;

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public bool HasSlotConnectorOut => (SlotFlag & SlotAvailableFlag.NodeOut) == SlotAvailableFlag.NodeOut;

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public bool HasSlotVariableIn => (SlotFlag & SlotAvailableFlag.VarIn) == SlotAvailableFlag.VarIn;

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public bool HasSlotVariableOut => (SlotFlag & SlotAvailableFlag.VarOut) == SlotAvailableFlag.VarOut;

        #endregion

        #endregion // Properties

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seqMgr_"></param>
        protected SequenceNode()
        {
            Id = ++_freeId;
            InitializeSlots();
        }

        #endregion // Constructor

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            CustomText = null;
            IsProcessing = false;
        }

        #region Creation

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionType_"></param>
        /// <param name="slot_"></param>
        protected void AddFunctionSlot(int slotId, SlotType connectionType_, SequenceFunctionSlot slot_)
        {
            AddSlot(new NodeFunctionSlot(slotId, this, connectionType_, slot_));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slotId"></param>
        /// <param name="text_"></param>
        /// <param name="connectionType_"></param>
        /// <param name="type_"></param>
        /// <param name="saveInternalValue_"></param>
        /// <param name="controlType_"></param>
        /// <param name="tag_"></param>
        protected void AddSlot(int slotId, string text_, SlotType connectionType_, Type type_ = null,
            bool saveInternalValue_ = true, VariableControlType controlType_ = VariableControlType.ReadOnly,
            object tag_ = null)
        {
            if (connectionType_ == SlotType.VarIn
                || connectionType_ == SlotType.VarOut)
            {
                AddSlot(new NodeSlotVar(slotId, this, text_, connectionType_, type_, controlType_, tag_, saveInternalValue_));
            }
            else
            {
                AddSlot(new NodeSlot(slotId, this, text_, connectionType_, type_, controlType_, tag_));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ite_"></param>
        private void AddSlot(NodeSlot ite_)
        {
            foreach (NodeSlot slot in _Slots)
            {
                if (slot.ID == ite_.ID)
                {
                    throw new InvalidOperationException("A slot with the Id '" + ite_.ID + "' already exists.");
                }
            }

            if (HasSlotConnectorIn == false
                && ite_.ConnectionType == SlotType.NodeIn)
            {
                throw new InvalidOperationException("This type of node can not have IN connector.");
            }

            if (HasSlotConnectorOut == false
                && ite_.ConnectionType == SlotType.NodeOut)
            {
                throw new InvalidOperationException("This type of node can not have OUT connector.");
            }

            if (HasSlotVariableIn == false
                && ite_.ConnectionType == SlotType.VarIn)
            {
                throw new InvalidOperationException("This type of node can not have IN variable.");
            }

            if (HasSlotVariableOut == false
                && ite_.ConnectionType == SlotType.VarOut)
            {
                throw new InvalidOperationException("This type of node can not have OUT variable.");
            }

            _Slots.Add(ite_);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_"></param>
        public void RemoveSlotById(int id_)
        {
            foreach (NodeSlot s in _Slots)
            {
                if (s.ID == id_)
                {
                    _Slots.Remove(s);
                    OnPropertyChanged("Slots");
                    break;
                }
            }
        }

        #endregion

        #region Helper

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index_"></param>
        /// <returns></returns>
        public bool IsConnectorIn(int index_)
        {
            return index_ < (SlotConnectorIn == null ? 0 : 1);
        }

        #endregion

        #region Save

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seqNodeNode_"></param>
        public virtual void Save(XmlNode seqNodeNode_)
        {
            const int version = 1;
            seqNodeNode_.AddAttribute("version", version.ToString());

            seqNodeNode_.AddAttribute("comment", Comment);
            seqNodeNode_.AddAttribute("id", Id.ToString());

            string typeName = GetType().AssemblyQualifiedName;
            int index = typeName.IndexOf(',', typeName.IndexOf(',') + 1);
            typeName = typeName.Substring(0, index);
            seqNodeNode_.AddAttribute("type", typeName);

            //Save slots
            foreach (NodeSlot slot in _Slots)
            {
                XmlNode nodeSlot = seqNodeNode_.OwnerDocument.CreateElement("Slot");
                seqNodeNode_.AppendChild(nodeSlot);
                slot.Save(nodeSlot);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionListNode_"></param>
        public void SaveConnections(XmlNode connectionListNode_)
        {
            const int versionConnection = 1;
            foreach (NodeSlot slot in _Slots)
            {
                foreach (NodeSlot otherSlot in slot.ConnectedNodes)
                {
                    XmlNode linkNode = connectionListNode_.OwnerDocument.CreateElement("Connection");
                    connectionListNode_.AppendChild(linkNode);

                    linkNode.AddAttribute("version", versionConnection.ToString());

                    linkNode.AddAttribute("srcNodeID", Id.ToString());
                    linkNode.AddAttribute("srcNodeSlotID", slot.ID.ToString());
                    linkNode.AddAttribute("destNodeID", otherSlot.Node.Id.ToString());
                    linkNode.AddAttribute("destNodeSlotID", otherSlot.ID.ToString());
                }
            }
        }

        #endregion // Save

        #endregion // Methods
    }
}
