using Newtonsoft.Json.Linq;
using FlowGraph.Attributes;

namespace FlowGraph.Nodes.Variables;

[Category("Variable")]
public abstract class GenericVariableNode<T> : VariableNode
{
    private T? _value;

    protected VariableControlType ControlType
    {
        get;
        set;
    }

    public override object? Value
    {
        get => _value;
        set
        {
            switch (value)
            {
                case null:
                    _value = default;
                    break;
                case T value1:
                    _value = value1;
                    break;
                case string s:
                    {
                        if (string.IsNullOrWhiteSpace(s) == false)
                        {
                            _value = (T)Convert.ChangeType(s, typeof(T));
                        }

                        break;
                    }
                default:
                    throw new InvalidCastException("GenericVariableNode<T>.Value : object can not be converted to " + typeof(T).Name + ".");
            }
        }
    }

    protected override void InitializeSlots()
    {
        base.InitializeSlots();

        AddSlot(0, string.Empty, SlotType.VarInOut, typeof(T), true, ControlType);
    }

    protected override void Load(JObject node)
    {
        System.Diagnostics.Debugger.Break();
        base.Load(node);
        //_value = (T)LoadValue(node["value"]);
    }

    protected override object LoadValue(JObject node)
    {
        return node.Value<T>();
    }

    protected override void SaveValue(JObject node)
    {
        var jsonObject = new JObject();
        //_value ?? "null"
        node["value"] = jsonObject;
    }
}

[Name("Object")]
public class VariableNodeObject : GenericVariableNode<object>
{
    public override string? Title => "Object";

    protected override void InitializeSlots()
    {
        ControlType = VariableControlType.ReadOnly;
        base.InitializeSlots();
    }

    public override SequenceNode Copy()
    {
        var node = new VariableNodeObject
        {
            Value = Value
        };
        return node;
    }
}

[Name("String")]
public class VariableNodeString : GenericVariableNode<string>
{
    public override string? Title => "String";

    public VariableNodeString()
    {
        Value = string.Empty;
    }

    public sealed override object? Value
    {
        get => base.Value;
        set => base.Value = value;
    }

    protected override void InitializeSlots()
    {
        ControlType = VariableControlType.Text;
        base.InitializeSlots();
    }

    public override SequenceNode Copy()
    {
        var node = new VariableNodeString
        {
            Value = Value
        };
        return node;
    }
}

[Name("Boolean")]
public class VariableNodeBool : GenericVariableNode<bool>
{
    public override string? Title => "Boolean";

    protected override void InitializeSlots()
    {
        ControlType = VariableControlType.Checkable;
        base.InitializeSlots();
    }

    public override SequenceNode Copy()
    {
        var node = new VariableNodeBool
        {
            Value = Value
        };
        return node;
    }
}

[Name("Byte")]
public class VariableNodeByte : GenericVariableNode<sbyte>
{
    public override string? Title => "Byte";

    protected override void InitializeSlots()
    {
        ControlType = VariableControlType.Numeric;
        base.InitializeSlots();
    }

    public override SequenceNode Copy()
    {
        var node = new VariableNodeByte
        {
            Value = Value
        };
        return node;
    }
}

[Name("Char")]
public class VariableNodeChar : GenericVariableNode<char>
{
    public override string? Title => "Char";

    protected override void InitializeSlots()
    {
        ControlType = VariableControlType.Numeric;
        base.InitializeSlots();
    }

    public override SequenceNode Copy()
    {
        var node = new VariableNodeChar
        {
            Value = Value
        };
        return node;
    }
}

[Name("Short")]
public class VariableNodeShort : GenericVariableNode<short>
{
    public override string? Title => "Short";

    protected override void InitializeSlots()
    {
        ControlType = VariableControlType.Numeric;
        base.InitializeSlots();
    }

    public override SequenceNode Copy()
    {
        var node = new VariableNodeShort
        {
            Value = Value
        };
        return node;
    }
}

[Name("Integer")]
public class VariableNodeInt : GenericVariableNode<int>
{
    public override string? Title => "Integer";

    protected override void InitializeSlots()
    {
        ControlType = VariableControlType.Numeric;
        base.InitializeSlots();
    }

    public override SequenceNode Copy()
    {
        var node = new VariableNodeInt
        {
            Value = Value
        };
        return node;
    }
}

[Name("Long")]
public class VariableNodeLong : GenericVariableNode<long>
{
    public override string? Title => "Long";

    protected override void InitializeSlots()
    {
        ControlType = VariableControlType.Numeric;
        base.InitializeSlots();
    }

    public override SequenceNode Copy()
    {
        var node = new VariableNodeLong
        {
            Value = Value
        };
        return node;
    }
}

[Name("Float")]
public class VariableNodeFloat : GenericVariableNode<float>
{
    public override string? Title => "Float";

    protected override void InitializeSlots()
    {
        ControlType = VariableControlType.Numeric;
        base.InitializeSlots();
    }

    public override SequenceNode Copy()
    {
        var node = new VariableNodeFloat
        {
            Value = Value
        };
        return node;
    }
}

[Name("Double")]
public class VariableNodeDouble : GenericVariableNode<double>
{
    public override string? Title => "Double";

    protected override void InitializeSlots()
    {
        ControlType = VariableControlType.Numeric;
        base.InitializeSlots();
    }

    public override SequenceNode Copy()
    {
        var node = new VariableNodeDouble
        {
            Value = Value
        };
        return node;
    }
}

[Name("Unsigned Byte")]
public class VariableNodeUByte : GenericVariableNode<byte>
{
    public override string? Title => "Unsigned Byte";

    protected override void InitializeSlots()
    {
        ControlType = VariableControlType.Numeric;
        base.InitializeSlots();
    }

    public override SequenceNode Copy()
    {
        var node = new VariableNodeUByte
        {
            Value = Value
        };
        return node;
    }
}

[Name("Unsigned Short")]
public class VariableNodeUShort : GenericVariableNode<ushort>
{
    public override string? Title => "Unsigned Short";

    protected override void InitializeSlots()
    {
        ControlType = VariableControlType.Numeric;
        base.InitializeSlots();
    }

    public override SequenceNode Copy()
    {
        var node = new VariableNodeUShort
        {
            Value = Value
        };
        return node;
    }
}

[Name("Unsigned Integer")]
public class VariableNodeUInt : GenericVariableNode<uint>
{
    public override string? Title => "Unsigned Integer";

    protected override void InitializeSlots()
    {
        ControlType = VariableControlType.Numeric;
        base.InitializeSlots();
    }

    public override SequenceNode Copy()
    {
        var node = new VariableNodeUInt
        {
            Value = Value
        };
        return node;
    }
}

[Name("Unsigned Long")]
public class VariableNodeULong : GenericVariableNode<ulong>
{
    public override string? Title => "Unsigned Long";

    protected override void InitializeSlots()
    {
        ControlType = VariableControlType.Numeric;
        base.InitializeSlots();
    }

    public override SequenceNode Copy()
    {
        var node = new VariableNodeULong
        {
            Value = Value
        };
        return node;
    }
}