﻿using CSharpSyntax;
using FlowGraph.Attributes;
using FlowGraph.Logger;
using FlowGraph.Process;
using Logger;

namespace FlowGraph.Nodes.Actions.FlowControl;

[Category("Flow Control"), Name("Do N")]
public class DoNNode : ActionNode
{
    public enum NodeSlotId
    {
        InEnter,
        InReset,
        Out,
        VarInN,
        VarCounter,
    }

    bool _isInitial;
    int _counter;

    public override string Title => "Do N";

    protected override void InitializeSlots()
    {
        base.InitializeSlots();

        AddSlot((int)NodeSlotId.InEnter, "Enter", SlotType.NodeIn);
        AddSlot((int)NodeSlotId.VarInN, "N", SlotType.VarIn, typeof(int));
        AddSlot((int)NodeSlotId.InReset, "Reset", SlotType.NodeIn);

        AddSlot((int)NodeSlotId.Out, "Exit", SlotType.NodeOut);
        AddSlot((int)NodeSlotId.VarCounter, "Counter", SlotType.VarOut, typeof(int));
    }

    public override ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot)
    {
        var info = new ProcessingInfo
        {
            State = LogicState.Ok
        };

        if (slot.Id == (int)NodeSlotId.InReset)
        {
            _counter = 0;
            _isInitial = false;
        }
        else if (slot.Id == (int)NodeSlotId.InEnter)
        {
            var memoryItem = context.CurrentFrame.GetValueFromId(slot.Id);

            if (_isInitial == false)
            {
                var objN = GetValueFromSlot((int)NodeSlotId.VarInN);

                if (objN == null)
                {
                    info.State = LogicState.Warning;
                    info.ErrorMessage = "Please connect a variable node into the slot N";
                    LogManager.Instance.WriteLine(LogVerbosity.Warning,
                        "{0} : DoN failed. {1}.",
                        Title, info.ErrorMessage);
                }
                else if (objN != null)
                {
                    var n = (int)objN;

                    if (memoryItem == null)
                    {
                        memoryItem = context.CurrentFrame.Allocate(slot.Id, n);
                    }

                    memoryItem.Value = n;
                }

                _isInitial = true;
            }

            if (_counter < (int)memoryItem.Value)
            {
                _counter++;
                ActivateOutputLink(context, (int)NodeSlotId.Out);
            }
        }

        return info;
    }

    public override SequenceNode Copy()
    {
        return new DoNNode();
    }

    public override SyntaxNode GenerateAst(ClassDeclarationSyntax classDeclaration)
    {
        //new If()

        throw new NotImplementedException();
    }
}