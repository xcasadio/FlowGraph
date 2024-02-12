using CSharpSyntax;
using FlowGraph.Attributes;

namespace FlowGraph.Nodes.Actions.Math;

[Category("Maths/Addition")]
public abstract class AdditionNode<T> : MathOperatorNode<T>
{
    public override T? DoActivateLogic(T a, T b)
    {
        dynamic x = a;
        dynamic y = b;
        return x + y;
    }

    public override SyntaxNode GenerateAst(ClassDeclarationSyntax classDeclaration)
    {
        //new If()

        throw new NotImplementedException();
    }
}

[Name("Byte")]
public class AdditionNodeByte : AdditionNode<sbyte>
{
    public override string? Title => "Addition Byte";

    public override SequenceNode Copy() { return new AdditionNodeByte(); }
}

[Name("Short")]
public class AdditionNodeShort : AdditionNode<short>
{
    public override string? Title => "Addition Short";

    public override SequenceNode Copy() { return new AdditionNodeShort(); }
}

[Name("Integer")]
public class AdditionNodeInt : AdditionNode<int>
{
    public override string? Title => "Addition Integer";

    public override SequenceNode Copy() { return new AdditionNodeInt(); }
}

[Name("Long")]
public class AdditionNodeLong : AdditionNode<long>
{
    public override string? Title => "Addition Long";

    public override SequenceNode Copy() { return new AdditionNodeLong(); }
}

[Name("Float")]
public class AdditionNodeFloat : AdditionNode<float>
{
    public override string? Title => "Addition Float";

    public override SequenceNode Copy() { return new AdditionNodeFloat(); }
}

[Name("Double")]
public class AdditionNodeDouble : AdditionNode<double>
{
    public override string? Title => "Addition Double";

    public override SequenceNode Copy() { return new AdditionNodeDouble(); }
}