using System.ComponentModel;

namespace FlowGraph
{
    public enum FunctionSlotType
    {
        Input,
        Output
    }

    public class SequenceFunctionSlot : INotifyPropertyChanged
    {
        private string? _name;
        private Type _varType;
        private bool _isArray;

        public int Id
        {
            get;
        }

        public FunctionSlotType SlotType
        {
            get;
        }

        public string? Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public Type VariableType
        {
            get => _varType;
            set
            {
                if (_varType != value)
                {
                    _varType = value;
                    OnPropertyChanged("VariableType");
                }
            }
        }

        public bool IsArray
        {
            get => _isArray;
            set
            {
                if (_isArray != value)
                {
                    _isArray = value;
                    OnPropertyChanged("IsArray");
                }
            }
        }

        public SequenceFunctionSlot(int id, FunctionSlotType type)
        {
            Id = id;
            SlotType = type;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
