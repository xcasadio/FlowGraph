using System.Collections.Generic;
using FlowSimulator.UI;
using NetworkModel;
using NetworkUI;

namespace FlowSimulator.Undo
{
    /// <summary>
    /// 
    /// </summary>
    class CreateNodeUndoCommand : IUndoCommand
    {
		#region Fields

        FlowGraphControlViewModel m_FlowGraphVM;
        NodeViewModel m_NodeVM;

		#endregion //Fields
	
		#region Properties
		
		#endregion //Properties
	
		#region Constructors

        /// <summary>
        /// 
        /// </summary>
        public CreateNodeUndoCommand(FlowGraphControlViewModel fgvm_, NodeViewModel nodeVM_)
        {
            m_FlowGraphVM = fgvm_;
            m_NodeVM = nodeVM_;
        }

		#endregion //Constructors
	
		#region Methods

        /// <summary>
        /// 
        /// </summary>
        public void Redo()
        {
            m_FlowGraphVM.AddNode(m_NodeVM);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Undo()
        {
            m_FlowGraphVM.DeleteNode(m_NodeVM);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} : Create node {1}",
                m_FlowGraphVM.Sequence.Name, m_NodeVM.Title);
        }

		#endregion //Methods
    }

    /// <summary>
    /// 
    /// </summary>
    class CreateNodesUndoCommand : IUndoCommand
    {
        #region Fields

        FlowGraphControlViewModel m_FlowGraphVM;
        IEnumerable<NodeViewModel> m_NodesVM;
        List<ConnectionInfo> m_ConnectionInfoList = new List<ConnectionInfo>();

        #endregion //Fields

        #region Properties

        #endregion //Properties

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public CreateNodesUndoCommand(FlowGraphControlViewModel fgvm_, IEnumerable<NodeViewModel> nodesVM_)
        {
            m_FlowGraphVM = fgvm_;

            List<ConnectionViewModel> connections = new List<ConnectionViewModel>();
            foreach (var node in nodesVM_)
            {
                connections.AddRange(node.AttachedConnections);
            }
            CopyConnections(connections);

            m_NodesVM = nodesVM_;
        }

        #endregion //Constructors

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connections_"></param>
        private void CopyConnections(IEnumerable<ConnectionViewModel> connections_)
        {
            foreach (ConnectionViewModel c in connections_)
            {
                m_ConnectionInfoList.Add(new ConnectionInfo
                {
                    ConnectionVM = null,
                    DestConnector = c.DestConnector,
                    DestConnectorHotspot = c.DestConnectorHotspot,
                    Points = c.Points,
                    SourceConnector = c.SourceConnector,
                    SourceConnectorHotspot = c.SourceConnectorHotspot
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Redo()
        {
            m_FlowGraphVM.Network.Nodes.AddRange(m_NodesVM);

            List<ConnectionViewModel> connList = new List<ConnectionViewModel>();

            for (int i = 0; i < m_ConnectionInfoList.Count; i++)
            {
                connList.Add(m_ConnectionInfoList[i].ConnectionVM);
            }

            m_FlowGraphVM.AddConnections(connList);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Undo()
        {
            List<ConnectionViewModel> connList = new List<ConnectionViewModel>();

            for (int i = 0; i < m_ConnectionInfoList.Count; i++)
            {
                ConnectionViewModel copy = new ConnectionViewModel();
                copy.DestConnector = m_ConnectionInfoList[i].DestConnector;
                copy.DestConnectorHotspot = m_ConnectionInfoList[i].DestConnectorHotspot;
                copy.Points = m_ConnectionInfoList[i].Points;
                copy.SourceConnector = m_ConnectionInfoList[i].SourceConnector;
                copy.SourceConnectorHotspot = m_ConnectionInfoList[i].SourceConnectorHotspot;

                connList.Add(copy);

                ConnectionInfo inf = m_ConnectionInfoList[i];
                inf.ConnectionVM = copy;
                m_ConnectionInfoList[i] = inf;
            }

            m_FlowGraphVM.DeleteConnections(connList);
            m_FlowGraphVM.DeleteNodes(m_NodesVM);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Graph[{0}] : Create nodes",
                m_FlowGraphVM.Sequence.Name);
        }

        #endregion //Methods
    }

    /// <summary>
    /// 
    /// </summary>
    class DeleteNodeUndoCommand : IUndoCommand
    {
        #region Fields

        FlowGraphControlViewModel m_FlowGraphVM;
        NodeViewModel m_NodeVM;
        List<ConnectionInfo> m_ConnectionInfoList = new List<ConnectionInfo>();

        #endregion //Fields

        #region Properties

        #endregion //Properties

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public DeleteNodeUndoCommand(FlowGraphControlViewModel fgvm_, NodeViewModel nodeVM_)
        {
            m_FlowGraphVM = fgvm_;
            m_NodeVM = nodeVM_;
            CopyConnections(m_NodeVM.AttachedConnections);
        }

        #endregion //Constructors

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connections_"></param>
        private void CopyConnections(IEnumerable<ConnectionViewModel> connections_)
        {
            foreach (ConnectionViewModel c in connections_)
            {
                m_ConnectionInfoList.Add(new ConnectionInfo
                {
                    ConnectionVM = null,
                    DestConnector = c.DestConnector,
                    DestConnectorHotspot = c.DestConnectorHotspot,
                    Points = c.Points,
                    SourceConnector = c.SourceConnector,
                    SourceConnectorHotspot = c.SourceConnectorHotspot
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Redo()
        {
            List<ConnectionViewModel> connList = new List<ConnectionViewModel>();

            for (int i = 0; i < m_ConnectionInfoList.Count; i++)
            {
                connList.Add(m_ConnectionInfoList[i].ConnectionVM);
            }

            m_FlowGraphVM.DeleteConnections(connList);
            m_FlowGraphVM.DeleteNode(m_NodeVM);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Undo()
        {
            List<ConnectionViewModel> connList = new List<ConnectionViewModel>();

            for (int i = 0; i < m_ConnectionInfoList.Count; i++)
            {
                ConnectionViewModel copy = new ConnectionViewModel();
                copy.DestConnector = m_ConnectionInfoList[i].DestConnector;
                copy.DestConnectorHotspot = m_ConnectionInfoList[i].DestConnectorHotspot;
                copy.Points = m_ConnectionInfoList[i].Points;
                copy.SourceConnector = m_ConnectionInfoList[i].SourceConnector;
                copy.SourceConnectorHotspot = m_ConnectionInfoList[i].SourceConnectorHotspot;

                connList.Add(copy);

                ConnectionInfo inf = m_ConnectionInfoList[i];
                inf.ConnectionVM = copy;
                m_ConnectionInfoList[i] = inf;
            }

            m_FlowGraphVM.AddNode(m_NodeVM);
            m_FlowGraphVM.AddConnections(connList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} : Delete node {1}",
                m_FlowGraphVM.Sequence.Name, m_NodeVM.Title);
        }

        #endregion //Methods
    }

    /// <summary>
    /// 
    /// </summary>
    class DeleteNodesUndoCommand : IUndoCommand
    {
        #region Fields

        FlowGraphControlViewModel m_FlowGraphVM;
        IEnumerable<NodeViewModel> m_NodesVM;
        List<ConnectionInfo> m_ConnectionInfoList = new List<ConnectionInfo>();

        #endregion //Fields

        #region Properties

        #endregion //Properties

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public DeleteNodesUndoCommand(FlowGraphControlViewModel fgvm_, IEnumerable<NodeViewModel> nodesVM_)
        {
            m_FlowGraphVM = fgvm_;

            List<ConnectionViewModel> connections = new List<ConnectionViewModel>();
            foreach (var node in nodesVM_)
            {
                connections.AddRange(node.AttachedConnections);
            }
            CopyConnections(connections);

            m_NodesVM = nodesVM_;
        }

        #endregion //Constructors

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connections_"></param>
        private void CopyConnections(IEnumerable<ConnectionViewModel> connections_)
        {
            foreach (ConnectionViewModel c in connections_)
            {
                m_ConnectionInfoList.Add(new ConnectionInfo
                {
                    ConnectionVM = null,
                    DestConnector = c.DestConnector,
                    DestConnectorHotspot = c.DestConnectorHotspot,
                    Points = c.Points,
                    SourceConnector = c.SourceConnector,
                    SourceConnectorHotspot = c.SourceConnectorHotspot
                });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Redo()
        {
            List<ConnectionViewModel> connList = new List<ConnectionViewModel>();

            for (int i = 0; i < m_ConnectionInfoList.Count; i++)
            {
                connList.Add(m_ConnectionInfoList[i].ConnectionVM);
            }

            m_FlowGraphVM.DeleteConnections(connList);
            m_FlowGraphVM.Network.Nodes.RemoveRange(m_NodesVM);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Undo()
        {
            List<ConnectionViewModel> connList = new List<ConnectionViewModel>();

            for (int i = 0; i < m_ConnectionInfoList.Count; i++)
            {
                ConnectionViewModel copy = new ConnectionViewModel();
                copy.DestConnector = m_ConnectionInfoList[i].DestConnector;
                copy.DestConnectorHotspot = m_ConnectionInfoList[i].DestConnectorHotspot;
                copy.Points = m_ConnectionInfoList[i].Points;
                copy.SourceConnector = m_ConnectionInfoList[i].SourceConnector;
                copy.SourceConnectorHotspot = m_ConnectionInfoList[i].SourceConnectorHotspot;

                connList.Add(copy);

                ConnectionInfo inf = m_ConnectionInfoList[i];
                inf.ConnectionVM = copy;
                m_ConnectionInfoList[i] = inf;
            }

            m_FlowGraphVM.Network.Nodes.AddRange(m_NodesVM);
            m_FlowGraphVM.AddConnections(connList);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Graph[{0}] : Delete nodes",
                m_FlowGraphVM.Sequence.Name);
        }

        #endregion //Methods
    }

    /// <summary>
    /// 
    /// </summary>
    class PositionNodeUndoCommand : IUndoCommand
    {
        #region Nested class

        internal class NodeDraggingInfo
        {
            public NodeViewModel Node;
            public double StartX, StartY;
            public double EndX, EndY;
        }

        #endregion

        #region Fields

        FlowGraphControlViewModel m_FlowGraphVM;
        IEnumerable<NodeDraggingInfo> m_NodeInfosVM;

        #endregion //Fields

        #region Properties

        #endregion //Properties

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public PositionNodeUndoCommand(FlowGraphControlViewModel fgvm_, IEnumerable<NodeDraggingInfo> nodeInfosVM_)
        {
            m_FlowGraphVM = fgvm_;
            m_NodeInfosVM = new List<NodeDraggingInfo>(nodeInfosVM_);
        }

        #endregion //Constructors

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public void Redo()
        {
            foreach (NodeDraggingInfo info in m_NodeInfosVM)
            {
                info.Node.X = info.EndX;
                info.Node.Y = info.EndY;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Undo()
        {
            foreach (NodeDraggingInfo info in m_NodeInfosVM)
            {
                info.Node.X = info.StartX;
                info.Node.Y = info.StartY;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} : Node position changed",
                m_FlowGraphVM.Sequence.Name);
        }

        #endregion //Methods
    }

    /// <summary>
    /// 
    /// </summary>
    class SelectNodesUndoCommand : IUndoCommand
    {
        #region Fields

        NetworkView m_View;
        IEnumerable<NodeViewModel> m_NodesVM;

        #endregion //Fields

        #region Properties

        #endregion //Properties

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public SelectNodesUndoCommand(NetworkView view_, IEnumerable<NodeViewModel> nodesVM_)
        {
            m_View = view_;
            m_NodesVM = new List<NodeViewModel>(nodesVM_);
        }

        #endregion //Constructors

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public void Redo()
        {
            m_View.IsUndoRegisterEnabled = false;

            foreach (var node in m_NodesVM)
            {
                m_View.SelectedNodes.Add(node);
            }

            m_View.IsUndoRegisterEnabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Undo()
        {
            m_View.IsUndoRegisterEnabled = false;

            foreach (var node in m_NodesVM)
            {
                m_View.SelectedNodes.Remove(node);
            }

            m_View.IsUndoRegisterEnabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Node selected");
        }

        #endregion //Methods
    }

    /// <summary>
    /// 
    /// </summary>
    class DeselectNodesUndoCommand : IUndoCommand
    {
        #region Fields

        NetworkView m_View;
        IEnumerable<NodeViewModel> m_NodesVM;

        #endregion //Fields

        #region Properties

        #endregion //Properties

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public DeselectNodesUndoCommand(NetworkView view_, IEnumerable<NodeViewModel> nodesVM_)
        {
            m_View = view_;
            m_NodesVM = new List<NodeViewModel>(nodesVM_);
        }

        #endregion //Constructors

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public void Redo()
        {
            m_View.IsUndoRegisterEnabled = false;

            foreach (var node in m_NodesVM)
            {
                m_View.SelectedNodes.Remove(node);
            }

            m_View.IsUndoRegisterEnabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Undo()
        {
            m_View.IsUndoRegisterEnabled = false;

            foreach (var node in m_NodesVM)
            {
                m_View.SelectedNodes.Add(node);
            }
 
             m_View.IsUndoRegisterEnabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Node deselected");
        }

        #endregion //Methods
    }
}
