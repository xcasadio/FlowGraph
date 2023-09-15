using System.Xml;
using FlowGraph.Attributes;

namespace FlowGraph.Nodes.Actions.Math;

[Category("Maths/Division")]
public abstract class DivisionNode<T> : MathOperatorNode<T>
{
    public DivisionNode(XmlNode node) : base(node) { }
    public DivisionNode()
    { }

    public override T? DoActivateLogic(T a, T b)
    {
        dynamic x = a;
        dynamic y = b;
        return x / y;
    }
}
[Name("Byte")]
public class DivisionNodeByte : DivisionNode<sbyte>
{
    public override string? Title => "Division Byte";

    public DivisionNodeByte()
    { }
    public DivisionNodeByte(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new DivisionNodeByte(); }
}

[Name("Short")]
public class DivisionNodeShort : DivisionNode<short>
{
    public override string? Title => "Division Short";

    public DivisionNodeShort()
    { }
    public DivisionNodeShort(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new DivisionNodeShort(); }
}

[Name("Integer")]
public class DivisionNodeInt : DivisionNode<int>
{
    public override string? Title => "Division Integer";

    public DivisionNodeInt()
    { }
    public DivisionNodeInt(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new DivisionNodeInt(); }
}

[Name("Long")]
public class DivisionNodeLong : DivisionNode<long>
{
    public override string? Title => "Division Long";

    public DivisionNodeLong()
    { }
    public DivisionNodeLong(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new DivisionNodeLong(); }
}

[Name("Float")]
public class DivisionNodeFloat : DivisionNode<float>
{
    public override string? Title => "Division Float";

    public DivisionNodeFloat()
    { }
    public DivisionNodeFloat(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new DivisionNodeFloat(); }
}

[Name("Double")]
public class DivisionNodeDouble : DivisionNode<double>
{
    public override string? Title => "Division Double";

    public DivisionNodeDouble()
    { }
    public DivisionNodeDouble(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new DivisionNodeDouble(); }
}