namespace FlowGraph
{
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
}
