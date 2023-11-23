﻿using System.Xml;
using FlowGraph.Attributes;

namespace FlowGraph.Nodes.Events;

[Visible(false)]
internal class OnEnterFunctionEvent : EventNode
{
    public enum NodeSlotId
    {
        Out,
        OutputStart
    }

    private int _functionId = -1; // used when the node is loaded, in order to retrieve the function
    private SequenceFunction _function;

    public override string? Title => _function.Name + " function";

    public OnEnterFunctionEvent(SequenceFunction? func)
    {
        _function = func;
    }
    /*
    private void OnFunctionSlotChanged(object sender, FunctionSlotChangedEventArg e)
    {
        if (e.Type == FunctionSlotChangedType.Added)
        {
            if (e.FunctionSlot.SlotType == FunctionSlotType.Input)
            {
                AddFunctionSlot((int)NodeSlotId.OutputStart + e.FunctionSlot.Id, SlotType.VarOut, e.FunctionSlot);
                //AddSlot((int)NodeSlotId.OutputStart + e.FunctionSlot.Id, e.FunctionSlot.Name, SlotType.VarOut, typeof(int));
            }
        }
        else if (e.Type == FunctionSlotChangedType.Removed)
        {
            if (e.FunctionSlot.SlotType == FunctionSlotType.Input)
            {
                RemoveSlotById((int)NodeSlotId.OutputStart + e.FunctionSlot.Id);
            }
        }
    }
    */
    private void UpdateNodeSlot()
    {
        foreach (var slot in _function.Inputs)
        {
            AddFunctionSlot((int)NodeSlotId.OutputStart + slot.Id, SlotType.VarOut, slot);
        }
    }

    private void SetFunction(SequenceFunction? func)
    {
        _function = func;
        UpdateNodeSlot();
    }

    protected override void InitializeSlots()
    {
        base.InitializeSlots();

        AddSlot((int)NodeSlotId.Out, "", SlotType.NodeOut);
    }

    protected override void TriggeredImpl(object para)
    {
        //SetValueInSlot(1, para_);
    }

    protected override void Load(XmlNode node)
    {
        base.Load(node);
        _functionId = int.Parse(node.Attributes["functionID"].Value);
    }

    public override void Save(XmlNode node)
    {
        base.Save(node);
        node.AddAttribute("functionID", _function.Id.ToString());
    }

    public override SequenceNode Copy()
    {
        return new OnEnterFunctionEvent(_function);
    }
}