using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace FlowGraphBase
{
    /// <summary>
    /// 
    /// </summary>
    public enum FunctionSlotType
    {
        Input,
        Output
    }

    /// <summary>
    /// 
    /// </summary>
    public class SequenceFunctionSlot : INotifyPropertyChanged
    {
		#region Fields

        private string m_Name;
        private Type m_VarType;
        private bool m_IsArray;

		#endregion //Fields
	
		#region Properties

        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public FunctionSlotType SlotType
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set
            {
                if (m_Name != value)
                {
                    m_Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Type VariableType
        {
            get { return m_VarType; }
            set
            {
                if (m_VarType != value)
                {
                    m_VarType = value;
                    OnPropertyChanged("VariableType");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsArray
        {
            get { return m_IsArray; }
            set
            {
                if (m_IsArray != value)
                {
                    m_IsArray = value;
                    OnPropertyChanged("IsArray");
                }
            }
        }

		#endregion //Properties
	
		#region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_"></param>
        /// <param name="type_"></param>
        public SequenceFunctionSlot(int id_, FunctionSlotType type_)
        {
            ID = id_;
            SlotType = type_;
        }

		#endregion //Constructors
	
		#region Methods

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion // INotifyPropertyChanged

		#endregion //Methods
    }
}
