using System.Xml;
using FlowGraph.Attributes;

namespace FlowGraph.Nodes.Actions.Math;

[Category("Maths/Logic/GreaterThanOrEqual")]
public abstract class GreaterThanOrEqualNode<T> : MathLogicOperatorNode<T>
{
    public GreaterThanOrEqualNode(XmlNode node) : base(node) { }
    public GreaterThanOrEqualNode()
    { }

    public override bool DoActivateLogic(T a, T b)
    {
        dynamic x = a;
        dynamic y = b;
        return x >= y;
    }
}
[Name("Byte")]
public class GreaterThanOrEqualNodeByte : GreaterThanOrEqualNode<sbyte>
{
    public override string? Title => "GreaterThanOrEqual Byte";

    public GreaterThanOrEqualNodeByte()
    { }
    public GreaterThanOrEqualNodeByte(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new GreaterThanOrEqualNodeByte(); }
}

[Name("Short")]
public class GreaterThanOrEqualNodeShort : GreaterThanOrEqualNode<short>
{
    public override string? Title => "GreaterThanOrEqual Short";

    public GreaterThanOrEqualNodeShort()
    { }
    public GreaterThanOrEqualNodeShort(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new GreaterThanOrEqualNodeShort(); }
}

[Name("Integer")]
public class GreaterThanOrEqualNodeInt : GreaterThanOrEqualNode<int>
{
    public override string? Title => "GreaterThanOrEqual Integer";

    public GreaterThanOrEqualNodeInt()
    { }
    public GreaterThanOrEqualNodeInt(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new GreaterThanOrEqualNodeInt(); }
}

[Name("Long")]
public class GreaterThanOrEqualNodeLong : GreaterThanOrEqualNode<long>
{
    public override string? Title => "GreaterThanOrEqual Long";

    public GreaterThanOrEqualNodeLong()
    { }
    public GreaterThanOrEqualNodeLong(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new GreaterThanOrEqualNodeLong(); }
}

[Name("Float")]
public class GreaterThanOrEqualNodeFloat : GreaterThanOrEqualNode<float>
{
    public override string? Title => "GreaterThanOrEqual Float";

    public GreaterThanOrEqualNodeFloat()
    { }
    public GreaterThanOrEqualNodeFloat(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new GreaterThanOrEqualNodeFloat(); }
}

[Name("Double")]
public class GreaterThanOrEqualNodeDouble : GreaterThanOrEqualNode<double>
{
    public override string? Title => "GreaterThanOrEqual Double";

    public GreaterThanOrEqualNodeDouble()
    { }
    public GreaterThanOrEqualNodeDouble(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new GreaterThanOrEqualNodeDouble(); }
}