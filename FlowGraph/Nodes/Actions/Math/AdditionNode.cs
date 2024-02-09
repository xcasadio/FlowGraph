using System.Xml;
using CSharpSyntax;
using FlowGraph.Attributes;

namespace FlowGraph.Nodes.Actions.Math;

[Category("Maths/Addition")]
public abstract class AdditionNode<T> : MathOperatorNode<T>
{
    public AdditionNode(XmlNode node) : base(node) { }
    public AdditionNode() { }

    public override T? DoActivateLogic(T a, T b)
    {
        dynamic x = a;
        dynamic y = b;
        return x + y;
    }

    public override SyntaxNode GenerateAst()
    {
        //new If()

        throw new NotImplementedException();
    }
}

[Name("Byte")]
public class AdditionNodeByte : AdditionNode<sbyte>
{
    public override string? Title => "Addition Byte";
    public AdditionNodeByte() { }
    public AdditionNodeByte(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new AdditionNodeByte(); }
}

[Name("Short")]
public class AdditionNodeShort : AdditionNode<short>
{
    public override string? Title => "Addition Short";

    public AdditionNodeShort() { }
    public AdditionNodeShort(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new AdditionNodeShort(); }
}

[Name("Integer")]
public class AdditionNodeInt : AdditionNode<int>
{
    public override string? Title => "Addition Integer";

    public AdditionNodeInt() { }
    public AdditionNodeInt(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new AdditionNodeInt(); }
}

[Name("Long")]
public class AdditionNodeLong : AdditionNode<long>
{
    public override string? Title => "Addition Long";

    public AdditionNodeLong() { }
    public AdditionNodeLong(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new AdditionNodeLong(); }
}

[Name("Float")]
public class AdditionNodeFloat : AdditionNode<float>
{
    public override string? Title => "Addition Float";

    public AdditionNodeFloat() { }
    public AdditionNodeFloat(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new AdditionNodeFloat(); }
}

[Name("Double")]
public class AdditionNodeDouble : AdditionNode<double>
{
    public override string? Title => "Addition Double";

    public AdditionNodeDouble() { }
    public AdditionNodeDouble(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new AdditionNodeDouble(); }
}