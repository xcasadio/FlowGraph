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
        private T _value;

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
            get => _value;
            set
            {
                switch (value)
                {
                    case null:
                        _value = default;
                        break;
                    case T value1:
                        _value = value1;
                        break;
                    case string s:
                        {
                            if (string.IsNullOrWhiteSpace(s) == false)
                            {
                                _value = (T)Convert.ChangeType(s, typeof(T));
                            }

                            break;
                        }
                    default:
                        throw new InvalidCastException("GenericVariableNode<T>.Value : object can not be converted to " + typeof(T).Name + ".");
                }

                OnPropertyChanged("Value");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        protected GenericVariableNode(XmlNode node)
            : base(node)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        protected GenericVariableNode()
        {
        }

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
        /// <param name="node"></param>
        protected override void Load(XmlNode node)
        {
            base.Load(node);
            _value = (T)LoadValue(node.SelectSingleNode("Value"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override object LoadValue(XmlNode node)
        {
            return (T)Convert.ChangeType(node.InnerText, typeof(T));
        }

        /// <summary>
        /// Save the value into the InnerText of the XmlNode node_
        /// </summary>
        /// <param name="node"></param>
        protected override void SaveValue(XmlNode node)
        {
            node.InnerText = (string)Convert.ChangeType(Value, typeof(string));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Object")]
    public class VariableNodeObject : GenericVariableNode<object>
    {
        public override string Title => "Object";

        public VariableNodeObject()
        { }
        public VariableNodeObject(XmlNode node) : base(node) { }

        protected override void InitializeSlots()
        {
            ControlType = VariableControlType.ReadOnly;
            base.InitializeSlots();
        }

        protected override SequenceNode CopyImpl()
        {
            VariableNodeObject node = new VariableNodeObject
            {
                Value = Value
            };
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

        public VariableNodeString()
        {
            Value = string.Empty;
        }

        public VariableNodeString(XmlNode node) : base(node) { }

        protected override void InitializeSlots()
        {
            ControlType = VariableControlType.Text;
            base.InitializeSlots();
        }

        protected override SequenceNode CopyImpl()
        {
            VariableNodeString node = new VariableNodeString
            {
                Value = Value
            };
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

        public VariableNodeBool()
        { }
        public VariableNodeBool(XmlNode node) : base(node) { }

        protected override void InitializeSlots()
        {
            ControlType = VariableControlType.Checkable;
            base.InitializeSlots();
        }

        protected override SequenceNode CopyImpl()
        {
            VariableNodeBool node = new VariableNodeBool
            {
                Value = Value
            };
            return node;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Byte")]
    public class VariableNodeByte : GenericVariableNode<sbyte>
    {
        public override string Title => "Byte";

        public VariableNodeByte()
        { }
        public VariableNodeByte(XmlNode node) : base(node) { }

        protected override void InitializeSlots()
        {
            ControlType = VariableControlType.Numeric;
            base.InitializeSlots();
        }

        protected override SequenceNode CopyImpl()
        {
            VariableNodeByte node = new VariableNodeByte
            {
                Value = Value
            };
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

        public VariableNodeChar()
        { }
        public VariableNodeChar(XmlNode node) : base(node) { }

        protected override void InitializeSlots()
        {
            ControlType = VariableControlType.Numeric;
            base.InitializeSlots();
        }

        protected override SequenceNode CopyImpl()
        {
            VariableNodeChar node = new VariableNodeChar
            {
                Value = Value
            };
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

        public VariableNodeShort()
        { }
        public VariableNodeShort(XmlNode node) : base(node) { }

        protected override void InitializeSlots()
        {
            ControlType = VariableControlType.Numeric;
            base.InitializeSlots();
        }

        protected override SequenceNode CopyImpl()
        {
            VariableNodeShort node = new VariableNodeShort
            {
                Value = Value
            };
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

        public VariableNodeInt()
        { }
        public VariableNodeInt(XmlNode node) : base(node) { }

        protected override void InitializeSlots()
        {
            ControlType = VariableControlType.Numeric;
            base.InitializeSlots();
        }

        protected override SequenceNode CopyImpl()
        {
            VariableNodeInt node = new VariableNodeInt
            {
                Value = Value
            };
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

        public VariableNodeLong()
        { }
        public VariableNodeLong(XmlNode node) : base(node) { }

        protected override void InitializeSlots()
        {
            ControlType = VariableControlType.Numeric;
            base.InitializeSlots();
        }

        protected override SequenceNode CopyImpl()
        {
            VariableNodeLong node = new VariableNodeLong
            {
                Value = Value
            };
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

        public VariableNodeFloat()
        { }
        public VariableNodeFloat(XmlNode node) : base(node) { }

        protected override void InitializeSlots()
        {
            ControlType = VariableControlType.Numeric;
            base.InitializeSlots();
        }

        protected override SequenceNode CopyImpl()
        {
            VariableNodeFloat node = new VariableNodeFloat
            {
                Value = Value
            };
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

        public VariableNodeDouble()
        { }
        public VariableNodeDouble(XmlNode node) : base(node) { }

        protected override void InitializeSlots()
        {
            ControlType = VariableControlType.Numeric;
            base.InitializeSlots();
        }

        protected override SequenceNode CopyImpl()
        {
            VariableNodeDouble node = new VariableNodeDouble
            {
                Value = Value
            };
            return node;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Unsigned Byte")]
    public class VariableNodeUByte : GenericVariableNode<byte>
    {
        public override string Title => "Unsigned Byte";

        public VariableNodeUByte()
        { }
        public VariableNodeUByte(XmlNode node) : base(node) { }

        protected override void InitializeSlots()
        {
            ControlType = VariableControlType.Numeric;
            base.InitializeSlots();
        }

        protected override SequenceNode CopyImpl()
        {
            VariableNodeUByte node = new VariableNodeUByte
            {
                Value = Value
            };
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

        public VariableNodeUShort()
        { }
        public VariableNodeUShort(XmlNode node) : base(node) { }

        protected override void InitializeSlots()
        {
            ControlType = VariableControlType.Numeric;
            base.InitializeSlots();
        }

        protected override SequenceNode CopyImpl()
        {
            VariableNodeUShort node = new VariableNodeUShort
            {
                Value = Value
            };
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

        public VariableNodeUInt()
        { }
        public VariableNodeUInt(XmlNode node) : base(node) { }

        protected override void InitializeSlots()
        {
            ControlType = VariableControlType.Numeric;
            base.InitializeSlots();
        }

        protected override SequenceNode CopyImpl()
        {
            VariableNodeUInt node = new VariableNodeUInt
            {
                Value = Value
            };
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

        public VariableNodeULong()
        { }
        public VariableNodeULong(XmlNode node) : base(node) { }

        protected override void InitializeSlots()
        {
            ControlType = VariableControlType.Numeric;
            base.InitializeSlots();
        }

        protected override SequenceNode CopyImpl()
        {
            VariableNodeULong node = new VariableNodeULong
            {
                Value = Value
            };
            return node;
        }
    }
}
