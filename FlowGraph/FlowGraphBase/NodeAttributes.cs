using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlowGraphBase
{
    /// <summary>
    /// 
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
    public class Category : System.Attribute
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
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
    public class Name : System.Attribute
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
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
    public class Visible : System.Attribute
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
