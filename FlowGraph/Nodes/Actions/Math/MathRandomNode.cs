using System.Xml;
using CSharpSyntax;
using FlowGraph.Attributes;
using FlowGraph.Logger;
using FlowGraph.Process;
using Logger;

namespace FlowGraph.Nodes.Actions.Math;

[Category("Maths/Random")]
public abstract class MathRandomNode<T> : ActionNode
{
    public enum NodeSlotId
    {
        In,
        Out,
        VarMin,
        VarMax,
        VarResult
    }

    static readonly Random Random = new();

    public MathRandomNode(XmlNode node)
        : base(node) { }

    public MathRandomNode()
    { }

    protected override void InitializeSlots()
    {
        base.InitializeSlots();

        AddSlot((int)NodeSlotId.In, "", SlotType.NodeIn);
        AddSlot((int)NodeSlotId.Out, "", SlotType.NodeOut);

        AddSlot((int)NodeSlotId.VarMin, "Min", SlotType.VarIn, typeof(T));
        AddSlot((int)NodeSlotId.VarMax, "Max", SlotType.VarIn, typeof(T));
        AddSlot((int)NodeSlotId.VarResult, "Result", SlotType.VarOut, typeof(T));
    }

    //public abstract T DoActivateLogic(T min_, T max_);

    public override ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot)
    {
        var info = new ProcessingInfo
        {
            State = LogicState.Ok
        };

        var objMin = GetValueFromSlot((int)NodeSlotId.VarMin);

        if (objMin == null)
        {
            info.ErrorMessage = "Please connect a variable node into the slot Min";
            info.State = LogicState.Warning;

            LogManager.Instance.WriteLine(LogVerbosity.Warning,
                "{0} : {1}.",
                Title, info.ErrorMessage);
        }

        var objMax = GetValueFromSlot((int)NodeSlotId.VarMax);

        if (objMax == null)
        {
            info.ErrorMessage = "Please connect a variable node into the slot Max";
            info.State = LogicState.Warning;

            LogManager.Instance.WriteLine(LogVerbosity.Warning,
                "{0} : Random failed. {1}.",
                Title, info.ErrorMessage);
        }

        if (objMin != null && objMax != null)
        {
            object? result;
            var typeVal = typeof(T);

            if (typeVal == typeof(double)
                || typeVal == typeof(float))
            {
                result = Random.NextDouble();

                dynamic min = objMin;
                dynamic max = objMax;
                result = min + (T)result * (max - min);
            }
            else
            {
                result = Random.Next((int)objMin, (int)objMax);
            }

            SetValueInSlot((int)NodeSlotId.VarResult, result);
        }

        ActivateOutputLink(context, (int)NodeSlotId.Out);

        return info;
    }

    public override SyntaxNode GenerateAst()
    {
        //new If()

        throw new NotImplementedException();
    }
}

[Name("Random Byte")]
public class RandomByteNode : MathRandomNode<sbyte>
{
    public override string? Title => "Random Byte";

    public RandomByteNode()
    { }
    public RandomByteNode(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new RandomByteNode(); }
}

[Name("Random Short")]
public class RandomShortNode : MathRandomNode<short>
{
    public override string? Title => "Random Short";

    public RandomShortNode()
    { }
    public RandomShortNode(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new RandomShortNode(); }
}

[Name("Random Integer")]
public class RandomIntegerNode : MathRandomNode<int>
{
    public override string? Title => "Random Integer";

    public RandomIntegerNode()
    { }
    public RandomIntegerNode(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new RandomIntegerNode(); }
}

[Name("Random Long")]
public class RandomLongNode : MathRandomNode<long>
{
    public override string? Title => "Random Long";

    public RandomLongNode()
    { }
    public RandomLongNode(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new RandomLongNode(); }
}

[Name("Random Float")]
public class RandomFloatNode : MathRandomNode<float>
{
    public override string? Title => "Random Float";

    public RandomFloatNode()
    { }
    public RandomFloatNode(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new RandomFloatNode(); }
}

[Name("Random Double")]
public class RandomDoubleNode : MathRandomNode<double>
{
    public override string? Title => "Random Double";

    public RandomDoubleNode()
    { }
    public RandomDoubleNode(XmlNode node) : base(node) { }

    public override SequenceNode Copy() { return new RandomDoubleNode(); }
}