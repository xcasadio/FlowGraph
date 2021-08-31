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
        private string _Name;
        private Type _VarType;
        private bool _IsArray;

        /// <summary>
        /// 
        /// </summary>
        public int ID
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
            get => _Name;
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Type VariableType
        {
            get => _VarType;
            set
            {
                if (_VarType != value)
                {
                    _VarType = value;
                    OnPropertyChanged("VariableType");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsArray
        {
            get => _IsArray;
            set
            {
                if (_IsArray != value)
                {
                    _IsArray = value;
                    OnPropertyChanged("IsArray");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_"></param>
        /// <param name="type_"></param>
        public SequenceFunctionSlot(int id_, FunctionSlotType type_)
        {
            ID = id_;
            SlotType = type_;
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
