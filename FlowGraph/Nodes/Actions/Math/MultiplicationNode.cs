using CSharpSyntax;
using FlowGraph.Attributes;

namespace FlowGraph.Nodes.Actions.Math;

[Category("Maths/Multiplication")]
public abstract class MultiplicationNode<T> : MathOperatorNode<T>
{
    public override T? DoActivateLogic(T a, T b)
    {
        dynamic x = a;
        dynamic y = b;
        return x * y;
    }

    public override SyntaxNode GenerateAst(ClassDeclarationSyntax classDeclaration)
    {
        //new If()

        throw new NotImplementedException();
    }
}

[Name("Byte")]
public class MultiplicationNodeByte : MultiplicationNode<sbyte>
{
    public override string? Title => "Multiplication Byte";

    public override SequenceNode Copy() { return new MultiplicationNodeByte(); }
}

[Name("Short")]
public class MultiplicationNodeShort : MultiplicationNode<short>
{
    public override string? Title => "Multiplication Short";

    public override SequenceNode Copy() { return new MultiplicationNodeShort(); }
}

[Name("Integer")]
public class MultiplicationNodeInt : MultiplicationNode<int>
{
    public override string? Title => "Multiplication Integer";

    public override SequenceNode Copy() { return new MultiplicationNodeInt(); }
}

[Name("Long")]
public class MultiplicationNodeLong : MultiplicationNode<long>
{
    public override string? Title => "Multiplication Long";

    public override SequenceNode Copy() { return new MultiplicationNodeLong(); }
}

[Name("Float")]
public class MultiplicationNodeFloat : MultiplicationNode<float>
{
    public override string? Title => "Multiplication Float";

    public override SequenceNode Copy() { return new MultiplicationNodeFloat(); }
}

[Name("Double")]
public class MultiplicationNodeDouble : MultiplicationNode<double>
{
    public override string? Title => "Multiplication Double";

    public override SequenceNode Copy() { return new MultiplicationNodeDouble(); }
}