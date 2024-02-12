using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
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
        public SequenceNode SeqNode { get; }

        /// <summary>
        /// 
        /// </summary>
        public NodeType SequenceNodeType => SeqNode.NodeType;

        /// <summary>
        /// The name of the node.
        /// </summary>
        private string _name = string.Empty;

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
        private readonly ObservableCollection<ConnectorViewModel> _allConnectors = new();

        private bool _isSelected;

        public NodeViewModel(SequenceNode node)
        {
            SeqNode = node;

            _allConnectors.CollectionChanged += AllConnectorsOnCollectionChanged;

            InitializeConnectors();
        }

        private void AllConnectorsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                allConnectors_ItemsAdded(sender, e.NewItems);
            }
            else if (e.Action is NotifyCollectionChangedAction.Remove or NotifyCollectionChangedAction.Reset)
            {
                allConnectors_ItemsRemoved(sender, e.OldItems);
            }
        }

        public string Title => SeqNode.Title;

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

        public string? Comment
        {
            get => SeqNode.Comment;
            set => SetField(ref SeqNode.Comment, value);
        }

        public string? CustomText
        {
            get => SeqNode.CustomText;
            set => SetField(ref SeqNode.CustomText, value);
        }

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

        public double X
        {
            get => SeqNode.X;
            set => SetField(ref SeqNode.X, value);
        }

        public double Y
        {
            get => SeqNode.Y;
            set => SetField(ref SeqNode.Y, value);
        }

        public int ZIndex
        {
            get => SeqNode.ZIndex;
            set => SetField(ref SeqNode.ZIndex, value);
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
                if (SetField(ref _size, value))
                {
                    SizeChanged?.Invoke(this, EventArgs.Empty);
                }
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
        public ObservableCollection<ConnectorViewModel> Connectors => _allConnectors;

        /// <summary>
        /// List of all input connectors (connections points) attached to the node.
        /// </summary>
        public IEnumerable<ConnectorViewModel> AllInputConnectors =>
            _allConnectors.Where(c => c.Type is ConnectorType.Input or ConnectorType.VariableInput);

        /// <summary>
        /// List of all output connectors (connections points) attached to the node.
        /// </summary>
        public IEnumerable<ConnectorViewModel> AllOutputConnectors =>
            _allConnectors.Where(c => c.Type is ConnectorType.Output or ConnectorType.VariableOutput);

        /// <summary>
        /// List of input connectors (connections points) attached to the node.
        /// </summary>
        public IEnumerable<ConnectorViewModel> InputConnectors => _allConnectors.Where(c => c.Type == ConnectorType.Input);

        /// <summary>
        /// List of input connectors (connections points) attached to the node.
        /// </summary>
        public IEnumerable<ConnectorViewModel> InputVariableConnectors => _allConnectors.Where(c => c.Type == ConnectorType.VariableInput);

        /// <summary>
        /// List of output connectors (connections points) attached to the node.
        /// </summary>
        public IEnumerable<ConnectorViewModel> OutputConnectors => _allConnectors.Where(c => c.Type == ConnectorType.Output);

        /// <summary>
        /// List of output connectors (connections points) attached to the node.
        /// </summary>
        public IEnumerable<ConnectorViewModel> OutputVariableConnectors => _allConnectors.Where(c => c.Type == ConnectorType.VariableOutput);

        /// <summary>
        /// List of output connectors (connections points) attached to the node.
        /// </summary>
        public IEnumerable<ConnectorViewModel> InOutVariableConnectors => _allConnectors.Where(c => c.Type == ConnectorType.VariableInputOutput);

        /// <summary>
        /// A helper property that retrieves a list (a new list each time) of all connections attached to the node. 
        /// </summary>
        public ICollection<ConnectionViewModel> AttachedConnections
        {
            get
            {
                var attachedConnections = new List<ConnectionViewModel>();

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

        public bool IsSelected
        {
            get => _isSelected;
            set => SetField(ref _isSelected, value);
        }

        public ConnectorViewModel? GetConnectorFromSlotId(int id)
        {
            return _allConnectors.FirstOrDefault(c => c.SourceSlot.Id == id);
        }

        private void InitializeConnectors()
        {
            //if we need to add a new slot
            foreach (var slot in SeqNode.Slots)
            {
                if (ContainsConnectorFromNodeSlots(slot) == false)
                {
                    _allConnectors.Add(new(slot));
                }
            }

            //if we need to remove a slot
            var connectorToRemove = new List<ConnectorViewModel>();
            foreach (var c in _allConnectors)
            {
                var contains = SeqNode.Slots.Any(slot => slot.Id == c.SourceSlot.Id);

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

        private bool ContainsConnectorFromNodeSlots(NodeSlot slot)
        {
            return _allConnectors.Any(c => c.SourceSlot.Id == slot.Id);
        }

        private void allConnectors_ItemsAdded(object sender, IList newItems)
        {
            foreach (ConnectorViewModel connector in newItems)
            {
                connector.ParentNode = this;
            }
        }

        private void allConnectors_ItemsRemoved(object sender, IList newItems)
        {
            foreach (ConnectorViewModel connector in newItems)
            {
                connector.ParentNode = null;
            }
        }

        public NodeViewModel Copy(bool copyConnections = false)
        {
            var node = new NodeViewModel(SeqNode.Copy())
            {
                _name = _name,
                X = X,
                Y = Y,
                ZIndex = ZIndex,
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
