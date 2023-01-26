using System.ComponentModel;

namespace FlowGraph.Process
{
    public class MemoryStackItem : INotifyPropertyChanged
    {
        private object? _value;
        public int Id { get; }

        public object? Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged("Value");
                }
            }
        }

        public MemoryStackItem(int id, object? val)
        {
            Id = id;
            _value = val;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Id} : {(_value == null ? "<null>" : _value.ToString())}";
        }
    }
}
