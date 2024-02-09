using System.Xml;
using CSharpSyntax;
using FlowGraph.Attributes;

namespace FlowGraph.Nodes.Actions.Math;

[Category("Maths/Cast/To Object")]
public abstract class ToObjectNode<TIn> : MathCastOperatorNode<TIn, object>
{
    public ToObjectNode(XmlNode node) : base(node) { }
    public ToObjectNode()
    { }

    public override object DoActivateLogic(TIn a)
    {
        dynamic x = a;
        return Convert.ChangeType(x, typeof(object));
    }

    public override SyntaxNode GenerateAst()
    {
        //new If()

        throw new NotImplementedException();
    }
}

[Name("Byte to Object")]
public class ToObjectNodeByte : ToObjectNode<sbyte>
{
    public override string? Title => "Byte to Object";

    public ToObjectNodeByte()
    { }
    public ToObjectNodeByte(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new ToObjectNodeByte(); }
}

[Name("Char to Object")]
public class ToObjectNodeChar : ToObjectNode<char>
{
    public override string? Title => "Char to Object";

    public ToObjectNodeChar()
    { }
    public ToObjectNodeChar(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new ToObjectNodeChar(); }
}

[Name("Short to Object")]
public class ToObjectNodeShort : ToObjectNode<short>
{
    public override string? Title => "Short to Object";

    public ToObjectNodeShort()
    { }
    public ToObjectNodeShort(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new ToObjectNodeShort(); }
}

[Name("Integer to Object")]
public class ToObjectNodeInteger : ToObjectNode<int>
{
    public override string? Title => "Integer to Object";

    public ToObjectNodeInteger()
    { }
    public ToObjectNodeInteger(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new ToObjectNodeInteger(); }
}

[Name("Long to Object")]
public class ToObjectNodeLong : ToObjectNode<long>
{
    public override string? Title => "Long to Object";

    public ToObjectNodeLong()
    { }
    public ToObjectNodeLong(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new ToObjectNodeLong(); }
}

[Name("Float to Object")]
public class ToObjectNodeFloat : ToObjectNode<float>
{
    public override string? Title => "Float to Object";

    public ToObjectNodeFloat()
    { }
    public ToObjectNodeFloat(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new ToObjectNodeFloat(); }
}

[Name("Double to Object")]
public class ToObjectNodeDouble : ToObjectNode<double>
{
    public override string? Title => "Double to Object";

    public ToObjectNodeDouble()
    { }
    public ToObjectNodeDouble(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new ToObjectNodeDouble(); }
}

[Name("String to Object")]
public class ToObjectNodeString : ToObjectNode<string>
{
    public override string? Title => "String to Object";

    public ToObjectNodeString()
    { }
    public ToObjectNodeString(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new ToObjectNodeString(); }
}