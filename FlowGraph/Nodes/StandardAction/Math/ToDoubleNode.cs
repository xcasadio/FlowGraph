using System.Xml;
using FlowGraph.Attributes;

namespace FlowGraph.Nodes.StandardAction.Math;

[Category("Maths/Cast/To Double")]
public abstract class ToDoubleNode<TIn> : MathCastOperatorNode<TIn, double>
{
    public ToDoubleNode(XmlNode node) : base(node) { }
    public ToDoubleNode()
    { }

    public override double DoActivateLogic(TIn a)
    {
        dynamic x = a;
        return (double)Convert.ChangeType(x, typeof(double));
    }
}

[Name("Byte to Double")]
public class ToDoubleNodeByte : ToDoubleNode<sbyte>
{
    public override string? Title => "Byte to Double";

    public ToDoubleNodeByte()
    { }
    public ToDoubleNodeByte(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new ToDoubleNodeByte(); }
}

[Name("Char to Double")]
public class ToDoubleNodeChar : ToDoubleNode<char>
{
    public override string? Title => "Char to Double";

    public ToDoubleNodeChar()
    { }
    public ToDoubleNodeChar(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new ToDoubleNodeChar(); }
}

[Name("Short to Double")]
public class ToDoubleNodeShort : ToDoubleNode<short>
{
    public override string? Title => "Short to Double";

    public ToDoubleNodeShort()
    { }
    public ToDoubleNodeShort(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new ToDoubleNodeShort(); }
}

[Name("Integer to Double")]
public class ToDoubleNodeInteger : ToDoubleNode<int>
{
    public override string? Title => "Integer to Double";

    public ToDoubleNodeInteger()
    { }
    public ToDoubleNodeInteger(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new ToDoubleNodeInteger(); }
}

[Name("Long to Double")]
public class ToDoubleNodeLong : ToDoubleNode<long>
{
    public override string? Title => "Long to Double";

    public ToDoubleNodeLong()
    { }
    public ToDoubleNodeLong(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new ToDoubleNodeLong(); }
}

[Name("Float to Double")]
public class ToDoubleNodeFloat : ToDoubleNode<float>
{
    public override string? Title => "FLoat to Double";

    public ToDoubleNodeFloat()
    { }
    public ToDoubleNodeFloat(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new ToDoubleNodeFloat(); }
}

[Name("String to Double")]
public class ToDoubleNodeString : ToDoubleNode<string>
{
    public override string? Title => "String to Double";

    public ToDoubleNodeString()
    { }
    public ToDoubleNodeString(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new ToDoubleNodeString(); }
}