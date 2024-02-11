using Newtonsoft.Json.Linq;
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

    public NamedVariableNode(string? name)
    {
        _value = NamedVariableManager.Instance.GetNamedVariable(name);
        InitializeSlots();
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

    protected override void Load(JObject node)
    {
        System.Diagnostics.Debugger.Break();
        base.Load(node);
        //_value = (NamedVariable)LoadValue(node["value"]);
    }

    protected override object LoadValue(JObject node)
    {
        return NamedVariableManager.Instance.GetNamedVariable(node["varName"].Value<string>());
    }

    protected override void SaveValue(JObject node)
    {
        node["varName"] = _value.Name;
    }
}