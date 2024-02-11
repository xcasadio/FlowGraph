using System.Xml;
using CSharpSyntax;
using FlowGraph.Attributes;

namespace FlowGraph.Nodes.Actions.Math;

[Category("Maths/Division")]
public abstract class DivisionNode<T> : MathOperatorNode<T>
{
    public override T? DoActivateLogic(T a, T b)
    {
        dynamic x = a;
        dynamic y = b;
        return x / y;
    }

    public override SyntaxNode GenerateAst()
    {
        //new If()

        throw new NotImplementedException();
    }
}
[Name("Byte")]
public class DivisionNodeByte : DivisionNode<sbyte>
{
    public override string? Title => "Division Byte";

    public override SequenceNode Copy() { return new DivisionNodeByte(); }
}

[Name("Short")]
public class DivisionNodeShort : DivisionNode<short>
{
    public override string? Title => "Division Short";

    public override SequenceNode Copy() { return new DivisionNodeShort(); }
}

[Name("Integer")]
public class DivisionNodeInt : DivisionNode<int>
{
    public override string? Title => "Division Integer";

    public override SequenceNode Copy() { return new DivisionNodeInt(); }
}

[Name("Long")]
public class DivisionNodeLong : DivisionNode<long>
{
    public override string? Title => "Division Long";

    public override SequenceNode Copy() { return new DivisionNodeLong(); }
}

[Name("Float")]
public class DivisionNodeFloat : DivisionNode<float>
{
    public override string? Title => "Division Float";

    public override SequenceNode Copy() { return new DivisionNodeFloat(); }
}

[Name("Double")]
public class DivisionNodeDouble : DivisionNode<double>
{
    public override string? Title => "Division Double";

    public override SequenceNode Copy() { return new DivisionNodeDouble(); }
}