﻿using System.Xml;
using FlowGraph.Attributes;

namespace FlowGraph.Nodes.Actions.Math;

[Category("Maths/Logic/Equal")]
public abstract class EqualNode<T> : MathLogicOperatorNode<T>
{
    public EqualNode(XmlNode node) : base(node) { }
    public EqualNode()
    { }

    public override bool DoActivateLogic(T a, T b)
    {
        dynamic x = a;
        dynamic y = b;
        return x == y;
    }
}

[Name("Byte")]
public class EqualNodeByte : EqualNode<sbyte>
{
    public override string? Title => "Equal Byte";

    public EqualNodeByte()
    { }
    public EqualNodeByte(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new EqualNodeByte(); }
}

[Name("Short")]
public class EqualNodeShort : EqualNode<short>
{
    public override string? Title => "Equal Short";

    public EqualNodeShort()
    { }
    public EqualNodeShort(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new EqualNodeShort(); }
}

[Name("Integer")]
public class EqualNodeInt : EqualNode<int>
{
    public override string? Title => "Equal Integer";

    public EqualNodeInt()
    { }
    public EqualNodeInt(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new EqualNodeInt(); }
}

[Name("Long")]
public class EqualNodeLong : EqualNode<long>
{
    public override string? Title => "Equal Long";

    public EqualNodeLong()
    { }
    public EqualNodeLong(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new EqualNodeLong(); }
}

[Name("Float")]
public class EqualNodeFloat : EqualNode<float>
{
    public override string? Title => "Equal Float";

    public EqualNodeFloat()
    { }
    public EqualNodeFloat(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new EqualNodeFloat(); }
}

[Name("Double")]
public class EqualNodeDouble : EqualNode<double>
{
    public override string? Title => "Equal Double";

    public EqualNodeDouble()
    { }
    public EqualNodeDouble(XmlNode node) : base(node) { }

    protected override SequenceNode CopyImpl() { return new EqualNodeDouble(); }
}