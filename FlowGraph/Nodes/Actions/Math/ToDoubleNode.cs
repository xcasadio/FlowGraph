﻿using CSharpSyntax;
using FlowGraph.Attributes;

namespace FlowGraph.Nodes.Actions.Math;

[Category("Maths/Cast/To Double")]
public abstract class ToDoubleNode<TIn> : MathCastOperatorNode<TIn, double>
{
    public override double DoActivateLogic(TIn a)
    {
        dynamic x = a;
        return (double)Convert.ChangeType(x, typeof(double));
    }

    public override SyntaxNode GenerateAst(ClassDeclarationSyntax classDeclaration)
    {
        //new If()

        throw new NotImplementedException();
    }
}

[Name("Byte to Double")]
public class ToDoubleNodeByte : ToDoubleNode<sbyte>
{
    public override string? Title => "Byte to Double";

    public override SequenceNode Copy() { return new ToDoubleNodeByte(); }
}

[Name("Char to Double")]
public class ToDoubleNodeChar : ToDoubleNode<char>
{
    public override string? Title => "Char to Double";

    public override SequenceNode Copy() { return new ToDoubleNodeChar(); }
}

[Name("Short to Double")]
public class ToDoubleNodeShort : ToDoubleNode<short>
{
    public override string? Title => "Short to Double";

    public override SequenceNode Copy() { return new ToDoubleNodeShort(); }
}

[Name("Integer to Double")]
public class ToDoubleNodeInteger : ToDoubleNode<int>
{
    public override string? Title => "Integer to Double";

    public override SequenceNode Copy() { return new ToDoubleNodeInteger(); }
}

[Name("Long to Double")]
public class ToDoubleNodeLong : ToDoubleNode<long>
{
    public override string? Title => "Long to Double";

    public override SequenceNode Copy() { return new ToDoubleNodeLong(); }
}

[Name("Float to Double")]
public class ToDoubleNodeFloat : ToDoubleNode<float>
{
    public override string? Title => "FLoat to Double";

    public override SequenceNode Copy() { return new ToDoubleNodeFloat(); }
}

[Name("String to Double")]
public class ToDoubleNodeString : ToDoubleNode<string>
{
    public override string? Title => "String to Double";

    public override SequenceNode Copy() { return new ToDoubleNodeString(); }
}