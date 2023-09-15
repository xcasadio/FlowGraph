using System.Xml;
using FlowGraph.Attributes;
using FlowGraph.Logger;
using FlowGraph.Process;

namespace FlowGraph.Nodes.Actions.Math;

[Category("Variable/Set")]
public abstract class VariableSetNode<T> : ActionNode
{
    public enum NodeSlotId
    {
        In,
        Out,
        Variable,
        Value,
        VarResult
    }

    public VariableSetNode(XmlNode node)
        : base(node) { }

    public VariableSetNode()
    { }

    protected override void InitializeSlots()
    {
        base.InitializeSlots();

        AddSlot((int)NodeSlotId.In, "", SlotType.NodeIn);
        AddSlot((int)NodeSlotId.Out, "", SlotType.NodeOut);

        AddSlot((int)NodeSlotId.Variable, "Variable", SlotType.VarIn, typeof(T), false);
        AddSlot((int)NodeSlotId.Value, "Value", SlotType.VarIn, typeof(T));
        AddSlot((int)NodeSlotId.VarResult, "Result", SlotType.VarOut, typeof(T));
    }

    public override ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot)
    {
        var info = new ProcessingInfo
        {
            State = LogicState.Ok
        };

        var objVariable = GetValueFromSlot((int)NodeSlotId.Variable);
        var objValue = GetValueFromSlot((int)NodeSlotId.Value);

        if (objVariable == null)
        {
            info.State = LogicState.Warning;
            info.ErrorMessage = "Please connect a variable node into the slot Variable";
            LogManager.Instance.WriteLine(LogVerbosity.Warning,
                "{0} : Addition failed. {1}.",
                Title, info.ErrorMessage);
        }
        else if (objValue == null)
        {
            info.State = LogicState.Warning;
            info.ErrorMessage = "Please connect a variable node into the slot Value";

            LogManager.Instance.WriteLine(LogVerbosity.Warning,
                "{0} : Addition failed. {1}.",
                Title, info.ErrorMessage);
        }

        if (objVariable != null
            && objValue != null)
        {
            SetValueInSlot((int)NodeSlotId.Variable, objValue);
            SetValueInSlot((int)NodeSlotId.VarResult, objValue);
        }

        ActivateOutputLink(context, (int)NodeSlotId.Out);

        return info;
    }
}
[Name("Byte")]
public class VariableSetNodeByte : VariableSetNode<sbyte>
{
    public override string? Title => "Set Byte";

    public VariableSetNodeByte()
    { }
    public VariableSetNodeByte(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new VariableSetNodeByte(); }
}

[Name("Short")]
public class VariableSetNodeShort : VariableSetNode<short>
{
    public override string? Title => "Set Short";

    public VariableSetNodeShort()
    { }
    public VariableSetNodeShort(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new VariableSetNodeShort(); }
}

[Name("Integer")]
public class VariableSetNodeInt : VariableSetNode<int>
{
    public override string? Title => "Set Integer";

    public VariableSetNodeInt()
    { }
    public VariableSetNodeInt(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new VariableSetNodeInt(); }
}

[Name("Long")]
public class VariableSetNodeLong : VariableSetNode<long>
{
    public override string? Title => "Set Long";

    public VariableSetNodeLong()
    { }
    public VariableSetNodeLong(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new VariableSetNodeLong(); }
}

[Name("Float")]
public class VariableSetNodeFloat : VariableSetNode<float>
{
    public override string? Title => "Set Float";

    public VariableSetNodeFloat()
    { }
    public VariableSetNodeFloat(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new VariableSetNodeFloat(); }
}

[Name("Double")]
public class VariableSetNodeDouble : VariableSetNode<double>
{
    public override string? Title => "Set Double";

    public VariableSetNodeDouble()
    { }
    public VariableSetNodeDouble(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new VariableSetNodeDouble(); }
}

[Name("String")]
public class VariableSetNodeString : VariableSetNode<double>
{
    public override string? Title => "Set String";

    public VariableSetNodeString()
    { }
    public VariableSetNodeString(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new VariableSetNodeString(); }
}

[Name("Object")]
public class VariableSetNodeObject : VariableSetNode<object>
{
    public override string? Title => "Set Object";

    public VariableSetNodeObject()
    { }
    public VariableSetNodeObject(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new VariableSetNodeObject(); }
}