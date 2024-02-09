using System.Xml;
using CSharpSyntax;
using FlowGraph.Attributes;

namespace FlowGraph.Nodes.Actions.Math;

[Category("Maths/Logic/LessThan")]
public abstract class LessThanNode<T> : MathLogicOperatorNode<T>
{
    public LessThanNode(XmlNode node) : base(node) { }
    public LessThanNode()
    { }

    public override bool DoActivateLogic(T a, T b)
    {
        dynamic x = a;
        dynamic y = b;
        return x < y;
    }

    public override SyntaxNode GenerateAst()
    {
        //new If()

        throw new NotImplementedException();
    }
}

[Name("Byte")]
public class LessThanNodeByte : LessThanNode<sbyte>
{
    public override string? Title => "LessThan Byte";

    public LessThanNodeByte()
    { }
    public LessThanNodeByte(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new LessThanNodeByte(); }
}

[Name("Short")]
public class LessThanNodeShort : LessThanNode<short>
{
    public override string? Title => "LessThan Short";

    public LessThanNodeShort()
    { }
    public LessThanNodeShort(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new LessThanNodeShort(); }
}

[Name("Integer")]
public class LessThanNodeInt : LessThanNode<int>
{
    public override string? Title => "LessThan Integer";

    public LessThanNodeInt()
    { }
    public LessThanNodeInt(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new LessThanNodeInt(); }
}

[Name("Long")]
public class LessThanNodeLong : LessThanNode<long>
{
    public override string? Title => "LessThan Long";

    public LessThanNodeLong()
    { }
    public LessThanNodeLong(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new LessThanNodeLong(); }
}

[Name("Float")]
public class LessThanNodeFloat : LessThanNode<float>
{
    public override string? Title => "LessThan Float";

    public LessThanNodeFloat()
    { }
    public LessThanNodeFloat(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new LessThanNodeFloat(); }
}

[Name("Double")]
public class LessThanNodeDouble : LessThanNode<double>
{
    public override string? Title => "LessThan Double";

    public LessThanNodeDouble()
    { }
    public LessThanNodeDouble(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new LessThanNodeDouble(); }
}