using System.Xml;

namespace FlowGraph.Node;

public class NodeSlotVar : NodeSlot
{
    private readonly ValueContainer _value;
    private readonly bool _saveValue;

    public object? Value
    {
        get => _value.Value;
        set { _value.Value = value; OnPropertyChanged("Value"); }
    }

    public NodeSlotVar(int slotId, SequenceNode node, string? text,
        SlotType connectionType, Type type,
        VariableControlType controlType = VariableControlType.ReadOnly,
        object? tag = null, bool saveValue = true) :
        base(slotId, node, text, connectionType, type, controlType, tag)
    {
        _saveValue = saveValue;

        object? val = null;

        if (type == typeof(bool))
        {
            val = true;
        }
        else if (type == typeof(sbyte)
                 || type == typeof(char)
                 || type == typeof(short)
                 || type == typeof(int)
                 || type == typeof(long)
                 || type == typeof(byte)
                 || type == typeof(ushort)
                 || type == typeof(uint)
                 || type == typeof(ulong)
                 || type == typeof(float)
                 || type == typeof(double))
        {
            val = Convert.ChangeType(0, type);
        }
        else if (type == typeof(string))
        {
            val = string.Empty;
        }

        _value = new ValueContainer(type, val);
    }

    public override void Save(XmlNode node)
    {
        base.Save(node);

        node.AddAttribute("saveValue", _saveValue.ToString());

        if (_saveValue)
        {
            _value.Save(node);
        }
    }

    public override void Load(XmlNode node)
    {
        base.Load(node);

        if (_saveValue)
        {
            _value.Load(node);
        }
    }
}