using Newtonsoft.Json.Linq;
using FlowGraph.Nodes.Actions;
using FlowGraph.Nodes.Events;

namespace FlowGraph;

public class SequenceFunction : SequenceBase
{
    private readonly List<SequenceFunctionSlot> _slots = new();
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

    public SequenceFunction(string? name = null)
        : base(name)
    {
        AddNode(new OnEnterFunctionEvent(this));
        AddNode(new ReturnNode(this));
    }

    public SequenceFunction(SequenceFunction sequenceFunction)
        : base(sequenceFunction)
    {
        AddNode(new OnEnterFunctionEvent(this));
        AddNode(new ReturnNode(this));
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
    }

    public void RemoveSlotById(int id)
    {
        foreach (var slot in _slots)
        {
            if (slot.Id == id)
            {
                _slots.Remove(slot);
                break;
            }
        }
    }

    public override void Load(JObject node)
    {
        base.Load(node);

        foreach (var slotNode in node["slots"])
        {
            var id = slotNode["id"].Value<int>();
            var type = (FunctionSlotType)Enum.Parse(typeof(FunctionSlotType), slotNode["type"].Value<string>());

            if (_nextSlotId <= id)
            {
                _nextSlotId = id + 1;
            }

            var slot = new SequenceFunctionSlot(id, type)
            {
                Name = slotNode["name"].Value<string>(),
                IsArray = slotNode["isArray"].Value<bool>(),
                VariableType = Type.GetType(slotNode["varType"].Value<string>())
            };

            _slots.Add(slot);
        }
    }

    public override void Save(JObject node)
    {
        base.Save(node);
        throw new NotImplementedException("");

        /*
        var graphNode = node.SelectSingleNode("Graph[@id='" + Id + "']");
        graphNode["type", "Function");

        XmlNode slotListNode = node.OwnerDocument.CreateElement("SlotList");
        graphNode.AppendChild(slotListNode);

        //save slots
        foreach (var s in _slots)
        {
            XmlNode slotNode = node.OwnerDocument.CreateElement("Slot");
            slotListNode.AppendChild(slotNode);

            slotNode["type", Enum.GetName(typeof(FunctionSlotType), s.SlotType));
            slotNode["varType", s.VariableType.FullName);
            slotNode["isArray", s.IsArray.ToString());
            slotNode["name", s.Name);
            slotNode["id", s.Id.ToString());
        }*/
    }

    public SequenceFunction Copy()
    {
        return new SequenceFunction(this);
    }
}