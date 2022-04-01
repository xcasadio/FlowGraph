using System;
using System.ComponentModel;
using System.Linq;
using System.Xml;
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
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public int Id
        {
            get;
            private set;
        }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        public void ActivateOutputLink(ProcessingContext context, int id)
        {
            GetSlotById(id).RegisterNodes(context);
        }

        /// <summary>
        /// 
        /// </summary>
        public NodeSlot GetSlotById(int id)
        {
            foreach (NodeSlot slot in _nodeSlots)
            {
                if (slot.Id == id)
                {
                    return slot;
                }
            }

            return null;

        }

        /// <summary>
        /// Gets the value of the first node connected to the slot with the Id id_
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object GetValueFromSlot(int id)
        {
            NodeSlot slot = GetSlotById(id);

            if (slot.ConnectedNodes.Count > 0)
            {
                NodeSlot dstSlot = slot.ConnectedNodes[0];
                SequenceNode node = slot.ConnectedNodes[0].Node;

                // Connected directly to a NodeSlot value (VarOut) ?
                if (dstSlot is NodeSlotVar @var)
                {
                    return var.Value;
                }

                if (node is VariableNode variableNode)
                {
                    return variableNode.Value;
                }
                throw new InvalidOperationException(
                    $"Node({Id}) GetValueFromSlot({id}) : type of link not supported");
            }
            // if no node is connected, we take the nested value of the slot

            if (slot is NodeSlotVar slotVar)
            {
                return slotVar.Value;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public void SetValueInSlot(int id, object value)
        {
            NodeSlot slot = GetSlotById(id);

            if (slot.ConnectedNodes.Count > 0)
            {
                foreach (NodeSlot other in slot.ConnectedNodes)
                {
                    if (other.Node is VariableNode node)
                    {
                        node.Value = value;
                    }
                }
            }
            else if (slot is NodeSlotVar @var)
            {
                var.Value = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        protected virtual void Load(XmlNode node)
        {
            Id = int.Parse(node.Attributes["id"].Value);
            if (_freeId <= Id) _freeId = Id + 1;
            Comment = node.Attributes["comment"].Value; // EDITOR

            foreach (NodeSlot slot in _nodeSlots)
            {
                XmlNode nodeSlot = node.SelectSingleNode("Slot[@index='" + slot.Id + "']");
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
        /// <param name="connectionListNode"></param>
        internal virtual void ResolveLinks(XmlNode connectionListNode, SequenceBase sequence)
        {
            foreach (XmlNode connNode in connectionListNode.SelectNodes("Connection[@srcNodeID='" + Id + "']"))
            {
                int outputSlotId = int.Parse(connNode.Attributes["srcNodeSlotID"].Value);
                int destNodeId = int.Parse(connNode.Attributes["destNodeID"].Value);
                int destNodeInputId = int.Parse(connNode.Attributes["destNodeSlotID"].Value);

                SequenceNode destNode = sequence.GetNodeById(destNodeId);
                GetSlotById(outputSlotId).ConnectTo(destNode.GetSlotById(destNodeInputId));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static SequenceNode CreateNodeFromXml(XmlNode node)
        {
            string typeVal = node.Attributes["type"].Value;

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
                       .SelectMany(t => t.GetTypes()).Single(t => t.IsClass
                                                                  && t.IsGenericType == false
                                                                  && t.IsInterface == false
                                                                  && t.IsAbstract == false
                                                                  && t.IsSubclassOf(typeof(SequenceNode))
                                                                  && t.AssemblyQualifiedName
                                                                      .Substring(0, t.AssemblyQualifiedName
                                                                          .IndexOf(',', t.AssemblyQualifiedName
                                                                              .IndexOf(',') + 1))
                                                                      .Equals(typeVal));

                return (SequenceNode)Activator.CreateInstance(type, node);
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteException(ex);
            }

            return null;
        }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Event raised to indicate that a property value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        public void RemoveAllConnections()
        {
            foreach (var slot in _nodeSlots)
            {
                slot.RemoveAllConnections();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected abstract void InitializeSlots();

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
    }
}
