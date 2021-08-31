using System;
using System.ComponentModel;
using System.Xml;
using FlowGraphBase.Logger;

namespace FlowGraphBase
{
    /// <summary>
    /// 
    /// </summary>
    class ValueContainer : INotifyPropertyChanged
    {
        private const string NullToken = "<null>";
        private Type _variableType;
        private object _value;

        /// <summary>
        /// Used as nested link with a variable node
        /// </summary>
        public object Value
        {
            get => _value;
            set
            {
                try
                {
                    _value = value;
                    OnPropertyChanged("Value");
                }
                catch (Exception)
                {
                    return;
                }

                if (value is string stringValue)
                {
                    if (string.IsNullOrWhiteSpace(stringValue))
                    {
                        value = null;
                    }
                    else
                    {
                        try
                        {
                            value = Convert.ChangeType(stringValue, _variableType);
                        }
                        catch (Exception ex)
                        {
                            LogManager.Instance.WriteException(ex);
                            return;
                        }
                    }
                }

                bool sameType = value == null || VariableType == value.GetType();

                if (_value == value || sameType != true) return;

                _value = value;
                OnPropertyChanged("Value");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Type VariableType
        {
            get => _variableType;
            set
            {
                if (_variableType != value)
                {
                    _variableType = value;
                    OnPropertyChanged("VariableType");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        public ValueContainer(Type type, object obj)
        {
            _variableType = type;
            _value = obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public ValueContainer(XmlNode node)
        {
            Load(node);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public void Save(XmlNode node)
        {
            const int version = 1;

            XmlNode valNode = node.OwnerDocument.CreateElement("ValueContainer");
            node.AppendChild(valNode);
            valNode.AddAttribute("version", version.ToString());

            string typeName = _variableType == null ? NullToken : _variableType.AssemblyQualifiedName;
            string val = NullToken;

            if (_variableType != null)
            {
                int index = typeName.IndexOf(',', typeName.IndexOf(',') + 1);
                typeName = typeName.Substring(0, index);

                if (_value != null)
                {
                    TypeConverter conv = TypeDescriptor.GetConverter(_variableType);
                    val = conv.ConvertToString(Value);
                }
            }

            valNode.AddAttribute("type", typeName);
            valNode.InnerText = val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public void Load(XmlNode node)
        {
            XmlNode valNode = node.SelectSingleNode("ValueContainer");
            int version = int.Parse(valNode.Attributes["version"].Value);
            string typeStr = valNode.Attributes["type"].Value;
            string valStr = valNode.InnerText;
            _variableType = NullToken.Equals(typeStr) ? null : Type.GetType(typeStr);

            if (_variableType != null)
            {
                TypeConverter conv = TypeDescriptor.GetConverter(_variableType);
                Value = NullToken.Equals(valStr) ? null : conv.ConvertFromString(valStr);
            }
            else
            {
                Value = null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
