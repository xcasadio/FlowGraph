using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using FlowGraphBase.Node;
using System.ComponentModel;
using FlowGraphBase.Process;
using FlowGraphBase.Logger;

namespace FlowGraphBase
{
    /// <summary>
    /// Manage a sequence of nodes.
    /// </summary>
    public class SequenceBase : INotifyPropertyChanged
    {
        #region Fields

        static int m_NewID = 0;
        
        protected Dictionary<int, SequenceNode> m_SequenceNodes = new Dictionary<int, SequenceNode>();

        private string m_Name, m_Description;

        #endregion // Fields

        #region Properties

        /// <summary>
        /// Gets/Sets
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set 
            {
                if (string.Equals(m_Name, value) == false)
                {
                    m_Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        /// <summary>
        /// Gets/Sets
        /// </summary>
        public string Description
        {
            get { return m_Description; }
            set 
            {
                if (string.Equals(m_Description, value) == false)
                {
                    m_Description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        /// <summary>
        /// Gets
        /// </summary>
        public int ID
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets
        /// </summary>
        public IEnumerable<SequenceNode> Nodes
        {
            get { return m_SequenceNodes.Values.ToArray(); }
        }

        #endregion // Properties

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name_"></param>
        protected SequenceBase(string name_)
        {
            Name = name_;
            ID = m_NewID++;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        protected SequenceBase(XmlNode node_)
        {
            Load(node_);
        }

        #endregion // Constructors

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_"></param>
        /// <returns></returns>
        public SequenceNode GetNodeByID(int id_)
        {
            return m_SequenceNodes[id_];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public void AddNode(SequenceNode node)
        {
            m_SequenceNodes.Add(node.ID, node);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public void RemoveNode(SequenceNode node)
        {
            m_SequenceNodes.Remove(node.ID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public void ClearAllNodes()
        {
            m_SequenceNodes.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStack_"></param>
        public void AllocateAllVariables(MemoryStack memoryStack_)
        {
            foreach (var pair in m_SequenceNodes)
            {
                if (pair.Value is VariableNode)
                {
                    VariableNode varNode = pair.Value as VariableNode;
                    varNode.Allocate(memoryStack_);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetNodes()
        {
            foreach (var pair in m_SequenceNodes)
            {
                pair.Value.Reset();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context_"></param>
        /// <param name="type_"></param>
        /// <param name="index_"></param>
        /// <param name="param_"></param>
        public void OnEvent(ProcessingContext context_, Type type_, int index_, object param_)
        {
            //m_MustStop = false;

            foreach (var pair in m_SequenceNodes)
            {
                if (pair.Value is EventNode
                    && pair.Value.GetType().Equals(type_) == true)
                {
                    EventNode eventNode = pair.Value as EventNode;
                    eventNode.Triggered(context_, index_, param_);
                }
            }
        }

        #region Persistence

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public virtual void Load(XmlNode node_)
        {
            ID = int.Parse(node_.Attributes["id"].Value);
            if (m_NewID <= ID) m_NewID = ID + 1;
            Name = node_.Attributes["name"].Value;
            Description = node_.Attributes["description"].Value;

            foreach (XmlNode nodeNode in node_.SelectNodes("NodeList/Node"))
            {
                int versionNode = int.Parse(nodeNode.Attributes["version"].Value);

                SequenceNode seqNode = SequenceNode.CreateNodeFromXml(nodeNode);

                if (seqNode != null)
                {
                    AddNode(seqNode);
                }
                else
                {
                    throw new InvalidOperationException("Can't create SequenceNode from xml " + string.Format("id={0}", nodeNode.Attributes["id"].Value));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        internal void ResolveNodesLinks(XmlNode node_)
        {
            XmlNode connectionListNode = node_.SelectSingleNode("ConnectionList");

            foreach (var node in m_SequenceNodes)
            {
                node.Value.ResolveLinks(connectionListNode, this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public virtual void Save(XmlNode node_)
        {
            const int version = 1;

            XmlNode graphNode = node_.OwnerDocument.CreateElement("Graph");
            node_.AppendChild(graphNode);

            graphNode.AddAttribute("version", version.ToString());
            graphNode.AddAttribute("id", ID.ToString());
            graphNode.AddAttribute("name", Name);
            graphNode.AddAttribute("description", Description);

            //save all nodes
            XmlNode nodeList = node_.OwnerDocument.CreateElement("NodeList");
            graphNode.AppendChild(nodeList);
            //save all connections
            XmlNode connectionList = node_.OwnerDocument.CreateElement("ConnectionList");
            graphNode.AppendChild(connectionList);

            foreach (var pair in m_SequenceNodes)
            {
                XmlNode nodeNode = node_.OwnerDocument.CreateElement("Node");
                nodeList.AppendChild(nodeNode);
                pair.Value.Save(nodeNode);
                pair.Value.SaveConnections(connectionList);
            }
        }

        #endregion // Persistence

        #region IPropertyNotify

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion // IPropertyNotify

        #endregion // Methods
    }
}
