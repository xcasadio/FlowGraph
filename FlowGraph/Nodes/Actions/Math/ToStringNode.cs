using System.Xml;
using CSharpSyntax;
using FlowGraph.Attributes;

namespace FlowGraph.Nodes.Actions.Math;

[Category("Maths/Cast/To String")]
public abstract class ToStringNode<TIn> : MathCastOperatorNode<TIn, string>
{
    public ToStringNode(XmlNode node) : base(node) { }
    public ToStringNode()
    { }

    public override string DoActivateLogic(TIn a)
    {
        dynamic x = a;
        return x.ToString();
    }

    public override SyntaxNode GenerateAst()
    {
        //new If()

        throw new NotImplementedException();
    }
}

[Name("Byte to String")]
public class ToStringNodeByte : ToStringNode<sbyte>
{
    public override string? Title => "Byte to String";

    public ToStringNodeByte()
    { }
    public ToStringNodeByte(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new ToStringNodeByte(); }
}

[Name("Char to String")]
public class ToStringNodeChar : ToStringNode<char>
{
    public override string? Title => "Char to String";

    public ToStringNodeChar()
    { }
    public ToStringNodeChar(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new ToStringNodeChar(); }
}

[Name("Short to String")]
public class ToStringNodeShort : ToStringNode<short>
{
    public override string? Title => "Short to String";

    public ToStringNodeShort()
    { }
    public ToStringNodeShort(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new ToStringNodeShort(); }
}

[Name("Integer to String")]
public class ToStringNodeInteger : ToStringNode<int>
{
    public override string? Title => "Integer to String";

    public ToStringNodeInteger()
    { }
    public ToStringNodeInteger(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new ToStringNodeInteger(); }
}

[Name("Long to String")]
public class ToStringNodeLong : ToStringNode<long>
{
    public override string? Title => "Long to String";

    public ToStringNodeLong()
    { }
    public ToStringNodeLong(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new ToStringNodeLong(); }
}

[Name("Float to String")]
public class ToStringNodeFloat : ToStringNode<float>
{
    public override string? Title => "Float to String";

    public ToStringNodeFloat()
    { }
    public ToStringNodeFloat(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new ToStringNodeFloat(); }
}

[Name("Double to String")]
public class ToStringNodeDouble : ToStringNode<double>
{
    public override string? Title => "Double to String";

    public ToStringNodeDouble()
    { }
    public ToStringNodeDouble(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new ToStringNodeDouble(); }
}