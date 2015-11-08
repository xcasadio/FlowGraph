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
    public class NamedVariable : INotifyPropertyChanged
    {
		#region Fields

        string m_Name;
        ValueContainer m_Value;

		#endregion //Fields
	
		#region Properties

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set
            {
                m_Name = value;
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public object Value
        {
            get { return m_Value.Value; }
            set
            {
                m_Value.Value = value;
                //OnPropertyChanged("Value");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Type VariableType
        {
            get { return m_Value.VariableType; }
        }

        /// <summary>
        /// 
        /// </summary>
        internal ValueContainer InternalValueContainer
        {
            get { return m_Value; }
            set 
            {
                if (m_Value != null)
                {
                    m_Value.PropertyChanged -= new PropertyChangedEventHandler(OnValueContainerPropertyChanged);
                }

                m_Value = value;

                if (m_Value != null)
                {
                    m_Value.PropertyChanged += new PropertyChangedEventHandler(OnValueContainerPropertyChanged);
                }

                OnPropertyChanged("InternalValueContainer");
            }
        }

		#endregion //Properties
	
		#region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name_"></param>
        /// <param name="var_"></param>
        internal NamedVariable(string name_, ValueContainer var_)
        {
            Name = name_;
            InternalValueContainer = var_;
        }

		#endregion //Constructors
	
		#region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnValueContainerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        #region INotifyPropertyChanged Members

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

        #endregion // INotifyPropertyChanged Members

		#endregion //Methods
    }
}
