﻿using System.ComponentModel;
using System.Windows;
using FlowGraph.Nodes;
using Utils;

namespace NetworkModel
{
    /// <summary>
    /// Defines a node in the view-model.
    /// Nodes are connected to other nodes through attached connectors (aka anchor/connection points).
    /// </summary>
    public sealed class NodeViewModel : AbstractModelBase
    {
        /// <summary>
        /// The sequence link with this MVVM
        /// </summary>
        public SequenceNode SeqNode
        {
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        public NodeType SequenceNodeType => SeqNode.NodeType;

        /// <summary>
        /// The name of the node.
        /// </summary>
        private string _name = string.Empty;

        /// <summary>
        /// The X coordinate for the position of the node.
        /// </summary>
        private double _x;

        /// <summary>
        /// The Y coordinate for the position of the node.
        /// </summary>
        private double _y;

        /// <summary>
        /// The Z index of the node.
        /// </summary>
        private int _zIndex;

        /// <summary>
        /// The size of the node.
        /// 
        /// Important Note: 
        ///     The size of a node in the UI is not determined by this property!!
        ///     Instead the size of a node in the UI is determined by the data-template for the Node class.
        ///     When the size is computed via the UI it is then pushed into the view-model
        ///     so that our application code has access to the size of a node.
        /// </summary>
        private Size _size = Size.Empty;

        /// <summary>
        /// List of all connectors (connections points) attached to the node.
        /// </summary>
        private readonly ImpObservableCollection<ConnectorViewModel> _allConnectors = new ImpObservableCollection<ConnectorViewModel>();

        /// <summary>
        /// Set to 'true' when the node is selected.
        /// </summary>
        private bool _isSelected;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public NodeViewModel(SequenceNode node)
        {
            SeqNode = node;
            SeqNode.PropertyChanged += OnSeqNodePropertyChanged;

            _allConnectors.ItemsAdded += allConnectors_ItemsAdded;
            _allConnectors.ItemsRemoved += allConnectors_ItemsRemoved;

            InitializeConnectors();
        }

        /// <summary>
        /// The name of the node.
        /// </summary>
        public string Title => SeqNode.Title;

        /// <summary>
        /// 
        /// </summary>
        public ActionNode.LogicState State
        {
            get
            {
                if (SeqNode is ActionNode node)
                {
                    return node.State.State;
                }

                return ActionNode.LogicState.Ok;
            }
        }

        /// <summary>
        /// The value of the variable node.
        /// </summary>
        public object Value
        {
            get
            {
                if (SeqNode is VariableNode node)
                {
                    return node.Value;
                }

                return null;
            }
            set
            {
                if (SeqNode is VariableNode node)
                {
                    try
                    {
                        node.Value = value;
                    }
                    catch (Exception /*ex*/)
                    {
                        //set error to false
                    }
                }
            }
        }

        /// <summary>
        /// The comments of the node.
        /// </summary>
        public string Comment
        {
            get => SeqNode.Comment;
            set
            {
                if (SeqNode.Comment == value)
                {
                    return;
                }

                SeqNode.Comment = value;
                OnPropertyChanged("Comment");
            }
        }

        /// <summary>
        /// The custom text of the node.
        /// </summary>
        public string CustomText
        {
            get => SeqNode.CustomText;
            set
            {
                if (SeqNode.CustomText == value)
                {
                    return;
                }

                SeqNode.CustomText = value;
                OnPropertyChanged("CustomText");
            }
        }

        /// <summary>
        /// The error message of the node.
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                if (SeqNode is ActionNode node)
                {
                    return node.ErrorMessage;
                }

                return "";
            }
        }

        /// <summary>
        /// The X coordinate for the position of the node.
        /// </summary>
        public double X
        {
            get => _x;
            set
            {
                if (_x == value)
                {
                    return;
                }

                _x = value;

                OnPropertyChanged("X");
            }
        }

        /// <summary>
        /// The Y coordinate for the position of the node.
        /// </summary>
        public double Y
        {
            get => _y;
            set
            {
                if (_y == value)
                {
                    return;
                }

                _y = value;

                OnPropertyChanged("Y");
            }
        }

        /// <summary>
        /// The Z index of the node.
        /// </summary>
        public int ZIndex
        {
            get => _zIndex;
            set
            {
                if (_zIndex == value)
                {
                    return;
                }

                _zIndex = value;

                OnPropertyChanged("ZIndex");
            }
        }

        /// <summary>
        /// The size of the node.
        /// 
        /// Important Note: 
        ///     The size of a node in the UI is not determined by this property!!
        ///     Instead the size of a node in the UI is determined by the data-template for the Node class.
        ///     When the size is computed via the UI it is then pushed into the view-model
        ///     so that our application code has access to the size of a node.
        /// </summary>
        public Size Size
        {
            get => _size;
            set
            {
                if (_size == value)
                {
                    return;
                }

                _size = value;

                SizeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Event raised when the size of the node is changed.
        /// The size will change when the UI has determined its size based on the contents
        /// of the nodes data-template.  It then pushes the size through to the view-model
        /// and this 'SizeChanged' event occurs.
        /// </summary>
        public event EventHandler<EventArgs> SizeChanged;

        /// <summary>
        /// List of all connectors (connections points) attached to the node.
        /// </summary>
        public ImpObservableCollection<ConnectorViewModel> Connectors => _allConnectors;

        /// <summary>
        /// List of all input connectors (connections points) attached to the node.
        /// </summary>
        public IEnumerable<ConnectorViewModel> AllInputConnectors
        {
            get
            {
                foreach (ConnectorViewModel c in _allConnectors)
                {
                    if (c.Type == ConnectorType.Input
                        || c.Type == ConnectorType.VariableInput)
                    {
                        yield return c;
                    }
                }
            }
        }

        /// <summary>
        /// List of all output connectors (connections points) attached to the node.
        /// </summary>
        public IEnumerable<ConnectorViewModel> AllOutputConnectors
        {
            get
            {
                foreach (ConnectorViewModel c in _allConnectors)
                {
                    if (c.Type == ConnectorType.Output
                        || c.Type == ConnectorType.VariableOutput)
                    {
                        yield return c;
                    }
                }
            }
        }

        /// <summary>
        /// List of input connectors (connections points) attached to the node.
        /// </summary>
        public IEnumerable<ConnectorViewModel> InputConnectors
        {
            get
            {
                foreach (ConnectorViewModel c in _allConnectors)
                {
                    if (c.Type == ConnectorType.Input)
                    {
                        yield return c;
                    }
                }
            }
        }

        /// <summary>
        /// List of input connectors (connections points) attached to the node.
        /// </summary>
        public IEnumerable<ConnectorViewModel> InputVariableConnectors
        {
            get
            {
                foreach (ConnectorViewModel c in _allConnectors)
                {
                    if (c.Type == ConnectorType.VariableInput)
                    {
                        yield return c;
                    }
                }
            }
        }

        /// <summary>
        /// List of output connectors (connections points) attached to the node.
        /// </summary>
        public IEnumerable<ConnectorViewModel> OutputConnectors
        {
            get
            {
                foreach (ConnectorViewModel c in _allConnectors)
                {
                    if (c.Type == ConnectorType.Output)
                    {
                        yield return c;
                    }
                }
            }
        }

        /// <summary>
        /// List of output connectors (connections points) attached to the node.
        /// </summary>
        public IEnumerable<ConnectorViewModel> OutputVariableConnectors
        {
            get
            {
                foreach (ConnectorViewModel c in _allConnectors)
                {
                    if (c.Type == ConnectorType.VariableOutput)
                    {
                        yield return c;
                    }
                }
            }
        }

        /// <summary>
        /// List of output connectors (connections points) attached to the node.
        /// </summary>
        public IEnumerable<ConnectorViewModel> InOutVariableConnectors
        {
            get
            {
                foreach (ConnectorViewModel c in _allConnectors)
                {
                    if (c.Type == ConnectorType.VariableInputOutput)
                    {
                        yield return c;
                    }
                }
            }
        }

        /// <summary>
        /// A helper property that retrieves a list (a new list each time) of all connections attached to the node. 
        /// </summary>
        public ICollection<ConnectionViewModel> AttachedConnections
        {
            get
            {
                List<ConnectionViewModel> attachedConnections = new List<ConnectionViewModel>();

                foreach (var connector in InputConnectors)
                {
                    attachedConnections.AddRange(connector.AttachedConnections);
                }

                foreach (var connector in OutputConnectors)
                {
                    attachedConnections.AddRange(connector.AttachedConnections);
                }

                foreach (var connector in InputVariableConnectors)
                {
                    attachedConnections.AddRange(connector.AttachedConnections);
                }

                foreach (var connector in OutputVariableConnectors)
                {
                    attachedConnections.AddRange(connector.AttachedConnections);
                }

                foreach (var connector in InOutVariableConnectors)
                {
                    attachedConnections.AddRange(connector.AttachedConnections);
                }

                return attachedConnections;
            }
        }

        /// <summary>
        /// Set to 'true' when the node is selected.
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value)
                {
                    return;
                }

                _isSelected = value;

                OnPropertyChanged("IsSelected");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ConnectorViewModel GetConnectorFromSlotId(int id)
        {
            foreach (ConnectorViewModel c in _allConnectors)
            {
                if (c.SourceSlot.Id == id)
                {
                    return c;
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeConnectors()
        {
            //if we need to add a new slot
            foreach (NodeSlot slot in SeqNode.Slots)
            {
                if (ContainsConnectorFromNodeSlots(slot) == false)
                {
                    _allConnectors.Add(new ConnectorViewModel(slot));
                }
            }

            //if we need to remove a slot
            List<ConnectorViewModel> connectorToRemove = new List<ConnectorViewModel>();
            foreach (var c in _allConnectors)
            {
                bool contains = false;

                foreach (NodeSlot slot in SeqNode.Slots)
                {
                    if (slot.Id == c.SourceSlot.Id)
                    {
                        contains = true;
                        break;
                    }
                }

                if (contains == false)
                {
                    connectorToRemove.Add(c);
                }
            }

            foreach (var slot in connectorToRemove)
            {
                _allConnectors.Remove(slot);
            }

            OnPropertyChanged("Connectors");
            OnPropertyChanged("AllInputConnectors");
            OnPropertyChanged("AllOutputConnectors");
            OnPropertyChanged("InputConnectors");
            OnPropertyChanged("InputVariableConnectors");
            OnPropertyChanged("OutputConnectors");
            OnPropertyChanged("OutputVariableConnectors");
            OnPropertyChanged("InOutVariableConnectors");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slot_"></param>
        /// <returns></returns>
        private bool ContainsConnectorFromNodeSlots(NodeSlot slot)
        {
            foreach (ConnectorViewModel c in _allConnectors)
            {
                if (c.SourceSlot.Id == slot.Id)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Event raised when connectors are added to the node.
        /// </summary>
        private void allConnectors_ItemsAdded(object sender, CollectionItemsChangedEventArgs e)
        {
            foreach (ConnectorViewModel connector in e.Items)
            {
                connector.ParentNode = this;
            }
        }

        /// <summary>
        /// Event raised when connectors are removed from the node.
        /// </summary>
        private void allConnectors_ItemsRemoved(object sender, CollectionItemsChangedEventArgs e)
        {
            foreach (ConnectorViewModel connector in e.Items)
            {
                connector.ParentNode = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnSeqNodePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Slots":
                    InitializeConnectors();
                    break;
            }

            OnPropertyChanged(e.PropertyName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="copyConnections_"></param>
        /// <returns></returns>
        public NodeViewModel Copy(bool copyConnections = false)
        {
            NodeViewModel node = new NodeViewModel(SeqNode.Copy())
            {
                _name = _name,
                _x = _x,
                _y = _y,
                _zIndex = _zIndex,
                _size = _size,
                _isSelected = _isSelected
            };

            if (copyConnections)
            {
                throw new NotImplementedException("NodeViewModel.Copy()");
            }

            return node;
        }
    }
}
