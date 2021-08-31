using System.ComponentModel;

namespace FlowGraphBase.Process
{
    /// <summary>
    /// 
    /// </summary>
    public class MemoryStackItem : INotifyPropertyChanged
    {
		#region Fields

        private object _Value;

		#endregion //Fields
	
		#region Properties

        /// <summary>
        /// 
        /// </summary>
        public int ID { get; }

        /// <summary>
        /// 
        /// </summary>
        public object Value
        {
            get => _Value;
            set
            {
                if (_Value != value)
                {
                    _Value = value;
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
            ID = id_;
            _Value = val_;
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
            return base.GetHashCode() * ID.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} : {1}", ID, (_Value == null ? "<null>" : _Value.ToString()));
        }

        #endregion // Object

        #endregion //Methods
    }
}
