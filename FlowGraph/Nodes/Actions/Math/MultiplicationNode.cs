using System.Xml;
using CSharpSyntax;
using FlowGraph.Attributes;

namespace FlowGraph.Nodes.Actions.Math;

[Category("Maths/Multiplication")]
public abstract class MultiplicationNode<T> : MathOperatorNode<T>
{
    public MultiplicationNode(XmlNode node) : base(node) { }
    public MultiplicationNode()
    { }

    public override T? DoActivateLogic(T a, T b)
    {
        dynamic x = a;
        dynamic y = b;
        return x * y;
    }

    public override SyntaxNode GenerateAst()
    {
        //new If()

        throw new NotImplementedException();
    }
}

[Name("Byte")]
public class MultiplicationNodeByte : MultiplicationNode<sbyte>
{
    public override string? Title => "Multiplication Byte";

    public MultiplicationNodeByte()
    { }
    public MultiplicationNodeByte(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new MultiplicationNodeByte(); }
}

[Name("Short")]
public class MultiplicationNodeShort : MultiplicationNode<short>
{
    public override string? Title => "Multiplication Short";

    public MultiplicationNodeShort()
    { }
    public MultiplicationNodeShort(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new MultiplicationNodeShort(); }
}

[Name("Integer")]
public class MultiplicationNodeInt : MultiplicationNode<int>
{
    public override string? Title => "Multiplication Integer";

    public MultiplicationNodeInt()
    { }
    public MultiplicationNodeInt(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new MultiplicationNodeInt(); }
}

[Name("Long")]
public class MultiplicationNodeLong : MultiplicationNode<long>
{
    public override string? Title => "Multiplication Long";

    public MultiplicationNodeLong()
    { }
    public MultiplicationNodeLong(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new MultiplicationNodeLong(); }
}

[Name("Float")]
public class MultiplicationNodeFloat : MultiplicationNode<float>
{
    public override string? Title => "Multiplication Float";

    public MultiplicationNodeFloat()
    { }
    public MultiplicationNodeFloat(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new MultiplicationNodeFloat(); }
}

[Name("Double")]
public class MultiplicationNodeDouble : MultiplicationNode<double>
{
    public override string? Title => "Multiplication Double";

    public MultiplicationNodeDouble()
    { }
    public MultiplicationNodeDouble(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new MultiplicationNodeDouble(); }
}