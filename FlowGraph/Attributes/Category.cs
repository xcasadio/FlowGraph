namespace FlowGraph.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class Category : Attribute
{
    public string CategoryPath
    {
        get;
    }

    public Category(string category)
    {
        CategoryPath = category;
    }
}