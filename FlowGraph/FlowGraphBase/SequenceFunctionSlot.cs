using System;
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
        private string _name;
        private Type _varType;
        private bool _isArray;

        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public FunctionSlotType SlotType
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
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

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        public SequenceFunctionSlot(int id, FunctionSlotType type)
        {
            Id = id;
            SlotType = type;
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
