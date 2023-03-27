using System.ComponentModel;
using System.Xml;
using FlowGraph.Logger;

namespace FlowGraph
{
    class ValueContainer : INotifyPropertyChanged
    {
        private const string? NullToken = "<null>";
        private Type _variableType;
        private object? _value;

        public object? Value
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

                var sameType = value == null || VariableType == value.GetType();

                if (_value == value || sameType != true) return;

                _value = value;
                OnPropertyChanged("Value");
            }
        }

        public Type VariableType
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

        public ValueContainer(Type type, object? obj)
        {
            _variableType = type;
            _value = obj;
        }

        public ValueContainer(XmlNode node)
        {
            Load(node);
        }

        public void Save(XmlNode node)
        {
            const int version = 1;

            XmlNode valNode = node.OwnerDocument.CreateElement("ValueContainer");
            node.AppendChild(valNode);
            valNode.AddAttribute("version", version.ToString());

            var typeName = _variableType == null ? NullToken : _variableType.AssemblyQualifiedName;
            var val = NullToken;

            if (_variableType != null)
            {
                var index = typeName.IndexOf(',', typeName.IndexOf(',') + 1);
                typeName = typeName.Substring(0, index);

                if (_value != null)
                {
                    var conv = TypeDescriptor.GetConverter(_variableType);
                    val = conv.ConvertToString(Value);
                }
            }

            valNode.AddAttribute("type", typeName);
            valNode.InnerText = val;
        }

        public void Load(XmlNode node)
        {
            var valNode = node.SelectSingleNode("ValueContainer");
            var version = int.Parse(valNode.Attributes["version"].Value);
            var typeStr = valNode.Attributes["type"].Value;
            var valStr = valNode.InnerText;
            _variableType = NullToken.Equals(typeStr) ? null : Type.GetType(typeStr);

            if (_variableType != null)
            {
                var conv = TypeDescriptor.GetConverter(_variableType);
                Value = NullToken.Equals(valStr) ? null : conv.ConvertFromString(valStr);
            }
            else
            {
                Value = null;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
