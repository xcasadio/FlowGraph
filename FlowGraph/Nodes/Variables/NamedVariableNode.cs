using System.ComponentModel;
using System.Xml;
using FlowGraph.Attributes;

namespace FlowGraph.Nodes.Variables;

[Visible(false)]
public sealed class NamedVariableNode : VariableNode
{
    NamedVariable _value;

    public override string? Title => _value.Name;
    public string? VariableName => _value.Name;
    public Type VariableType => _value.VariableType;

    public override object? Value
    {
        get => _value.Value;
        set => _value.InternalValueContainer.Value = value;
    }

    public NamedVariableNode(XmlNode node)
        : base(node)
    {
        InitializeSlots();
    }

    public NamedVariableNode(string? name)
    {
        _value = NamedVariableManager.Instance.GetNamedVariable(name);
        _value.PropertyChanged += OnNamedVariablePropertyChanged;
        AddSlot(0, string.Empty, SlotType.VarInOut, _value.VariableType);
    }

    protected override void InitializeSlots()
    {
        base.InitializeSlots();

        if (_value != null) // call only when loaded with xml
        {
            AddSlot(0, string.Empty, SlotType.VarInOut, _value.VariableType);
        }
    }

    public override SequenceNode Copy()
    {
        return new NamedVariableNode(_value.Name);
    }

    private void OnNamedVariablePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(e.PropertyName);
    }

    protected override object LoadValue(XmlNode node)
    {
        return NamedVariableManager.Instance.GetNamedVariable(node.Attributes["varName"].Value);
    }

    protected override void SaveValue(XmlNode node)
    {
        node.AddAttribute("varName", _value.Name);
    }

    protected override void Load(XmlNode node)
    {
        base.Load(node);
        _value = (NamedVariable)LoadValue(node.SelectSingleNode("Value"));
    }
}