using System.ComponentModel;

namespace FlowGraph
{
    public class NamedVariable : INotifyPropertyChanged
    {
        private string? _name;
        private ValueContainer _value;

        public string? Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public object? Value
        {
            get => _value.Value;
            set => _value.Value = value;
            //OnPropertyChanged("Value");
        }

        public Type VariableType => _value.VariableType;

        internal ValueContainer InternalValueContainer
        {
            get => _value;
            set
            {
                _value.PropertyChanged -= OnValueContainerPropertyChanged!;

                _value = value;

                _value.PropertyChanged += OnValueContainerPropertyChanged!;

                OnPropertyChanged("InternalValueContainer");
            }
        }

        internal NamedVariable(string? name, ValueContainer var)
        {
            Name = name;
            _value = var;
            InternalValueContainer = var;
        }

        private void OnValueContainerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName!);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
