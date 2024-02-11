using System.Xml;
using CSharpSyntax;
using FlowGraph.Attributes;

namespace FlowGraph.Nodes.Actions.Math;

[Category("Maths/Logic/Equal")]
public abstract class EqualNode<T> : MathLogicOperatorNode<T>
{
    public override bool DoActivateLogic(T a, T b)
    {
        dynamic x = a;
        dynamic y = b;
        return x == y;
    }

    public override SyntaxNode GenerateAst()
    {
        //new If()

        throw new NotImplementedException();
    }
}

[Name("Byte")]
public class EqualNodeByte : EqualNode<sbyte>
{
    public override string? Title => "Equal Byte";

    public override SequenceNode Copy() { return new EqualNodeByte(); }
}

[Name("Short")]
public class EqualNodeShort : EqualNode<short>
{
    public override string? Title => "Equal Short";

    public override SequenceNode Copy() { return new EqualNodeShort(); }
}

[Name("Integer")]
public class EqualNodeInt : EqualNode<int>
{
    public override string? Title => "Equal Integer";

    public override SequenceNode Copy() { return new EqualNodeInt(); }
}

[Name("Long")]
public class EqualNodeLong : EqualNode<long>
{
    public override string? Title => "Equal Long";

    public override SequenceNode Copy() { return new EqualNodeLong(); }
}

[Name("Float")]
public class EqualNodeFloat : EqualNode<float>
{
    public override string? Title => "Equal Float";

    public override SequenceNode Copy() { return new EqualNodeFloat(); }
}

[Name("Double")]
public class EqualNodeDouble : EqualNode<double>
{
    public override string? Title => "Equal Double";

    public override SequenceNode Copy() { return new EqualNodeDouble(); }
}