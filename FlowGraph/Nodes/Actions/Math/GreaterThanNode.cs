using System.Xml;
using CSharpSyntax;
using FlowGraph.Attributes;

namespace FlowGraph.Nodes.Actions.Math;

[Category("Maths/Logic/GreaterThan")]
public abstract class GreaterThanNode<T> : MathLogicOperatorNode<T>
{
    public GreaterThanNode(XmlNode node) : base(node) { }
    public GreaterThanNode()
    { }

    public override bool DoActivateLogic(T a, T b)
    {
        dynamic x = a;
        dynamic y = b;
        return x > y;
    }

    public override SyntaxNode GenerateAst()
    {
        //new If()

        throw new NotImplementedException();
    }
}
[Name("Byte")]
public class GreaterThanNodeByte : GreaterThanNode<sbyte>
{
    public override string? Title => "GreaterThan Byte";

    public GreaterThanNodeByte()
    { }
    public GreaterThanNodeByte(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new GreaterThanNodeByte(); }
}

[Name("Short")]
public class GreaterThanNodeShort : GreaterThanNode<short>
{
    public override string? Title => "GreaterThan Short";

    public GreaterThanNodeShort()
    { }
    public GreaterThanNodeShort(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new GreaterThanNodeShort(); }
}

[Name("Integer")]
public class GreaterThanNodeInt : GreaterThanNode<int>
{
    public override string? Title => "GreaterThan Integer";

    public GreaterThanNodeInt()
    { }
    public GreaterThanNodeInt(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new GreaterThanNodeInt(); }
}

[Name("Long")]
public class GreaterThanNodeLong : GreaterThanNode<long>
{
    public override string? Title => "GreaterThan Long";

    public GreaterThanNodeLong()
    { }
    public GreaterThanNodeLong(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new GreaterThanNodeLong(); }
}

[Name("Float")]
public class GreaterThanNodeFloat : GreaterThanNode<float>
{
    public override string? Title => "GreaterThan Float";

    public GreaterThanNodeFloat()
    { }
    public GreaterThanNodeFloat(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new GreaterThanNodeFloat(); }
}

[Name("Double")]
public class GreaterThanNodeDouble : GreaterThanNode<double>
{
    public override string? Title => "GreaterThan Double";

    public GreaterThanNodeDouble()
    { }
    public GreaterThanNodeDouble(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new GreaterThanNodeDouble(); }
}
