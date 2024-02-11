using System.Xml;
using CSharpSyntax;
using FlowGraph.Attributes;

namespace FlowGraph.Nodes.Actions.Math;

[Category("Maths/Logic/LessThan")]
public abstract class LessThanNode<T> : MathLogicOperatorNode<T>
{
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

    public override SequenceNode Copy() { return new LessThanNodeByte(); }
}

[Name("Short")]
public class LessThanNodeShort : LessThanNode<short>
{
    public override string? Title => "LessThan Short";

    public override SequenceNode Copy() { return new LessThanNodeShort(); }
}

[Name("Integer")]
public class LessThanNodeInt : LessThanNode<int>
{
    public override string? Title => "LessThan Integer";

    public override SequenceNode Copy() { return new LessThanNodeInt(); }
}

[Name("Long")]
public class LessThanNodeLong : LessThanNode<long>
{
    public override string? Title => "LessThan Long";

    public override SequenceNode Copy() { return new LessThanNodeLong(); }
}

[Name("Float")]
public class LessThanNodeFloat : LessThanNode<float>
{
    public override string? Title => "LessThan Float";

    public override SequenceNode Copy() { return new LessThanNodeFloat(); }
}

[Name("Double")]
public class LessThanNodeDouble : LessThanNode<double>
{
    public override string? Title => "LessThan Double";

    public override SequenceNode Copy() { return new LessThanNodeDouble(); }
}