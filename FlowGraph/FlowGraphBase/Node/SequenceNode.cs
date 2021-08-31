using System;
using System.Linq;
using System.Xml;
using System.ComponentModel;
using FlowGraphBase.Logger;
using FlowGraphBase.Process;

namespace FlowGraphBase.Node
{
    /// <summary>
    /// Represents a base node
    /// </summary>
    public abstract
#if EDITOR
    partial
#endif
    class SequenceNode : INotifyPropertyChanged
    {
        #region Fields

        #endregion // Fields

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public int Id
        {
            get;
            private set;
        }

        #endregion // Properties

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected SequenceNode(XmlNode node) :
            this()
        {
            if (node == null)
            {
                throw new ArgumentNullException("SequenceNode() : XmlNode is null");
            }

            Load(node);
        }

        #endregion // Constructor

        #region Methods

        #region Process

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context_"></param>
        /// <param name="id_"></param>
        public void ActivateOutputLink(ProcessingContext context_, int id_)
        {
            GetSlotById(id_).RegisterNodes(context_);
        }

        /// <summary>
        /// 
        /// </summary>
        public NodeSlot GetSlotById(int id_)
        {
            foreach (NodeSlot slot in _Slots)
            {
                if (slot.ID == id_)
                {
                    return slot;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the value of the first node connected to the slot with the Id id_
        /// </summary>
        /// <param name="index_"></param>
        /// <returns></returns>
        public object GetValueFromSlot(int id_)
        {
            NodeSlot slot = GetSlotById(id_);

            if (slot.ConnectedNodes.Count > 0)
            {
                NodeSlot dstSlot = slot.ConnectedNodes[0];
                SequenceNode node = slot.ConnectedNodes[0].Node;

                // Connected directly to a NodeSlot value (VarOut) ?
                if (dstSlot is NodeSlotVar)
                {
                    return ((NodeSlotVar)dstSlot).Value;
                }

                if (node is VariableNode)
                {
                    return ((VariableNode)node).Value;
                }
                throw new InvalidOperationException(
                    string.Format("Node({0}) GetValueFromSlot({1}) : type of link not supported", Id, id_));
            }
            // if no node is connected, we take the nested value of the slot

            if (slot is NodeSlotVar)
            {
                return ((NodeSlotVar)slot).Value;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_"></param>
        /// <param name="value_"></param>
        public void SetValueInSlot(int id_, object value_)
        {
            NodeSlot slot = GetSlotById(id_);

            if (slot.ConnectedNodes.Count > 0)
            {
                foreach (NodeSlot other in slot.ConnectedNodes)
                {
                    if (other.Node is VariableNode)
                    {
                        (other.Node as VariableNode).Value = value_;
                    }
                }
            }
            else if (slot is NodeSlotVar)
            {
                ((NodeSlotVar)slot).Value = value_;
            }
        }

        #endregion // Process

        #region Persistence

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        protected virtual void Load(XmlNode node_)
        {
            Id = int.Parse(node_.Attributes["id"].Value);
            if (_freeId <= Id) _freeId = Id + 1;
            Comment = node_.Attributes["comment"].Value; // EDITOR

            foreach (NodeSlot slot in _Slots)
            {
                XmlNode nodeSlot = node_.SelectSingleNode("Slot[@index='" + slot.ID + "']");
                if (nodeSlot != null)
                {
                    slot.Load(nodeSlot);
                }
            }
        }

        /// <summary>
        /// Call after Load() to connect nodes each others
        /// </summary>
        /// <param name="node_"></param>
        /// <param name="connectionListNode_"></param>
        internal virtual void ResolveLinks(XmlNode connectionListNode_, SequenceBase sequence_)
        {
            foreach (XmlNode connNode in connectionListNode_.SelectNodes("Connection[@srcNodeID='" + Id + "']"))
            {
                int outputSlotID = int.Parse(connNode.Attributes["srcNodeSlotID"].Value);
                int destNodeID = int.Parse(connNode.Attributes["destNodeID"].Value);
                int destNodeInputID = int.Parse(connNode.Attributes["destNodeSlotID"].Value);

                SequenceNode destNode = sequence_.GetNodeById(destNodeID);
                GetSlotById(outputSlotID).ConnectTo(destNode.GetSlotById(destNodeInputID));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        /// <returns></returns>
        public static SequenceNode CreateNodeFromXml(XmlNode node_)
        {
            string typeVal = node_.Attributes["type"].Value;

            try
            {
//                 IEnumerable<Type> classes = AppDomain.CurrentDomain.GetAssemblies()
//                        .SelectMany(t => t.GetTypes())
//                        .Where(t => t.IsClass == true
//                            && t.IsGenericType == false
//                            && t.IsInterface == false
//                            && t.IsAbstract == false
//                            && t.IsSubclassOf(typeof(SequenceNode)));
// 
//                 foreach (Type t in classes)
//                 {
//                     LogManager.Instance.WriteLine(LogVerbosity.Info, "{0}", t.FullName);
//                 }

                Type type = AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany(t => t.GetTypes()).Single<Type>(t =>
                       {
                           return t.IsClass
                              && t.IsGenericType == false
                              && t.IsInterface == false
                              && t.IsAbstract == false
                              && t.IsSubclassOf(typeof(SequenceNode))
                              && t.AssemblyQualifiedName
                                .Substring(0, t.AssemblyQualifiedName
                                    .IndexOf(',', t.AssemblyQualifiedName
                                        .IndexOf(',') + 1))
                                .Equals(typeVal);
                       });

                return (SequenceNode)Activator.CreateInstance(type, node_);
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteException(ex);
            }

            return null;
        }

        #endregion

        #region INotifyPropertyChanged

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Event raised to indicate that a property value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion // INotifyPropertyChanged

        /// <summary>
        /// 
        /// </summary>
        public void RemoveAllConnections()
        {
            foreach (var slot in _Slots)
            {
                slot.RemoveAllConnections();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected abstract void InitializeSlots();

        #region Copy

        /// <summary>
        /// 
        /// </summary>
        protected abstract SequenceNode CopyImpl();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public SequenceNode Copy()
        {
            return CopyImpl();
        }

        #endregion // Copy

        #endregion // Methods
    }
}
