using System.ComponentModel;
using System.Xml;
using FlowGraph.Attributes;
using FlowGraph.Process;

namespace FlowGraph.Nodes.Actions;

[Visible(false)]
public class ReturnNode
    : ActionNode
{
    public enum NodeSlotId
    {
        In,
        InputStart
    }

    private int _functionId = -1; // used when the node is loaded, in order to retrieve the function
    private SequenceFunction? _function;

    private List<int> _outputIds = new();

    public override string? Title
    {
        get
        {
            GetFunction(); // TODO : ugly but the fast way to initialize
            return "ReturnNode";
        }
    }

    public ReturnNode(SequenceFunction? function)
    {
        _function = function;
        _function.PropertyChanged += OnFuntionPropertyChanged;
    }

    public ReturnNode(XmlNode node)
        : base(node)
    {

    }

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

        OnPropertyChanged("Slots");
    }

    private void UpdateNodeSlot()
    {
        GetFunction();

        foreach (var slot in _function.Outputs)
        {
            AddFunctionSlot((int)NodeSlotId.InputStart + slot.Id, SlotType.VarIn, slot);
            //AddSlot((int)NodeSlotId.InputStart + slot.Id, slot.Name, SlotType.VarIn, typeof(int));
        }

        OnPropertyChanged("Slots");
        //OnPropertyChanged("SlotVariableIn");
    }

    private SequenceFunction? GetFunction()
    {
        if (_function == null
            && _functionId != -1)
        {
            SetFunction(GraphDataManager.Instance.GetFunctionById(_functionId));
        }

        return _function;
    }

    private void SetFunction(SequenceFunction? func)
    {
        _function = func;
        _function.PropertyChanged += OnFuntionPropertyChanged;
        _function.FunctionSlotChanged += OnFunctionSlotChanged;
        UpdateNodeSlot();
    }

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

    protected override SequenceNode CopyImpl()
    {
        return new ReturnNode(_function);
    }

    protected override void Load(XmlNode node)
    {
        base.Load(node);
        _functionId = int.Parse(node.Attributes["functionID"].Value);
    }

    public override void Save(XmlNode node)
    {
        base.Save(node);
        node.AddAttribute("functionID", GetFunction().Id.ToString());
    }

    void OnFuntionPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
    }
}