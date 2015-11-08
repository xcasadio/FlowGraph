using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace FlowGraphBase.Process
{
    /// <summary>
    /// 
    /// </summary>
    public class MemoryStackItem : INotifyPropertyChanged
    {
		#region Fields

        private int m_Id;
        private object m_Value;

		#endregion //Fields
	
		#region Properties

        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            get { return m_Id; }
        }

        /// <summary>
        /// 
        /// </summary>
        public object Value
        {
            get { return m_Value; }
            set
            {
                if (m_Value != value)
                {
                    m_Value = value;
                    OnPropertyChanged("Value");
                }
            }
        }

		#endregion //Properties
	
		#region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_"></param>
        /// <param name="val_"></param>
        public MemoryStackItem(int id_, object val_)
        {
            m_Id = id_;
            m_Value = val_;
        }

		#endregion //Constructors
	
		#region Methods

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

        #region Object

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
//         public override bool Equals(object obj)
//         {
//             return ;
//         }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode() * m_Id.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} : {1}", m_Id, (m_Value == null ? "<null>" : m_Value.ToString()));
        }

        #endregion // Object

        #endregion //Methods
    }
}
