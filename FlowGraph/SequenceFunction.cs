using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Xml;
using FlowGraph.Nodes.Actions;
using FlowGraph.Nodes.Events;

namespace FlowGraph;

public class SequenceFunction : SequenceBase
{
    public const string? XmlAttributeTypeValue = "Function";

    public event EventHandler<FunctionSlotChangedEventArg> FunctionSlotChanged;

    private readonly ObservableCollection<SequenceFunctionSlot> _slots = new();
    private int _nextSlotId;

    public IEnumerable<SequenceFunctionSlot> Inputs
    {
        get
        {
            foreach (var s in _slots)
            {
                if (s.SlotType == FunctionSlotType.Input)
                {
                    yield return s;
                }
            }
        }
    }

    public IEnumerable<SequenceFunctionSlot> Outputs
    {
        get
        {
            foreach (var s in _slots)
            {
                if (s.SlotType == FunctionSlotType.Output)
                {
                    yield return s;
                }
            }
        }
    }

    public SequenceFunction(string? name)
        : base(name)
    {
        AddNode(new OnEnterFunctionEvent(this));
        AddNode(new ReturnNode(this));

        _slots.CollectionChanged += OnSlotCollectionChanged;
    }

    public SequenceFunction(XmlNode node)
        : base(node)
    {
        _slots.CollectionChanged += OnSlotCollectionChanged;
    }

    public void AddInput(string name)
    {
        AddSlot(new SequenceFunctionSlot(++_nextSlotId, FunctionSlotType.Input) { Name = name });
    }

    public void AddOutput(string name)
    {
        AddSlot(new SequenceFunctionSlot(++_nextSlotId, FunctionSlotType.Output) { Name = name });
    }

    private void AddSlot(SequenceFunctionSlot slot)
    {
        slot.IsArray = false;
        slot.VariableType = typeof(int);

        _slots.Add(slot);

        FunctionSlotChanged?.Invoke(this, new FunctionSlotChangedEventArg(FunctionSlotChangedType.Added, slot));
    }

    public void RemoveSlotById(int id)
    {
        foreach (var slot in _slots)
        {
            if (slot.Id == id)
            {
                _slots.Remove(slot);

                FunctionSlotChanged?.Invoke(this, new FunctionSlotChangedEventArg(FunctionSlotChangedType.Removed, slot));

                break;
            }
        }
    }

    private void OnSlotCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged("Inputs");
        OnPropertyChanged("Outputs");
    }

    public override void Load(XmlNode node)
    {
        base.Load(node);

        foreach (XmlNode slotNode in node.SelectNodes("SlotList/Slot"))
        {
            var id = int.Parse(slotNode.Attributes["id"].Value);
            var type = (FunctionSlotType)Enum.Parse(typeof(FunctionSlotType), slotNode.Attributes["type"].Value);

            if (_nextSlotId <= id) _nextSlotId = id + 1;

            var slot = new SequenceFunctionSlot(id, type)
            {
                Name = slotNode.Attributes["name"].Value,
                IsArray = bool.Parse(slotNode.Attributes["isArray"].Value),
                VariableType = Type.GetType(slotNode.Attributes["varType"].Value)
            };

            _slots.Add(slot);
        }
    }

    public override void Save(XmlNode node)
    {
        base.Save(node);

        var graphNode = node.SelectSingleNode("Graph[@id='" + Id + "']");
        graphNode.AddAttribute("type", XmlAttributeTypeValue);

        XmlNode slotListNode = node.OwnerDocument.CreateElement("SlotList");
        graphNode.AppendChild(slotListNode);

        //save slots
        foreach (var s in _slots)
        {
            XmlNode slotNode = node.OwnerDocument.CreateElement("Slot");
            slotListNode.AppendChild(slotNode);

            slotNode.AddAttribute("type", Enum.GetName(typeof(FunctionSlotType), s.SlotType));
            slotNode.AddAttribute("varType", s.VariableType.FullName);
            slotNode.AddAttribute("isArray", s.IsArray.ToString());
            slotNode.AddAttribute("name", s.Name);
            slotNode.AddAttribute("id", s.Id.ToString());
        }
    }
}