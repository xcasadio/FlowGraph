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
        private Type m_VariableType;
        private object m_Value;

		#endregion //Fields
	
		#region Properties

        /// <summary>
        /// Used as nested link with a variable node
        /// </summary>
        public object Value
        {
            get { return m_Value; }
            set
            {
//                 if (value == null)
//                 {
//                     m_Value = value;
//                 } 
//                 else if (VariableType.Equals(value.GetType()))
//                 {
//                     m_Value = value;
//                 }
//                 else 
                bool castOK = true;

                try
                {
                    m_Value = value;
                    OnPropertyChanged("Value");
                }
                catch (System.Exception)
                {
                    castOK = false;
                }

                if (castOK == false)
                {
                    if (value is string)
                    {
                        if (string.IsNullOrWhiteSpace(value as string) == true)
                        {
                            value = null;
                        }
                        else
                        {
                            try
                            {
                                value = Convert.ChangeType((string)value, m_VariableType);
                            }
                            catch (System.Exception ex)
                            {
                                LogManager.Instance.WriteException(ex);
                                return;
                            }
                        }
                    }

                    bool sameType = value == null ? true : VariableType.Equals(value.GetType());

                    if (m_Value != value
                        && sameType == true)
                    {
                        m_Value = value;
                        OnPropertyChanged("Value");
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Type VariableType
        {
            get { return m_VariableType; }
            set
            {
                if (m_VariableType != value)
                {
                    m_VariableType = value;
                    OnPropertyChanged("VariableType");
                }
            }
        }

		#endregion //Properties
	
		#region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type_"></param>
        /// <param name="obj_"></param>
        public ValueContainer(Type type_, object obj_)
        {
            m_VariableType = type_;
            m_Value = obj_;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public ValueContainer(XmlNode node_)
        {
            Load(node_);
        }

		#endregion //Constructors
	
		#region Methods

        #region Persistence

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public void Save(XmlNode node_)
        {
            const int version = 1;

            XmlNode valNode = node_.OwnerDocument.CreateElement("ValueContainer");
            node_.AppendChild(valNode);
            valNode.AddAttribute("version", version.ToString());

            string typeName = m_VariableType == null ? NullToken : m_VariableType.AssemblyQualifiedName;
            string val = NullToken;

            if (m_VariableType != null)
            {
                int index = typeName.IndexOf(',', typeName.IndexOf(',') + 1);
                typeName = typeName.Substring(0, index);

                if (m_Value != null)
                {
                    TypeConverter conv = TypeDescriptor.GetConverter(m_VariableType);
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
            m_VariableType = NullToken.Equals(typeStr) ? null : Type.GetType(typeStr);

            if (m_VariableType != null)
            {
                TypeConverter conv = TypeDescriptor.GetConverter(m_VariableType);
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
