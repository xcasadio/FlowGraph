using System;
using System.ComponentModel;

namespace FlowGraphBase
{
    /// <summary>
    /// 
    /// </summary>
    public class NamedVariable : INotifyPropertyChanged
    {
        string _Name;
        ValueContainer _Value;

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get => _Name;
            set
            {
                _Name = value;
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public object Value
        {
            get => _Value.Value;
            set => _Value.Value = value;
            //OnPropertyChanged("Value");
        }

        /// <summary>
        /// 
        /// </summary>
        public Type VariableType => _Value.VariableType;

        /// <summary>
        /// 
        /// </summary>
        internal ValueContainer InternalValueContainer
        {
            get => _Value;
            set 
            {
                if (_Value != null)
                {
                    _Value.PropertyChanged -= OnValueContainerPropertyChanged;
                }

                _Value = value;

                if (_Value != null)
                {
                    _Value.PropertyChanged += OnValueContainerPropertyChanged;
                }

                OnPropertyChanged("InternalValueContainer");
            }
        }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnValueContainerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
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
    }
}
