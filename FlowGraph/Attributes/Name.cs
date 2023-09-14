namespace FlowGraph.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class Name : Attribute
{
    public string DisplayName
    {
        get;
    }

    public Name(string name)
    {
        DisplayName = name;
    }
}