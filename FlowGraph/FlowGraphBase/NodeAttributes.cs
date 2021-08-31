using System;

namespace FlowGraphBase
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class Category : Attribute
    {
        public string CategoryPath
        {
            get;
        }

        /// <summary>
        /// / as separator
        /// </summary>
        /// <param name="category_"></param>
        public Category(string category_)
        {
            CategoryPath = category_;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class Name : Attribute
    {
        public string DisplayName
        {
            get;
        }

        /// <summary>
        /// / as separator
        /// </summary>
        /// <param name="category_"></param>
        public Name(string name_)
        {
            DisplayName = name_;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class Visible : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public bool Value
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val_"></param>
        public Visible(bool val_)
        {
            Value = val_;
        }
    }
}
