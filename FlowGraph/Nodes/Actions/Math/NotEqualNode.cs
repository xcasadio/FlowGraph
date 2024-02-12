using CSharpSyntax;
using FlowGraph.Attributes;

namespace FlowGraph.Nodes.Actions.Math;

[Category("Maths/Logic/NotEqual")]
public abstract class NotEqualNode<T> : MathLogicOperatorNode<T>
{
    public override bool DoActivateLogic(T a, T b)
    {
        dynamic x = a;
        dynamic y = b;
        return x != y;
    }

    public override SyntaxNode GenerateAst(ClassDeclarationSyntax classDeclaration)
    {
        //new If()

        throw new NotImplementedException();
    }
}

[Name("Byte")]
public class NotEqualNodeByte : NotEqualNode<sbyte>
{
    public override string? Title => "NotEqual Byte";

    public override SequenceNode Copy() { return new NotEqualNodeByte(); }
}

[Name("Short")]
public class NotEqualNodeShort : NotEqualNode<short>
{
    public override string? Title => "NotEqual Short";

    public override SequenceNode Copy() { return new NotEqualNodeShort(); }
}

[Name("Integer")]
public class NotEqualNodeInt : NotEqualNode<int>
{
    public override string? Title => "NotEqual Integer";

    public override SequenceNode Copy() { return new NotEqualNodeInt(); }
}

[Name("Long")]
public class NotEqualNodeLong : NotEqualNode<long>
{
    public override string? Title => "NotEqual Long";

    public override SequenceNode Copy() { return new NotEqualNodeLong(); }
}

[Name("Float")]
public class NotEqualNodeFloat : NotEqualNode<float>
{
    public override string? Title => "NotEqual Float";

    public override SequenceNode Copy() { return new NotEqualNodeFloat(); }
}

[Name("Double")]
public class NotEqualNodeDouble : NotEqualNode<double>
{
    public override string? Title => "NotEqual Double";

    public override SequenceNode Copy() { return new NotEqualNodeDouble(); }
}