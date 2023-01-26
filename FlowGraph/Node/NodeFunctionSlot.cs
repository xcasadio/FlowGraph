using System.ComponentModel;

namespace FlowGraph.Node;

public class NodeFunctionSlot : NodeSlotVar
{
    private readonly SequenceFunctionSlot? _funcSlot;

    public override string? Text
    {
        get => _funcSlot == null ? string.Empty : _funcSlot.Name;
        set
        {
            if (_funcSlot != null)
            {
                _funcSlot.Name = value;
            }
        }
    }

    public override Type VariableType
    {
        get => _funcSlot?.VariableType!;
        set
        {
            if (_funcSlot != null)
            {
                _funcSlot.VariableType = value;
            }
        }
    }

    public NodeFunctionSlot(
        int slotId,
        SequenceNode node,
        SlotType connectionType,
        SequenceFunctionSlot slot,
        VariableControlType controlType = VariableControlType.ReadOnly,
        object? tag = null,
        bool saveValue = true) :

        base(slotId,
            node,
            slot.Name,
            connectionType,
            slot.VariableType,
            controlType,
            tag,
            saveValue)
    {
        _funcSlot = slot;
        _funcSlot.PropertyChanged += OnFunctionSlotPropertyChanged!;
    }

    void OnFunctionSlotPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case "Name":
                OnPropertyChanged("Text");
                break;

            case "VariableType":
                OnPropertyChanged("VariableType");
                break;
            //IsArray
        }
    }
}