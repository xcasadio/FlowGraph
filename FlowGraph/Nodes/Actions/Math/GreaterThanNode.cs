using CSharpSyntax;
using FlowGraph.Attributes;

namespace FlowGraph.Nodes.Actions.Math;

[Category("Maths/Logic/GreaterThan")]
public abstract class GreaterThanNode<T> : MathLogicOperatorNode<T>
{
    public override bool DoActivateLogic(T a, T b)
    {
        dynamic x = a;
        dynamic y = b;
        return x > y;
    }

    public override SyntaxNode GenerateAst(ClassDeclarationSyntax classDeclaration)
    {
        //new If()

        throw new NotImplementedException();
    }
}
[Name("Byte")]
public class GreaterThanNodeByte : GreaterThanNode<sbyte>
{
    public override string? Title => "GreaterThan Byte";

    public override SequenceNode Copy() { return new GreaterThanNodeByte(); }
}

[Name("Short")]
public class GreaterThanNodeShort : GreaterThanNode<short>
{
    public override string? Title => "GreaterThan Short";

    public override SequenceNode Copy() { return new GreaterThanNodeShort(); }
}

[Name("Integer")]
public class GreaterThanNodeInt : GreaterThanNode<int>
{
    public override string? Title => "GreaterThan Integer";

    public override SequenceNode Copy() { return new GreaterThanNodeInt(); }
}

[Name("Long")]
public class GreaterThanNodeLong : GreaterThanNode<long>
{
    public override string? Title => "GreaterThan Long";

    public override SequenceNode Copy() { return new GreaterThanNodeLong(); }
}

[Name("Float")]
public class GreaterThanNodeFloat : GreaterThanNode<float>
{
    public override string? Title => "GreaterThan Float";

    public override SequenceNode Copy() { return new GreaterThanNodeFloat(); }
}

[Name("Double")]
public class GreaterThanNodeDouble : GreaterThanNode<double>
{
    public override string? Title => "GreaterThan Double";

    public override SequenceNode Copy() { return new GreaterThanNodeDouble(); }
}
