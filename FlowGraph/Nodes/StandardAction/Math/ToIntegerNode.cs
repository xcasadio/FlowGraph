using System.Xml;
using FlowGraph.Attributes;

namespace FlowGraph.Nodes.StandardAction.Math;

[Category("Maths/Cast/To Integer")]
public abstract class ToIntegerNode<TIn> : MathCastOperatorNode<TIn, int>
{
    public ToIntegerNode(XmlNode node) : base(node) { }
    public ToIntegerNode()
    { }

    public override int DoActivateLogic(TIn a)
    {
        dynamic x = a;
        return (int)Convert.ChangeType(x, typeof(int));
    }
}
[Name("Byte to Integer")]
public class ToIntegerNodeByte : ToIntegerNode<sbyte>
{
    public override string? Title => "Byte to Integer";

    public ToIntegerNodeByte()
    { }
    public ToIntegerNodeByte(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new ToIntegerNodeByte(); }
}

[Name("Char to Integer")]
public class ToIntegerNodeChar : ToIntegerNode<char>
{
    public override string? Title => "Char to Integer";

    public ToIntegerNodeChar()
    { }
    public ToIntegerNodeChar(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new ToIntegerNodeChar(); }
}

[Name("Short to Integer")]
public class ToIntegerNodeShort : ToIntegerNode<short>
{
    public override string? Title => "Short to Integer";

    public ToIntegerNodeShort()
    { }
    public ToIntegerNodeShort(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new ToIntegerNodeShort(); }
}

[Name("Long to Integer")]
public class ToIntegerNodeLong : ToIntegerNode<long>
{
    public override string? Title => "Long to Integer";

    public ToIntegerNodeLong()
    { }
    public ToIntegerNodeLong(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new ToIntegerNodeLong(); }
}

[Name("Float to Integer")]
public class ToIntegerNodeFloat : ToIntegerNode<float>
{
    public override string? Title => "Float to Integer";

    public ToIntegerNodeFloat()
    { }
    public ToIntegerNodeFloat(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new ToIntegerNodeFloat(); }
}

[Name("Double to Integer")]
public class ToIntegerNodeDouble : ToIntegerNode<double>
{
    public override string? Title => "Double to Integer";

    public ToIntegerNodeDouble()
    { }
    public ToIntegerNodeDouble(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new ToIntegerNodeDouble(); }
}

[Name("String to Integer")]
public class ToIntegerNodeString : ToIntegerNode<string>
{
    public override string? Title => "Short to Integer";

    public ToIntegerNodeString()
    { }
    public ToIntegerNodeString(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new ToIntegerNodeString(); }
}