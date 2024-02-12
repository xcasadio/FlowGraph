using Newtonsoft.Json.Linq;
using CSharpSyntax;
using FlowGraph.Attributes;
using FlowGraph.Nodes.Events;
using FlowGraph.Process;

namespace FlowGraph.Nodes.Actions;

[Visible(false)]
public class CallFunctionNode : ActionNode
{
    public enum NodeSlotId
    {
        In,
        Out,
        InputStart,
        OutputStart = 1073741823 // int.MaxValue / 2
    }

    private int _functionId = -1; // used when the node is loaded, in order to retrieve the function
    private SequenceFunction _function;

    public override string Title => _function.Name + " function";

    public CallFunctionNode(SequenceFunction function)
    {
        _function = function;
        SetFunction(function);
    }

    public CallFunctionNode()
    {

    }

    /*
    private void OnFunctionSlotChanged(object sender, FunctionSlotChangedEventArg e)
    {
        if (e.Type == FunctionSlotChangedType.Added)
        {
            if (e.FunctionSlot.SlotType == FunctionSlotType.Input)
            {
                AddFunctionSlot((int)NodeSlotId.InputStart + e.FunctionSlot.Id, SlotType.VarIn, e.FunctionSlot);
                //AddSlot((int)NodeSlotId.InputStart + e.FunctionSlot.Id, e.FunctionSlot.Name, SlotType.VarIn, typeof(int));
            }
            else if (e.FunctionSlot.SlotType == FunctionSlotType.Output)
            {
                AddFunctionSlot((int)NodeSlotId.OutputStart + e.FunctionSlot.Id, SlotType.VarOut, e.FunctionSlot);
                //AddSlot((int)NodeSlotId.OutputStart + e.FunctionSlot.Id, e.FunctionSlot.Name, SlotType.VarOut, typeof(int));
            }
        }
        else if (e.Type == FunctionSlotChangedType.Removed)
        {
            if (e.FunctionSlot.SlotType == FunctionSlotType.Input)
            {
                RemoveSlotById((int)NodeSlotId.InputStart + e.FunctionSlot.Id);
            }
            else if (e.FunctionSlot.SlotType == FunctionSlotType.Output)
            {
                RemoveSlotById((int)NodeSlotId.OutputStart + e.FunctionSlot.Id);
            }
        }

        OnPropertyChanged("Slots");
    }
    */
    private void UpdateNodeSlot()
    {
        GetFunction();

        foreach (var slot in _function.Inputs)
        {
            AddFunctionSlot((int)NodeSlotId.InputStart + slot.Id, SlotType.VarIn, slot);
        }

        foreach (var slot in _function.Outputs)
        {
            AddFunctionSlot((int)NodeSlotId.OutputStart + slot.Id, SlotType.VarOut, slot);
        }
    }

    private SequenceFunction GetFunction()
    {
        //if (_function == null
        //    && _functionId != -1)
        //{
        //    SetFunction(GraphDataManager.Instance.GetFunctionById(_functionId));
        //}

        return _function;
    }

    private void SetFunction(SequenceFunction func)
    {
        _function = func;
        UpdateNodeSlot();
    }

    protected override void InitializeSlots()
    {
        base.InitializeSlots();

        AddSlot((int)NodeSlotId.In, "", SlotType.NodeIn);
        AddSlot((int)NodeSlotId.Out, "Completed", SlotType.NodeOut);
    }

    public override ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot)
    {
        var info = new ProcessingInfo
        {
            State = LogicState.Ok
        };
        ActivateOutputLink(context, (int)NodeSlotId.Out);
        context.RegisterNextSequence(GetFunction(), typeof(OnEnterFunctionEvent), null);
        return info;
    }

    public override SequenceNode Copy()
    {
        return new CallFunctionNode(_function);
    }

    protected override void Load(JObject node)
    {
        base.Load(node);
        _functionId = node["functionID"].Value<int>();
    }

    internal override void ResolveLinks(JObject connectionListNode, SequenceBase sequence)
    {
        GetFunction();
        base.ResolveLinks(connectionListNode, sequence);
    }

    public override void Save(JObject node)
    {
        base.Save(node);
        node["functionID"] = GetFunction().Id;
    }

    public override SyntaxNode GenerateAst(ClassDeclarationSyntax classDeclaration)
    {
        return Syntax.InvocationExpression(
            Syntax.ParseName("_function.Name")/*,
            Syntax.ArgumentList(Syntax.Argument(Syntax.LiteralExpression("Hello world!")))*/);
    }
}