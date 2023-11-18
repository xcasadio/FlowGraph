using System.Xml;
using DotNetCodeGenerator.Ast;
using FlowGraph.Attributes;
using FlowGraph.Logger;
using FlowGraph.Process;
using Logger;

namespace FlowGraph.Nodes.Actions.FlowControl;

[Category("Flow Control"), Name("Branch")]
public class BranchNode : ActionNode
{
    public enum NodeSlotId
    {
        In,
        OutTrue,
        OutFalse,
        VarCond,
    }

    public override string Title => "Branch";

    public BranchNode(XmlNode node)
        : base(node) { }

    public BranchNode()
    { }

    protected override void InitializeSlots()
    {
        base.InitializeSlots();

        AddSlot((int)NodeSlotId.In, "", SlotType.NodeIn);
        AddSlot((int)NodeSlotId.VarCond, "Condition", SlotType.VarIn, typeof(bool));

        AddSlot((int)NodeSlotId.OutTrue, "True", SlotType.NodeOut);
        AddSlot((int)NodeSlotId.OutFalse, "False", SlotType.NodeOut);
    }

    public override ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot)
    {
        var info = new ProcessingInfo
        {
            State = LogicState.Ok
        };

        var objCond = GetValueFromSlot((int)NodeSlotId.VarCond);

        if (objCond == null)
        {
            info.State = LogicState.Warning;
            info.ErrorMessage = "Please connect a variable node into the slot Condition";
            LogManager.Instance.WriteLine(LogVerbosity.Warning,
                "{0} : Branch failed. {1}.",
                Title, info.ErrorMessage);
        }

        if (objCond != null)
        {
            var cond = (bool)objCond;

            if (cond)
            {
                ActivateOutputLink(context, (int)NodeSlotId.OutTrue);
            }
            else
            {
                ActivateOutputLink(context, (int)NodeSlotId.OutFalse);
            }
        }

        return info;
    }

    public override SequenceNode Copy()
    {
        return new BranchNode();
    }

    public override Statement GenerateAst()
    {
        //new If()

        throw new NotImplementedException();
    }
}