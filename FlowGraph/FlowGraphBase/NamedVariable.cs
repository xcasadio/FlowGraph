using System;
using System.ComponentModel;

namespace FlowGraphBase
{
    /// <summary>
    /// 
    /// </summary>
    public class NamedVariable : INotifyPropertyChanged
    {
        string _name;
        ValueContainer _value;

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public object Value
        {
            get => _value.Value;
            set => _value.Value = value;
            //OnPropertyChanged("Value");
        }

        /// <summary>
        /// 
        /// </summary>
        public Type VariableType => _value.VariableType;

        /// <summary>
        /// 
        /// </summary>
        internal ValueContainer InternalValueContainer
        {
            get => _value;
            set 
            {
                if (_value != null)
                {
                    _value.PropertyChanged -= OnValueContainerPropertyChanged;
                }

                _value = value;

                if (_value != null)
                {
                    _value.PropertyChanged += OnValueContainerPropertyChanged;
                }

                OnPropertyChanged("InternalValueContainer");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="var"></param>
        internal NamedVariable(string name, ValueContainer var)
        {
            Name = name;
            InternalValueContainer = var;
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
