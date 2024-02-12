using CSharpSyntax;
using FlowGraph.Attributes;

namespace FlowGraph.Nodes.Actions.Math;

[Category("Maths/Substraction")]
public abstract class SubstractionNode<T> : MathOperatorNode<T>
{
    public override T? DoActivateLogic(T a, T b)
    {
        dynamic x = a;
        dynamic y = b;
        return x - y;
    }

    public override SyntaxNode GenerateAst(ClassDeclarationSyntax classDeclaration)
    {
        //new If()

        throw new NotImplementedException();
    }
}

[Name("Byte")]
public class SubstractionNodeByte : SubstractionNode<sbyte>
{
    public override string? Title => "Substraction Byte";

    public override SequenceNode Copy() { return new SubstractionNodeByte(); }
}

[Name("Short")]
public class SubstractionNodeShort : SubstractionNode<short>
{
    public override string? Title => "Substraction Short";

    public override SequenceNode Copy() { return new SubstractionNodeShort(); }
}

[Name("Integer")]
public class SubstractionNodeInt : SubstractionNode<int>
{
    public override string? Title => "Substraction Integer";

    public override SequenceNode Copy() { return new SubstractionNodeInt(); }
}

[Name("Long")]
public class SubstractionNodeLong : SubstractionNode<long>
{
    public override string? Title => "Substraction Long";

    public override SequenceNode Copy() { return new SubstractionNodeLong(); }
}

[Name("Float")]
public class SubstractionNodeFloat : SubstractionNode<float>
{
    public override string? Title => "Substraction Float";

    public override SequenceNode Copy() { return new SubstractionNodeFloat(); }
}

[Name("Double")]
public class SubstractionNodeDouble : SubstractionNode<double>
{
    public override string? Title => "Substraction Double";

    public override SequenceNode Copy() { return new SubstractionNodeDouble(); }
}