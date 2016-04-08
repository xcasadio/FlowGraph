using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
		#region Fields

        private const string NullToken = "<null>";
        private Type _variableType;
        private object _value;

		#endregion //Fields
	
		#region Properties

        /// <summary>
        /// Used as nested link with a variable node
        /// </summary>
        public object Value
        {
            get { return _value; }
            set
            {
                try
                {
                    _value = value;
                    OnPropertyChanged("Value");
                }
                catch (System.Exception)
                {
                    return;
                }
                var stringValue = value as string;

                if (stringValue != null)
                {
                    if (string.IsNullOrWhiteSpace(stringValue) == true)
                    {
                        value = null;
                    }
                    else
                    {
                        try
                        {
                            value = Convert.ChangeType(stringValue, _variableType);
                        }
                        catch (System.Exception ex)
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
            get { return _variableType; }
            set
            {
                if (_variableType != value)
                {
                    _variableType = value;
                    OnPropertyChanged("VariableType");
                }
            }
        }

		#endregion //Properties
	
		#region Constructors

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

		#endregion //Constructors
	
		#region Methods

        #region Persistence

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
        public void Load(XmlNode node_)
        {
            XmlNode valNode = node_.SelectSingleNode("ValueContainer");
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

        #endregion // Persistence

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion // INotifyPropertyChanged

		#endregion //Methods
    }
}
