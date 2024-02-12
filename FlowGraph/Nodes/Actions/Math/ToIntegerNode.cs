using CSharpSyntax;
using FlowGraph.Attributes;

namespace FlowGraph.Nodes.Actions.Math;

[Category("Maths/Cast/To Integer")]
public abstract class ToIntegerNode<TIn> : MathCastOperatorNode<TIn, int>
{
    public override int DoActivateLogic(TIn a)
    {
        dynamic x = a;
        return (int)Convert.ChangeType(x, typeof(int));
    }

    public override SyntaxNode GenerateAst(ClassDeclarationSyntax classDeclaration)
    {
        //new If()

        throw new NotImplementedException();
    }
}

[Name("Byte to Integer")]
public class ToIntegerNodeByte : ToIntegerNode<sbyte>
{
    public override string? Title => "Byte to Integer";

    public override SequenceNode Copy() { return new ToIntegerNodeByte(); }
}

[Name("Char to Integer")]
public class ToIntegerNodeChar : ToIntegerNode<char>
{
    public override string? Title => "Char to Integer";

    public override SequenceNode Copy() { return new ToIntegerNodeChar(); }
}

[Name("Short to Integer")]
public class ToIntegerNodeShort : ToIntegerNode<short>
{
    public override string? Title => "Short to Integer";

    public override SequenceNode Copy() { return new ToIntegerNodeShort(); }
}

[Name("Long to Integer")]
public class ToIntegerNodeLong : ToIntegerNode<long>
{
    public override string? Title => "Long to Integer";

    public override SequenceNode Copy() { return new ToIntegerNodeLong(); }
}

[Name("Float to Integer")]
public class ToIntegerNodeFloat : ToIntegerNode<float>
{
    public override string? Title => "Float to Integer";

    public override SequenceNode Copy() { return new ToIntegerNodeFloat(); }
}

[Name("Double to Integer")]
public class ToIntegerNodeDouble : ToIntegerNode<double>
{
    public override string? Title => "Double to Integer";

    public override SequenceNode Copy() { return new ToIntegerNodeDouble(); }
}

[Name("String to Integer")]
public class ToIntegerNodeString : ToIntegerNode<string>
{
    public override string? Title => "Short to Integer";

    public override SequenceNode Copy() { return new ToIntegerNodeString(); }
}