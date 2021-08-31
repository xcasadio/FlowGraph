using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;

namespace FlowGraphBase.Node
{
    /// <summary>
    /// 
    /// </summary>
    partial class SequenceNode
    {
        //static private readonly int UndefinedID = -1;
        private static int _freeId;

        //Used to convert a SequenceNode to a Node (graphic node)
        protected List<NodeSlot> _nodeSlots = new List<NodeSlot>();

        private string _customText;

        private bool _isProcessing;

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
            get => _customText;
            set 
            { 
                _customText = value;
                OnPropertyChanged("CustomText");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public NodeSlot[] Slots => _nodeSlots.ToArray();

        /// <summary>
        /// 
        /// </summary>
        public bool IsProcessing
        {
            get => _isProcessing;
            set 
            {
                _isProcessing = value;
                OnPropertyChanged("IsProcessing");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public NodeSlot SlotConnectorIn
        {
            get
            {
                foreach (var slot in _nodeSlots)
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
                foreach (var slot in _nodeSlots)
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

                foreach (var slot in _nodeSlots)
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
                foreach (var slot in _nodeSlots)
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

                foreach (var slot in _nodeSlots)
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
                foreach (var slot in _nodeSlots)
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

                foreach (var slot in _nodeSlots)
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
                foreach (var slot in _nodeSlots)
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

                foreach (var slot in _nodeSlots)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seqMgr_"></param>
        protected SequenceNode()
        {
            Id = ++_freeId;
            InitializeSlots();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            CustomText = null;
            IsProcessing = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionType_"></param>
        /// <param name="slot_"></param>
        protected void AddFunctionSlot(int slotId, SlotType connectionType, SequenceFunctionSlot slot)
        {
            AddSlot(new NodeFunctionSlot(slotId, this, connectionType, slot));
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
        protected void AddSlot(int slotId, string text, SlotType connectionType, Type type = null,
            bool saveInternalValue = true, VariableControlType controlType = VariableControlType.ReadOnly,
            object tag = null)
        {
            if (connectionType == SlotType.VarIn
                || connectionType == SlotType.VarOut)
            {
                AddSlot(new NodeSlotVar(slotId, this, text, connectionType, type, controlType, tag, saveInternalValue));
            }
            else
            {
                AddSlot(new NodeSlot(slotId, this, text, connectionType, type, controlType, tag));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ite_"></param>
        private void AddSlot(NodeSlot ite)
        {
            foreach (NodeSlot slot in _nodeSlots)
            {
                if (slot.Id == ite.Id)
                {
                    throw new InvalidOperationException("A slot with the Id '" + ite.Id + "' already exists.");
                }
            }

            if (HasSlotConnectorIn == false
                && ite.ConnectionType == SlotType.NodeIn)
            {
                throw new InvalidOperationException("This type of node can not have IN connector.");
            }

            if (HasSlotConnectorOut == false
                && ite.ConnectionType == SlotType.NodeOut)
            {
                throw new InvalidOperationException("This type of node can not have OUT connector.");
            }

            if (HasSlotVariableIn == false
                && ite.ConnectionType == SlotType.VarIn)
            {
                throw new InvalidOperationException("This type of node can not have IN variable.");
            }

            if (HasSlotVariableOut == false
                && ite.ConnectionType == SlotType.VarOut)
            {
                throw new InvalidOperationException("This type of node can not have OUT variable.");
            }

            _nodeSlots.Add(ite);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_"></param>
        public void RemoveSlotById(int id)
        {
            foreach (NodeSlot s in _nodeSlots)
            {
                if (s.Id == id)
                {
                    _nodeSlots.Remove(s);
                    OnPropertyChanged("Slots");
                    break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index_"></param>
        /// <returns></returns>
        public bool IsConnectorIn(int index)
        {
            return index < (SlotConnectorIn == null ? 0 : 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seqNodeNode_"></param>
        public virtual void Save(XmlNode seqNodeNode)
        {
            const int version = 1;
            seqNodeNode.AddAttribute("version", version.ToString());

            seqNodeNode.AddAttribute("comment", Comment);
            seqNodeNode.AddAttribute("id", Id.ToString());

            string typeName = GetType().AssemblyQualifiedName;
            int index = typeName.IndexOf(',', typeName.IndexOf(',') + 1);
            typeName = typeName.Substring(0, index);
            seqNodeNode.AddAttribute("type", typeName);

            //Save slots
            foreach (NodeSlot slot in _nodeSlots)
            {
                XmlNode nodeSlot = seqNodeNode.OwnerDocument.CreateElement("Slot");
                seqNodeNode.AppendChild(nodeSlot);
                slot.Save(nodeSlot);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionListNode_"></param>
        public void SaveConnections(XmlNode connectionListNode)
        {
            const int versionConnection = 1;
            foreach (NodeSlot slot in _nodeSlots)
            {
                foreach (NodeSlot otherSlot in slot.ConnectedNodes)
                {
                    XmlNode linkNode = connectionListNode.OwnerDocument.CreateElement("Connection");
                    connectionListNode.AppendChild(linkNode);

                    linkNode.AddAttribute("version", versionConnection.ToString());

                    linkNode.AddAttribute("srcNodeID", Id.ToString());
                    linkNode.AddAttribute("srcNodeSlotID", slot.Id.ToString());
                    linkNode.AddAttribute("destNodeID", otherSlot.Node.Id.ToString());
                    linkNode.AddAttribute("destNodeSlotID", otherSlot.Id.ToString());
                }
            }
        }
    }
}
