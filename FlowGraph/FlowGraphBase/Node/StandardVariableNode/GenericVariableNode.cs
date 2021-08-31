using System;
using System.Xml;

namespace FlowGraphBase.Node.StandardVariableNode
{
    /// <summary>
    /// T value = (T)Convert.ChangeType("123", typeof(T));
    /// 
    /// if you want to add another type implement the extension :
    /// public static T ChangeType<T>(this object obj)
    /// {
    ///     return (T)Convert.ChangeType(obj, typeof(T));
    /// }
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Category("Variable")]
    public abstract class GenericVariableNode<T> : VariableNode
    {
		#region Fields

        private T m_Value;

		#endregion //Fields
	
		#region Properties

        /// <summary>
        /// 
        /// </summary>
        protected VariableControlType ControlType
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override object Value
        {
            get => m_Value;
            set
            {
                if (value == null)
                {
                    m_Value = default(T);
                }
                else if (value is T)
                {
                    m_Value = (T)value;
                }
                else if (value is string)
                {
                    if (string.IsNullOrWhiteSpace(value as string) == false)
                    {
                        m_Value = (T)Convert.ChangeType((string)value, typeof(T));
                    }
                }
                else
                {
                    throw new InvalidCastException("GenericVariableNode<T>.Value : object can not be converted to " + typeof(T).Name + ".");
                }

                OnPropertyChanged("Value");
            }
        }

		#endregion //Properties
	
		#region Constructors
		
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        protected GenericVariableNode(XmlNode node_)
            : base(node_)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title_"></param>
        protected GenericVariableNode()
            : base()
        {
        }

		#endregion //Constructors
	
		#region Methods

        /// <summary>
        /// 
        /// </summary>
        protected override void InitializeSlots()
        {
            base.InitializeSlots();

            AddSlot(0, string.Empty, SlotType.VarInOut, typeof(T), true, ControlType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        protected override void Load(XmlNode node_)
        {
            base.Load(node_);
            m_Value = (T)LoadValue(node_.SelectSingleNode("Value"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        /// <returns></returns>
        protected override object LoadValue(XmlNode node_)
        {
            return (T)Convert.ChangeType(node_.InnerText, typeof(T));
        }

        /// <summary>
        /// Save the value into the InnerText of the XmlNode node_
        /// </summary>
        /// <param name="node_"></param>
        protected override void SaveValue(XmlNode node_)
        {
            node_.InnerText = (string)Convert.ChangeType(Value, typeof(string));
        }

		#endregion //Methods
    }

    #region Declaration

    /// <summary>
    /// 
    /// </summary>
    [Name("Object")]
    public class VariableNodeObject : GenericVariableNode<object>
    {
        public override string Title => "Object";

        public VariableNodeObject() : base() { }
        public VariableNodeObject(XmlNode node_) : base(node_) { }

        protected override void InitializeSlots()
        {
            ControlType = VariableControlType.ReadOnly;
            base.InitializeSlots();
        }

        protected override SequenceNode CopyImpl()
        {
            VariableNodeObject node = new VariableNodeObject();
            node.Value = Value;
            return node;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("String")]
    public class VariableNodeString : GenericVariableNode<string>
    {
        public override string Title => "String";

        public VariableNodeString() : base() 
        {
            Value = string.Empty; 
        }

        public VariableNodeString(XmlNode node_) : base(node_) { }

        protected override void InitializeSlots()
        {
            ControlType = VariableControlType.Text;
            base.InitializeSlots();
        }

        protected override SequenceNode CopyImpl()
        {
            VariableNodeString node = new VariableNodeString();
            node.Value = Value;
            return node;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Boolean")]
    public class VariableNodeBool : GenericVariableNode<bool>
    {
        public override string Title => "Boolean";

        public VariableNodeBool() : base() { }
        public VariableNodeBool(XmlNode node_) : base(node_) { }

        protected override void InitializeSlots()
        {
            ControlType = VariableControlType.Checkable;
            base.InitializeSlots();
        }

        protected override SequenceNode CopyImpl()
        {
            VariableNodeBool node = new VariableNodeBool();
            node.Value = Value;
            return node;
        }
    }

    #region signed

    /// <summary>
    /// 
    /// </summary>
    [Name("Byte")]
    public class VariableNodeByte : GenericVariableNode<sbyte>
    {
        public override string Title => "Byte";

        public VariableNodeByte() : base() { }
        public VariableNodeByte(XmlNode node_) : base(node_) { }

        protected override void InitializeSlots()
        {
            ControlType = VariableControlType.Numeric;
            base.InitializeSlots();
        }

        protected override SequenceNode CopyImpl()
        {
            VariableNodeByte node = new VariableNodeByte();
            node.Value = Value;
            return node;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Char")]
    public class VariableNodeChar : GenericVariableNode<char>
    {
        public override string Title => "Char";

        public VariableNodeChar() : base() { }
        public VariableNodeChar(XmlNode node_) : base(node_) { }

        protected override void InitializeSlots()
        {
            ControlType = VariableControlType.Numeric;
            base.InitializeSlots();
        }

        protected override SequenceNode CopyImpl()
        {
            VariableNodeChar node = new VariableNodeChar();
            node.Value = Value;
            return node;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Short")]
    public class VariableNodeShort : GenericVariableNode<short>
    {
        public override string Title => "Short";

        public VariableNodeShort() : base() { }
        public VariableNodeShort(XmlNode node_) : base(node_) { }

        protected override void InitializeSlots()
        {
            ControlType = VariableControlType.Numeric;
            base.InitializeSlots();
        }

        protected override SequenceNode CopyImpl()
        {
            VariableNodeShort node = new VariableNodeShort();
            node.Value = Value;
            return node;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Integer")]
    public class VariableNodeInt : GenericVariableNode<int>
    {
        public override string Title => "Integer";

        public VariableNodeInt() : base() { }
        public VariableNodeInt(XmlNode node_) : base(node_) { }

        protected override void InitializeSlots()
        {
            ControlType = VariableControlType.Numeric;
            base.InitializeSlots();
        }

        protected override SequenceNode CopyImpl()
        {
            VariableNodeInt node = new VariableNodeInt();
            node.Value = Value;
            return node;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Long")]
    public class VariableNodeLong : GenericVariableNode<long>
    {
        public override string Title => "Long";

        public VariableNodeLong() : base() { }
        public VariableNodeLong(XmlNode node_) : base(node_) { }

        protected override void InitializeSlots()
        {
            ControlType = VariableControlType.Numeric;
            base.InitializeSlots();
        }

        protected override SequenceNode CopyImpl()
        {
            VariableNodeLong node = new VariableNodeLong();
            node.Value = Value;
            return node;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Float")]
    public class VariableNodeFloat : GenericVariableNode<float>
    {
        public override string Title => "Float";

        public VariableNodeFloat() : base() { }
        public VariableNodeFloat(XmlNode node_) : base(node_) { }

        protected override void InitializeSlots()
        {
            ControlType = VariableControlType.Numeric;
            base.InitializeSlots();
        }

        protected override SequenceNode CopyImpl()
        {
            VariableNodeFloat node = new VariableNodeFloat();
            node.Value = Value;
            return node;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Double")]
    public class VariableNodeDouble : GenericVariableNode<double>
    {
        public override string Title => "Double";

        public VariableNodeDouble() : base() { }
        public VariableNodeDouble(XmlNode node_) : base(node_) { }

        protected override void InitializeSlots()
        {
            ControlType = VariableControlType.Numeric;
            base.InitializeSlots();
        }

        protected override SequenceNode CopyImpl()
        {
            VariableNodeDouble node = new VariableNodeDouble();
            node.Value = Value;
            return node;
        }
    }

    #endregion // signed

    #region unsigned

    /// <summary>
    /// 
    /// </summary>
    [Name("Unsigned Byte")]
    public class VariableNodeUByte : GenericVariableNode<byte>
    {
        public override string Title => "Unsigned Byte";

        public VariableNodeUByte() : base() { }
        public VariableNodeUByte(XmlNode node_) : base(node_) { }

        protected override void InitializeSlots()
        {
            ControlType = VariableControlType.Numeric;
            base.InitializeSlots();
        }

        protected override SequenceNode CopyImpl()
        {
            VariableNodeUByte node = new VariableNodeUByte();
            node.Value = Value;
            return node;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Unsigned Short")]
    public class VariableNodeUShort : GenericVariableNode<ushort>
    {
        public override string Title => "Unsigned Short";

        public VariableNodeUShort() : base() { }
        public VariableNodeUShort(XmlNode node_) : base(node_) { }

        protected override void InitializeSlots()
        {
            ControlType = VariableControlType.Numeric;
            base.InitializeSlots();
        }

        protected override SequenceNode CopyImpl()
        {
            VariableNodeUShort node = new VariableNodeUShort();
            node.Value = Value;
            return node;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Unsigned Integer")]
    public class VariableNodeUInt : GenericVariableNode<uint>
    {
        public override string Title => "Unsigned Integer";

        public VariableNodeUInt() : base() { }
        public VariableNodeUInt(XmlNode node_) : base(node_) { }

        protected override void InitializeSlots()
        {
            ControlType = VariableControlType.Numeric;
            base.InitializeSlots();
        }

        protected override SequenceNode CopyImpl()
        {
            VariableNodeUInt node = new VariableNodeUInt();
            node.Value = Value;
            return node;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Unsigned Long")]
    public class VariableNodeULong : GenericVariableNode<ulong>
    {
        public override string Title => "Unsigned Long";

        public VariableNodeULong() : base() { }
        public VariableNodeULong(XmlNode node_) : base(node_) { }

        protected override void InitializeSlots()
        {
            ControlType = VariableControlType.Numeric;
            base.InitializeSlots();
        }

        protected override SequenceNode CopyImpl()
        {
            VariableNodeULong node = new VariableNodeULong();
            node.Value = Value;
            return node;
        }
    }

    #endregion // signed

    #endregion // Declaration
}
