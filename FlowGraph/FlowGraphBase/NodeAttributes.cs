using System;

namespace FlowGraphBase
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class Category : Attribute
    {
		#region Fields

        public string CategoryPath
        {
            get;
            private set;
        }

		#endregion //Fields
	
		#region Properties
		
		#endregion //Properties
	
		#region Constructors

        /// <summary>
        /// / as separator
        /// </summary>
        /// <param name="category_"></param>
        public Category(string category_)
        {
            CategoryPath = category_;
        }

		#endregion //Constructors
	
		#region Methods
		
		#endregion //Methods
    }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class Name : Attribute
    {
        #region Fields

        public string DisplayName
        {
            get;
            private set;
        }

        #endregion //Fields

        #region Properties

        #endregion //Properties

        #region Constructors

        /// <summary>
        /// / as separator
        /// </summary>
        /// <param name="category_"></param>
        public Name(string name_)
        {
            DisplayName = name_;
        }

        #endregion //Constructors

        #region Methods

        #endregion //Methods
    }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class Visible : Attribute
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        public bool Value
        {
            get;
            private set;
        }

        #endregion //Fields

        #region Properties

        #endregion //Properties

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val_"></param>
        public Visible(bool val_)
        {
            Value = val_;
        }

        #endregion //Constructors

        #region Methods

        #endregion //Methods
    }
}
