using System.Text;
using System.Xml;
using FlowGraph.Attributes;
using FlowGraph.Logger;
using FlowGraph.Process;

namespace FlowGraph.Nodes.Actions.StringManipulation;

[Category("String")]
[Name("Concat")]
public class StringConcatNode : ActionNode
{
    public enum NodeSlotId
    {
        In,
        Out,
        VarA,
        VarB,
        VarResult
    }

    public override string? Title => "String Concat";

    public StringConcatNode()
    { }

    public StringConcatNode(XmlNode node) : base(node) { }

    protected override void InitializeSlots()
    {
        base.InitializeSlots();

        AddSlot((int)NodeSlotId.In, "", SlotType.NodeIn);
        AddSlot((int)NodeSlotId.Out, "", SlotType.NodeOut);

        AddSlot((int)NodeSlotId.VarA, "A", SlotType.VarIn, typeof(string));
        AddSlot((int)NodeSlotId.VarB, "B", SlotType.VarIn, typeof(string));
        AddSlot((int)NodeSlotId.VarResult, "Result", SlotType.VarOut, typeof(string));
    }

    public override ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot)
    {
        var info = new ProcessingInfo
        {
            State = LogicState.Ok
        };

        var objA = GetValueFromSlot((int)NodeSlotId.VarA);

        if (objA == null)
        {
            info.ErrorMessage = "Please connect a variable node into the slot A";
            info.State = LogicState.Warning;

            LogManager.Instance.WriteLine(LogVerbosity.Warning,
                "{0} : {1}.",
                Title, info.ErrorMessage);
        }

        var objB = GetValueFromSlot((int)NodeSlotId.VarB);

        if (objB == null)
        {
            info.ErrorMessage = "Please connect a variable node into the slot B";
            info.State = LogicState.Warning;

            LogManager.Instance.WriteLine(LogVerbosity.Warning,
                "{0} : String Concat failed. {1}.",
                Title, info.ErrorMessage);
        }

        if (objA != null && objB != null)
        {
            var result = new StringBuilder();
            result.Append(objA);
            result.Append(objB);
            SetValueInSlot((int)NodeSlotId.VarResult, result.ToString());
        }

        ActivateOutputLink(context, (int)NodeSlotId.Out);

        return info;
    }

    protected override SequenceNode CopyImpl() { return new StringConcatNode(); }
}