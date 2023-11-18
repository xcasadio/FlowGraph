using System.Xml;
using DotNetCodeGenerator.Ast;
using FlowGraph.Attributes;

namespace FlowGraph.Nodes.Actions.Math;

[Category("Maths/Substraction")]
public abstract class SubstractionNode<T> : MathOperatorNode<T>
{
    public SubstractionNode(XmlNode node) : base(node) { }

    public SubstractionNode()
    { }

    public override T? DoActivateLogic(T a, T b)
    {
        dynamic x = a;
        dynamic y = b;
        return x - y;
    }

    public override Statement GenerateAst()
    {
        //new If()

        throw new NotImplementedException();
    }
}

[Name("Byte")]
public class SubstractionNodeByte : SubstractionNode<sbyte>
{
    public override string? Title => "Substraction Byte";

    public SubstractionNodeByte()
    { }
    public SubstractionNodeByte(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new SubstractionNodeByte(); }
}

[Name("Short")]
public class SubstractionNodeShort : SubstractionNode<short>
{
    public override string? Title => "Substraction Short";

    public SubstractionNodeShort()
    { }
    public SubstractionNodeShort(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new SubstractionNodeShort(); }
}

[Name("Integer")]
public class SubstractionNodeInt : SubstractionNode<int>
{
    public override string? Title => "Substraction Integer";

    public SubstractionNodeInt()
    { }
    public SubstractionNodeInt(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new SubstractionNodeInt(); }
}

[Name("Long")]
public class SubstractionNodeLong : SubstractionNode<long>
{
    public override string? Title => "Substraction Long";

    public SubstractionNodeLong()
    { }
    public SubstractionNodeLong(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new SubstractionNodeLong(); }
}

[Name("Float")]
public class SubstractionNodeFloat : SubstractionNode<float>
{
    public override string? Title => "Substraction Float";

    public SubstractionNodeFloat()
    { }
    public SubstractionNodeFloat(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new SubstractionNodeFloat(); }
}

[Name("Double")]
public class SubstractionNodeDouble : SubstractionNode<double>
{
    public override string? Title => "Substraction Double";

    public SubstractionNodeDouble()
    { }
    public SubstractionNodeDouble(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new SubstractionNodeDouble(); }
}