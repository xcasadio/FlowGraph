using Newtonsoft.Json.Linq;
using CSharpSyntax;
using FlowGraph.Attributes;
using FlowGraph.Process;

namespace FlowGraph.Nodes.Actions;

[Visible(false)]
public class ReturnNode : ActionNode
{
    public enum NodeSlotId
    {
        In,
        InputStart
    }

    private readonly SequenceFunction _function;

    private List<int> _outputIds = new();

    public override string Title => "ReturnNode";

    public ReturnNode(SequenceFunction function)
    {
        _function = function;
    }

    /*
    private void OnFunctionSlotChanged(object sender, FunctionSlotChangedEventArg e)
    {
        if (e.Type == FunctionSlotChangedType.Added)
        {
            if (e.FunctionSlot.SlotType == FunctionSlotType.Output)
            {
                AddFunctionSlot((int)NodeSlotId.InputStart + e.FunctionSlot.Id, SlotType.VarIn, e.FunctionSlot);
                //AddSlot((int)NodeSlotId.InputStart + e.FunctionSlot.Id, e.FunctionSlot.Name, SlotType.VarIn, typeof(int));
            }
        }
        else if (e.Type == FunctionSlotChangedType.Removed)
        {
            if (e.FunctionSlot.SlotType == FunctionSlotType.Output)
            {
                RemoveSlotById((int)NodeSlotId.InputStart + e.FunctionSlot.Id);
            }
        }
    }*/

    protected override void InitializeSlots()
    {
        base.InitializeSlots();

        AddSlot((int)NodeSlotId.In, "", SlotType.NodeIn);
    }

    public override ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot)
    {
        var info = new ProcessingInfo
        {
            State = LogicState.Ok
        };

        //TODO
        //Set output variable

        // the nodes executed after the node CallFunctionNode are already registered
        // we only have to delete the subsequence
        context.RemoveLastSequence();

        return info;
    }

    public override SequenceNode Copy()
    {
        return new ReturnNode(_function);
    }

    protected override void Load(JObject node)
    {
        base.Load(node);
        //_functionId = int.Parse(node.Attributes["functionID"].Value);
    }

    public override void Save(JObject node)
    {
        base.Save(node);
        node["functionID"] = _function.Id;
    }

    public override SyntaxNode GenerateAst(ClassDeclarationSyntax classDeclaration)
    {
        //new If()

        throw new NotImplementedException();
    }
}