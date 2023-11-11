namespace FlowGraph.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class Visible : Attribute
{
    public bool Value
    {
        get;
    }

    public Visible(bool val)
    {
        Value = val;
    }
}