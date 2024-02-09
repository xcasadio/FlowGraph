using System.Xml;
using CSharpSyntax;
using FlowGraph.Attributes;

namespace FlowGraph.Nodes.Actions.Math;

[Category("Maths/Logic/LessThanOrEqual")]
public abstract class LessThanOrEqualNode<T> : MathLogicOperatorNode<T>
{
    public LessThanOrEqualNode(XmlNode node) : base(node) { }
    public LessThanOrEqualNode()
    { }

    public override bool DoActivateLogic(T a, T b)
    {
        dynamic x = a;
        dynamic y = b;
        return x <= y;
    }

    public override SyntaxNode GenerateAst()
    {
        //new If()

        throw new NotImplementedException();
    }
}
[Name("Byte")]
public class LessThanOrEqualNodeByte : LessThanOrEqualNode<sbyte>
{
    public override string? Title => "LessThanOrEqual Byte";

    public LessThanOrEqualNodeByte()
    { }
    public LessThanOrEqualNodeByte(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new LessThanOrEqualNodeByte(); }
}

[Name("Short")]
public class LessThanOrEqualNodeShort : LessThanOrEqualNode<short>
{
    public override string? Title => "LessThanOrEqual Short";

    public LessThanOrEqualNodeShort()
    { }
    public LessThanOrEqualNodeShort(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new LessThanOrEqualNodeShort(); }
}

[Name("Integer")]
public class LessThanOrEqualNodeInt : LessThanOrEqualNode<int>
{
    public override string? Title => "LessThanOrEqual Integer";

    public LessThanOrEqualNodeInt()
    { }
    public LessThanOrEqualNodeInt(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new LessThanOrEqualNodeInt(); }
}

[Name("Long")]
public class LessThanOrEqualNodeLong : LessThanOrEqualNode<long>
{
    public override string? Title => "LessThanOrEqual Long";

    public LessThanOrEqualNodeLong()
    { }
    public LessThanOrEqualNodeLong(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new LessThanOrEqualNodeLong(); }
}

[Name("Float")]
public class LessThanOrEqualNodeFloat : LessThanOrEqualNode<float>
{
    public override string? Title => "LessThanOrEqual Float";

    public LessThanOrEqualNodeFloat()
    { }
    public LessThanOrEqualNodeFloat(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new LessThanOrEqualNodeFloat(); }
}

[Name("Double")]
public class LessThanOrEqualNodeDouble : LessThanOrEqualNode<double>
{
    public override string? Title => "LessThanOrEqual Double";

    public LessThanOrEqualNodeDouble()
    { }
    public LessThanOrEqualNodeDouble(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new LessThanOrEqualNodeDouble(); }
}