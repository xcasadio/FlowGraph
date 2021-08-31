using System.ComponentModel;

namespace FlowGraphBase.Process
{
    /// <summary>
    /// 
    /// </summary>
    public class MemoryStackItem : INotifyPropertyChanged
    {
        private object _Value;

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
    }
}
