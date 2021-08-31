using System;
using System.Text;
using System.Xml;
using FlowGraphBase.Logger;
using FlowGraphBase.Process;

namespace FlowGraphBase.Node.StandardActionNode
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MathOperatorNode<T> : ActionNode
    {
        public enum NodeSlotId
        {
            In,
            Out,
            VarA,
            VarB,
            VarResult
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public MathOperatorNode(XmlNode node)
            : base(node) { }

        /// <summary>
        /// 
        /// </summary>
        public MathOperatorNode()
        { }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitializeSlots()
        {
            base.InitializeSlots();

            AddSlot((int)NodeSlotId.In, "", SlotType.NodeIn);
            AddSlot((int)NodeSlotId.Out, "", SlotType.NodeOut);

            AddSlot((int)NodeSlotId.VarA, "A", SlotType.VarIn, typeof(T));
            AddSlot((int)NodeSlotId.VarB, "B", SlotType.VarIn, typeof(T));
            AddSlot((int)NodeSlotId.VarResult, "Result", SlotType.VarOut, typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public abstract T DoActivateLogic(T a, T b);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot)
        {
            ProcessingInfo info = new ProcessingInfo
            {
                State = LogicState.Ok
            };

            object objA = GetValueFromSlot((int)NodeSlotId.VarA);
            object objB = GetValueFromSlot((int)NodeSlotId.VarB);

            if (objA == null)
            {
                info.State = LogicState.Warning;
                info.ErrorMessage = "Please connect a variable node into the slot A";
                LogManager.Instance.WriteLine(LogVerbosity.Warning,
                    "{0} : Addition failed. {1}.",
                    Title, info.ErrorMessage);
            }
            else if (objB == null)
            {
                info.State = LogicState.Warning;
                info.ErrorMessage = "Please connect a variable node into the slot B";

                LogManager.Instance.WriteLine(LogVerbosity.Warning,
                    "{0} : Addition failed. {1}.",
                    Title, info.ErrorMessage);
            }

            if (objA != null
                && objB != null)
            {
                SetValueInSlot((int)NodeSlotId.VarResult, DoActivateLogic((T)objA, (T)objB));
            }

            ActivateOutputLink(context, (int)NodeSlotId.Out);

            return info;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Category("Maths/Addition")]
    public abstract class AdditionNode<T> : MathOperatorNode<T>
    {
        public AdditionNode(XmlNode node) : base(node) { }
        public AdditionNode()
        { }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public override T DoActivateLogic(T a, T b)
        {
            dynamic x = a;
            dynamic y = b;
            return x + y;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Byte")]
    public class AdditionNodeByte : AdditionNode<sbyte>
    {
        public override string Title => "Addition Byte";

        public AdditionNodeByte()
        { }
        public AdditionNodeByte(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new AdditionNodeByte(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Short")]
    public class AdditionNodeShort : AdditionNode<short>
    {
        public override string Title => "Addition Short";

        public AdditionNodeShort()
        { }
        public AdditionNodeShort(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new AdditionNodeShort(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Integer")]
    public class AdditionNodeInt : AdditionNode<int>
    {
        public override string Title => "Addition Integer";

        public AdditionNodeInt()
        { }
        public AdditionNodeInt(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new AdditionNodeInt(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Long")]
    public class AdditionNodeLong : AdditionNode<long>
    {
        public override string Title => "Addition Long";

        public AdditionNodeLong()
        { }
        public AdditionNodeLong(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new AdditionNodeLong(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Float")]
    public class AdditionNodeFloat : AdditionNode<float>
    {
        public override string Title => "Addition Float";

        public AdditionNodeFloat()
        { }
        public AdditionNodeFloat(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new AdditionNodeFloat(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Double")]
    public class AdditionNodeDouble : AdditionNode<double>
    {
        public override string Title => "Addition Double";

        public AdditionNodeDouble()
        { }
        public AdditionNodeDouble(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new AdditionNodeDouble(); }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Category("Maths/Substraction")]
    public abstract class SubstractionNode<T> : MathOperatorNode<T>
    {
        public SubstractionNode(XmlNode node) : base(node) { }
        public SubstractionNode()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public override T DoActivateLogic(T a, T b)
        {
            dynamic x= a;
            dynamic y = b;
            return x - y;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Byte")]
    public class SubstractionNodeByte : SubstractionNode<sbyte>
    {
        public override string Title => "Substraction Byte";

        public SubstractionNodeByte()
        { }
        public SubstractionNodeByte(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new SubstractionNodeByte(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Short")]
    public class SubstractionNodeShort : SubstractionNode<short>
    {
        public override string Title => "Substraction Short";

        public SubstractionNodeShort()
        { }
        public SubstractionNodeShort(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new SubstractionNodeShort(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Integer")]
    public class SubstractionNodeInt : SubstractionNode<int>
    {
        public override string Title => "Substraction Integer";

        public SubstractionNodeInt()
        { }
        public SubstractionNodeInt(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new SubstractionNodeInt(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Long")]
    public class SubstractionNodeLong : SubstractionNode<long>
    {
        public override string Title => "Substraction Long";

        public SubstractionNodeLong()
        { }
        public SubstractionNodeLong(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new SubstractionNodeLong(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Float")]
    public class SubstractionNodeFloat : SubstractionNode<float>
    {
        public override string Title => "Substraction Float";

        public SubstractionNodeFloat()
        { }
        public SubstractionNodeFloat(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new SubstractionNodeFloat(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Double")]
    public class SubstractionNodeDouble : SubstractionNode<double>
    {
        public override string Title => "Substraction Double";

        public SubstractionNodeDouble()
        { }
        public SubstractionNodeDouble(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new SubstractionNodeDouble(); }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Category("Maths/Multiplication")]
    public abstract class MultiplicationNode<T> : MathOperatorNode<T>
    {
        public MultiplicationNode(XmlNode node) : base(node) { }
        public MultiplicationNode()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public override T DoActivateLogic(T a, T b)
        {
            dynamic x = a;
            dynamic y = b;
            return x * y;
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    [Name("Byte")]
    public class MultiplicationNodeByte : MultiplicationNode<sbyte>
    {
        public override string Title => "Multiplication Byte";

        public MultiplicationNodeByte()
        { }
        public MultiplicationNodeByte(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new MultiplicationNodeByte(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Short")]
    public class MultiplicationNodeShort : MultiplicationNode<short>
    {
        public override string Title => "Multiplication Short";

        public MultiplicationNodeShort()
        { }
        public MultiplicationNodeShort(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new MultiplicationNodeShort(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Integer")]
    public class MultiplicationNodeInt : MultiplicationNode<int>
    {
        public override string Title => "Multiplication Integer";

        public MultiplicationNodeInt()
        { }
        public MultiplicationNodeInt(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new MultiplicationNodeInt(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Long")]
    public class MultiplicationNodeLong : MultiplicationNode<long>
    {
        public override string Title => "Multiplication Long";

        public MultiplicationNodeLong()
        { }
        public MultiplicationNodeLong(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new MultiplicationNodeLong(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Float")]
    public class MultiplicationNodeFloat : MultiplicationNode<float>
    {
        public override string Title => "Multiplication Float";

        public MultiplicationNodeFloat()
        { }
        public MultiplicationNodeFloat(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new MultiplicationNodeFloat(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Double")]
    public class MultiplicationNodeDouble : MultiplicationNode<double>
    {
        public override string Title => "Multiplication Double";

        public MultiplicationNodeDouble()
        { }
        public MultiplicationNodeDouble(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new MultiplicationNodeDouble(); }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Category("Maths/Division")]
    public abstract class DivisionNode<T> : MathOperatorNode<T>
    {
        public DivisionNode(XmlNode node) : base(node) { }
        public DivisionNode()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public override T DoActivateLogic(T a, T b)
        {
            dynamic x = a;
            dynamic y = b;
            return x / y;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Byte")]
    public class DivisionNodeByte : DivisionNode<sbyte>
    {
        public override string Title => "Division Byte";

        public DivisionNodeByte()
        { }
        public DivisionNodeByte(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new DivisionNodeByte(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Short")]
    public class DivisionNodeShort : DivisionNode<short>
    {
        public override string Title => "Division Short";

        public DivisionNodeShort()
        { }
        public DivisionNodeShort(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new DivisionNodeShort(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Integer")]
    public class DivisionNodeInt : DivisionNode<int>
    {
        public override string Title => "Division Integer";

        public DivisionNodeInt()
        { }
        public DivisionNodeInt(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new DivisionNodeInt(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Long")]
    public class DivisionNodeLong : DivisionNode<long>
    {
        public override string Title => "Division Long";

        public DivisionNodeLong()
        { }
        public DivisionNodeLong(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new DivisionNodeLong(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Float")]
    public class DivisionNodeFloat : DivisionNode<float>
    {
        public override string Title => "Division Float";

        public DivisionNodeFloat()
        { }
        public DivisionNodeFloat(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new DivisionNodeFloat(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Double")]
    public class DivisionNodeDouble : DivisionNode<double>
    {
        public override string Title => "Division Double";

        public DivisionNodeDouble()
        { }
        public DivisionNodeDouble(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new DivisionNodeDouble(); }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MathLogicOperatorNode<T> : ActionNode
    {
        public enum NodeSlotId
        {
            In,
            OutTrue,
            OutFalse,
            VarA,
            VarB
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public MathLogicOperatorNode(XmlNode node)
            : base(node) { }

        /// <summary>
        /// 
        /// </summary>
        public MathLogicOperatorNode()
        { }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitializeSlots()
        {
            base.InitializeSlots();

            AddSlot((int)NodeSlotId.In, "", SlotType.NodeIn);
            AddSlot((int)NodeSlotId.OutTrue, "True", SlotType.NodeOut);
            AddSlot((int)NodeSlotId.OutFalse, "False", SlotType.NodeOut);

            AddSlot((int)NodeSlotId.VarA, "A", SlotType.VarIn, typeof(T));
            AddSlot((int)NodeSlotId.VarB, "B", SlotType.VarIn, typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public abstract bool DoActivateLogic(T a, T b);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot)
        {
            ProcessingInfo info = new ProcessingInfo
            {
                State = LogicState.Ok
            };

            object objA = GetValueFromSlot((int)NodeSlotId.VarA);
            object objB = GetValueFromSlot((int)NodeSlotId.VarB);

            if (objA == null)
            {
                info.State = LogicState.Error;
                info.ErrorMessage = "Please connect a variable node into the slot A";
 
                LogManager.Instance.WriteLine(LogVerbosity.Warning,
                    "{0} : {1}.",
                    Title, info.ErrorMessage);
            }
            else if (objB == null)
            {
                info.State = LogicState.Error;
                info.ErrorMessage = "Please connect a variable node into the slot B";

                LogManager.Instance.WriteLine(LogVerbosity.Warning,
                    "{0} : {1}.",
                    Title, info.ErrorMessage);
            }

            if (objA != null
                && objB != null)
            {
                bool res = DoActivateLogic((T)objA, (T)objB);

                if (res) ActivateOutputLink(context, (int)NodeSlotId.OutTrue);
                else ActivateOutputLink(context, (int)NodeSlotId.OutFalse);
            }

            return info;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Category("Maths/Logic/Equal")]
    public abstract class EqualNode<T> : MathLogicOperatorNode<T>
    {
        public EqualNode(XmlNode node) : base(node) { }
        public EqualNode()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public override bool DoActivateLogic(T a, T b)
        {
            dynamic x = a;
            dynamic y = b;
            return x == y;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Byte")]
    public class EqualNodeByte : EqualNode<sbyte>
    {
        public override string Title => "Equal Byte";

        public EqualNodeByte()
        { }
        public EqualNodeByte(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new EqualNodeByte(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Short")]
    public class EqualNodeShort : EqualNode<short>
    {
        public override string Title => "Equal Short";

        public EqualNodeShort()
        { }
        public EqualNodeShort(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new EqualNodeShort(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Integer")]
    public class EqualNodeInt : EqualNode<int>
    {
        public override string Title => "Equal Integer";

        public EqualNodeInt()
        { }
        public EqualNodeInt(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new EqualNodeInt(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Long")]
    public class EqualNodeLong : EqualNode<long>
    {
        public override string Title => "Equal Long";

        public EqualNodeLong()
        { }
        public EqualNodeLong(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new EqualNodeLong(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Float")]
    public class EqualNodeFloat : EqualNode<float>
    {
        public override string Title => "Equal Float";

        public EqualNodeFloat()
        { }
        public EqualNodeFloat(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new EqualNodeFloat(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Double")]
    public class EqualNodeDouble : EqualNode<double>
    {
        public override string Title => "Equal Double";

        public EqualNodeDouble()
        { }
        public EqualNodeDouble(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new EqualNodeDouble(); }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Category("Maths/Logic/NotEqual")]
    public abstract class NotEqualNode<T> : MathLogicOperatorNode<T>
    {
        public NotEqualNode(XmlNode node) : base(node) { }
        public NotEqualNode()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public override bool DoActivateLogic(T a, T b)
        {
            dynamic x = a;
            dynamic y = b;
            return x != y;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Byte")]
    public class NotEqualNodeByte : NotEqualNode<sbyte>
    {
        public override string Title => "NotEqual Byte";

        public NotEqualNodeByte()
        { }
        public NotEqualNodeByte(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new NotEqualNodeByte(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Short")]
    public class NotEqualNodeShort : NotEqualNode<short>
    {
        public override string Title => "NotEqual Short";

        public NotEqualNodeShort()
        { }
        public NotEqualNodeShort(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new NotEqualNodeShort(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Integer")]
    public class NotEqualNodeInt : NotEqualNode<int>
    {
        public override string Title => "NotEqual Integer";

        public NotEqualNodeInt()
        { }
        public NotEqualNodeInt(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new NotEqualNodeInt(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Long")]
    public class NotEqualNodeLong : NotEqualNode<long>
    {
        public override string Title => "NotEqual Long";

        public NotEqualNodeLong()
        { }
        public NotEqualNodeLong(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new NotEqualNodeLong(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Float")]
    public class NotEqualNodeFloat : NotEqualNode<float>
    {
        public override string Title => "NotEqual Float";

        public NotEqualNodeFloat()
        { }
        public NotEqualNodeFloat(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new NotEqualNodeFloat(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Double")]
    public class NotEqualNodeDouble : NotEqualNode<double>
    {
        public override string Title => "NotEqual Double";

        public NotEqualNodeDouble()
        { }
        public NotEqualNodeDouble(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new NotEqualNodeDouble(); }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Category("Maths/Logic/LessThan")]
    public abstract class LessThanNode<T> : MathLogicOperatorNode<T>
    {
        public LessThanNode(XmlNode node) : base(node) { }
        public LessThanNode()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public override bool DoActivateLogic(T a, T b)
        {
            dynamic x = a;
            dynamic y = b;
            return x < y;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Byte")]
    public class LessThanNodeByte : LessThanNode<sbyte>
    {
        public override string Title => "LessThan Byte";

        public LessThanNodeByte()
        { }
        public LessThanNodeByte(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new LessThanNodeByte(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Short")]
    public class LessThanNodeShort : LessThanNode<short>
    {
        public override string Title => "LessThan Short";

        public LessThanNodeShort()
        { }
        public LessThanNodeShort(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new LessThanNodeShort(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Integer")]
    public class LessThanNodeInt : LessThanNode<int>
    {
        public override string Title => "LessThan Integer";

        public LessThanNodeInt()
        { }
        public LessThanNodeInt(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new LessThanNodeInt(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Long")]
    public class LessThanNodeLong : LessThanNode<long>
    {
        public override string Title => "LessThan Long";

        public LessThanNodeLong()
        { }
        public LessThanNodeLong(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new LessThanNodeLong(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Float")]
    public class LessThanNodeFloat : LessThanNode<float>
    {
        public override string Title => "LessThan Float";

        public LessThanNodeFloat()
        { }
        public LessThanNodeFloat(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new LessThanNodeFloat(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Double")]
    public class LessThanNodeDouble : LessThanNode<double>
    {
        public override string Title => "LessThan Double";

        public LessThanNodeDouble()
        { }
        public LessThanNodeDouble(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new LessThanNodeDouble(); }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Category("Maths/Logic/LessThanOrEqual")]
    public abstract class LessThanOrEqualNode<T> : MathLogicOperatorNode<T>
    {
        public LessThanOrEqualNode(XmlNode node) : base(node) { }
        public LessThanOrEqualNode()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public override bool DoActivateLogic(T a, T b)
        {
            dynamic x = a;
            dynamic y = b;
            return x <= y;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Byte")]
    public class LessThanOrEqualNodeByte : LessThanOrEqualNode<sbyte>
    {
        public override string Title => "LessThanOrEqual Byte";

        public LessThanOrEqualNodeByte()
        { }
        public LessThanOrEqualNodeByte(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new LessThanOrEqualNodeByte(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Short")]
    public class LessThanOrEqualNodeShort : LessThanOrEqualNode<short>
    {
        public override string Title => "LessThanOrEqual Short";

        public LessThanOrEqualNodeShort()
        { }
        public LessThanOrEqualNodeShort(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new LessThanOrEqualNodeShort(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Integer")]
    public class LessThanOrEqualNodeInt : LessThanOrEqualNode<int>
    {
        public override string Title => "LessThanOrEqual Integer";

        public LessThanOrEqualNodeInt()
        { }
        public LessThanOrEqualNodeInt(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new LessThanOrEqualNodeInt(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Long")]
    public class LessThanOrEqualNodeLong : LessThanOrEqualNode<long>
    {
        public override string Title => "LessThanOrEqual Long";

        public LessThanOrEqualNodeLong()
        { }
        public LessThanOrEqualNodeLong(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new LessThanOrEqualNodeLong(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Float")]
    public class LessThanOrEqualNodeFloat : LessThanOrEqualNode<float>
    {
        public override string Title => "LessThanOrEqual Float";

        public LessThanOrEqualNodeFloat()
        { }
        public LessThanOrEqualNodeFloat(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new LessThanOrEqualNodeFloat(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Double")]
    public class LessThanOrEqualNodeDouble : LessThanOrEqualNode<double>
    {
        public override string Title => "LessThanOrEqual Double";

        public LessThanOrEqualNodeDouble()
        { }
        public LessThanOrEqualNodeDouble(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new LessThanOrEqualNodeDouble(); }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Category("Maths/Logic/GreaterThan")]
    public abstract class GreaterThanNode<T> : MathLogicOperatorNode<T>
    {
        public GreaterThanNode(XmlNode node) : base(node) { }
        public GreaterThanNode()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public override bool DoActivateLogic(T a, T b)
        {
            dynamic x = a;
            dynamic y = b;
            return x > y;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Byte")]
    public class GreaterThanNodeByte : GreaterThanNode<sbyte>
    {
        public override string Title => "GreaterThan Byte";

        public GreaterThanNodeByte()
        { }
        public GreaterThanNodeByte(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new GreaterThanNodeByte(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Short")]
    public class GreaterThanNodeShort : GreaterThanNode<short>
    {
        public override string Title => "GreaterThan Short";

        public GreaterThanNodeShort()
        { }
        public GreaterThanNodeShort(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new GreaterThanNodeShort(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Integer")]
    public class GreaterThanNodeInt : GreaterThanNode<int>
    {
        public override string Title => "GreaterThan Integer";

        public GreaterThanNodeInt()
        { }
        public GreaterThanNodeInt(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new GreaterThanNodeInt(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Long")]
    public class GreaterThanNodeLong : GreaterThanNode<long>
    {
        public override string Title => "GreaterThan Long";

        public GreaterThanNodeLong()
        { }
        public GreaterThanNodeLong(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new GreaterThanNodeLong(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Float")]
    public class GreaterThanNodeFloat : GreaterThanNode<float>
    {
        public override string Title => "GreaterThan Float";

        public GreaterThanNodeFloat()
        { }
        public GreaterThanNodeFloat(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new GreaterThanNodeFloat(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Double")]
    public class GreaterThanNodeDouble : GreaterThanNode<double>
    {
        public override string Title => "GreaterThan Double";

        public GreaterThanNodeDouble()
        { }
        public GreaterThanNodeDouble(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new GreaterThanNodeDouble(); }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Category("Maths/Logic/GreaterThanOrEqual")]
    public abstract class GreaterThanOrEqualNode<T> : MathLogicOperatorNode<T>
    {
        public GreaterThanOrEqualNode(XmlNode node) : base(node) { }
        public GreaterThanOrEqualNode()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public override bool DoActivateLogic(T a, T b)
        {
            dynamic x = a;
            dynamic y = b;
            return x >= y;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Byte")]
    public class GreaterThanOrEqualNodeByte : GreaterThanOrEqualNode<sbyte>
    {
        public override string Title => "GreaterThanOrEqual Byte";

        public GreaterThanOrEqualNodeByte()
        { }
        public GreaterThanOrEqualNodeByte(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new GreaterThanOrEqualNodeByte(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Short")]
    public class GreaterThanOrEqualNodeShort : GreaterThanOrEqualNode<short>
    {
        public override string Title => "GreaterThanOrEqual Short";

        public GreaterThanOrEqualNodeShort()
        { }
        public GreaterThanOrEqualNodeShort(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new GreaterThanOrEqualNodeShort(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Integer")]
    public class GreaterThanOrEqualNodeInt : GreaterThanOrEqualNode<int>
    {
        public override string Title => "GreaterThanOrEqual Integer";

        public GreaterThanOrEqualNodeInt()
        { }
        public GreaterThanOrEqualNodeInt(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new GreaterThanOrEqualNodeInt(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Long")]
    public class GreaterThanOrEqualNodeLong : GreaterThanOrEqualNode<long>
    {
        public override string Title => "GreaterThanOrEqual Long";

        public GreaterThanOrEqualNodeLong()
        { }
        public GreaterThanOrEqualNodeLong(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new GreaterThanOrEqualNodeLong(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Float")]
    public class GreaterThanOrEqualNodeFloat : GreaterThanOrEqualNode<float>
    {
        public override string Title => "GreaterThanOrEqual Float";

        public GreaterThanOrEqualNodeFloat()
        { }
        public GreaterThanOrEqualNodeFloat(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new GreaterThanOrEqualNodeFloat(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Double")]
    public class GreaterThanOrEqualNodeDouble : GreaterThanOrEqualNode<double>
    {
        public override string Title => "GreaterThanOrEqual Double";

        public GreaterThanOrEqualNodeDouble()
        { }
        public GreaterThanOrEqualNodeDouble(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new GreaterThanOrEqualNodeDouble(); }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MathCastOperatorNode<TIn, TOut> : ActionNode
    {
        public enum NodeSlotId
        {
            In,
            Out,
            VarA,
            VarResult
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public MathCastOperatorNode(XmlNode node)
            : base(node)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public MathCastOperatorNode()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitializeSlots()
        {
            base.InitializeSlots();

            AddSlot((int)NodeSlotId.In, "", SlotType.NodeIn);
            AddSlot((int)NodeSlotId.Out, "", SlotType.NodeOut);

            AddSlot((int)NodeSlotId.VarA, "A", SlotType.VarIn, typeof(TIn), false);
            AddSlot((int)NodeSlotId.VarResult, "Result", SlotType.VarOut, typeof(TOut), false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public abstract TOut DoActivateLogic(TIn a);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot)
        {
            ProcessingInfo info = new ProcessingInfo
            {
                State = LogicState.Ok
            };

            object objA = GetValueFromSlot((int)NodeSlotId.VarA);

            if (objA == null)
            {
                info.State = LogicState.Warning;
                info.ErrorMessage = "Please connect a variable node into the slot A";

                LogManager.Instance.WriteLine(LogVerbosity.Warning,
                    "{0} : {1}.",
                    Title, info.ErrorMessage);
            }

            if (objA != null)
            {
                SetValueInSlot((int)NodeSlotId.VarResult, DoActivateLogic((TIn)objA));
            }

            ActivateOutputLink(context, (int)NodeSlotId.Out);

            return info;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Category("Maths/Cast/To String")]
    public abstract class ToStringNode<TIn> : MathCastOperatorNode<TIn, string>
    {
        public ToStringNode(XmlNode node) : base(node) { }
        public ToStringNode()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public override string DoActivateLogic(TIn a)
        {
            dynamic x = a;
            return x.ToString();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Byte to String")]
    public class ToStringNodeByte : ToStringNode<sbyte>
    {
        public override string Title => "Byte to String";

        public ToStringNodeByte()
        { }
        public ToStringNodeByte(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new ToStringNodeByte(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Char to String")]
    public class ToStringNodeChar : ToStringNode<char>
    {
        public override string Title => "Char to String";

        public ToStringNodeChar()
        { }
        public ToStringNodeChar(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new ToStringNodeChar(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Short to String")]
    public class ToStringNodeShort : ToStringNode<short>
    {
        public override string Title => "Short to String";

        public ToStringNodeShort()
        { }
        public ToStringNodeShort(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new ToStringNodeShort(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Integer to String")]
    public class ToStringNodeInteger : ToStringNode<int>
    {
        public override string Title => "Integer to String";

        public ToStringNodeInteger()
        { }
        public ToStringNodeInteger(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new ToStringNodeInteger(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Long to String")]
    public class ToStringNodeLong : ToStringNode<long>
    {
        public override string Title => "Long to String";

        public ToStringNodeLong()
        { }
        public ToStringNodeLong(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new ToStringNodeLong(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Float to String")]
    public class ToStringNodeFloat : ToStringNode<float>
    {
        public override string Title => "Float to String";

        public ToStringNodeFloat()
        { }
        public ToStringNodeFloat(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new ToStringNodeFloat(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Double to String")]
    public class ToStringNodeDouble : ToStringNode<double>
    {
        public override string Title => "Double to String";

        public ToStringNodeDouble()
        { }
        public ToStringNodeDouble(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new ToStringNodeDouble(); }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Category("Maths/Cast/To Integer")]
    public abstract class ToIntegerNode<TIn> : MathCastOperatorNode<TIn, int>
    {
        public ToIntegerNode(XmlNode node) : base(node) { }
        public ToIntegerNode()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public override int DoActivateLogic(TIn a)
        {
            dynamic x = a;
            return (int) Convert.ChangeType(x, typeof(int));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Byte to Integer")]
    public class ToIntegerNodeByte : ToIntegerNode<sbyte>
    {
        public override string Title => "Byte to Integer";

        public ToIntegerNodeByte()
        { }
        public ToIntegerNodeByte(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new ToIntegerNodeByte(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Char to Integer")]
    public class ToIntegerNodeChar : ToIntegerNode<char>
    {
        public override string Title => "Char to Integer";

        public ToIntegerNodeChar()
        { }
        public ToIntegerNodeChar(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new ToIntegerNodeChar(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Short to Integer")]
    public class ToIntegerNodeShort : ToIntegerNode<short>
    {
        public override string Title => "Short to Integer";

        public ToIntegerNodeShort()
        { }
        public ToIntegerNodeShort(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new ToIntegerNodeShort(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Long to Integer")]
    public class ToIntegerNodeLong : ToIntegerNode<long>
    {
        public override string Title => "Long to Integer";

        public ToIntegerNodeLong()
        { }
        public ToIntegerNodeLong(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new ToIntegerNodeLong(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Float to Integer")]
    public class ToIntegerNodeFloat : ToIntegerNode<float>
    {
        public override string Title => "Float to Integer";

        public ToIntegerNodeFloat()
        { }
        public ToIntegerNodeFloat(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new ToIntegerNodeFloat(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Double to Integer")]
    public class ToIntegerNodeDouble : ToIntegerNode<double>
    {
        public override string Title => "Double to Integer";

        public ToIntegerNodeDouble()
        { }
        public ToIntegerNodeDouble(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new ToIntegerNodeDouble(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("String to Integer")]
    public class ToIntegerNodeString : ToIntegerNode<string>
    {
        public override string Title => "Short to Integer";

        public ToIntegerNodeString()
        { }
        public ToIntegerNodeString(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new ToIntegerNodeString(); }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Category("Maths/Cast/To Double")]
    public abstract class ToDoubleNode<TIn> : MathCastOperatorNode<TIn, double>
    {
        public ToDoubleNode(XmlNode node) : base(node) { }
        public ToDoubleNode()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public override double DoActivateLogic(TIn a)
        {
            dynamic x = a;
            return (double)Convert.ChangeType(x, typeof(double));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Byte to Double")]
    public class ToDoubleNodeByte : ToDoubleNode<sbyte>
    {
        public override string Title => "Byte to Double";

        public ToDoubleNodeByte()
        { }
        public ToDoubleNodeByte(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new ToDoubleNodeByte(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Char to Double")]
    public class ToDoubleNodeChar : ToDoubleNode<char>
    {
        public override string Title => "Char to Double";

        public ToDoubleNodeChar()
        { }
        public ToDoubleNodeChar(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new ToDoubleNodeChar(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Short to Double")]
    public class ToDoubleNodeShort : ToDoubleNode<short>
    {
        public override string Title => "Short to Double";

        public ToDoubleNodeShort()
        { }
        public ToDoubleNodeShort(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new ToDoubleNodeShort(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Integer to Double")]
    public class ToDoubleNodeInteger : ToDoubleNode<int>
    {
        public override string Title => "Integer to Double";

        public ToDoubleNodeInteger()
        { }
        public ToDoubleNodeInteger(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new ToDoubleNodeInteger(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Long to Double")]
    public class ToDoubleNodeLong : ToDoubleNode<long>
    {
        public override string Title => "Long to Double";

        public ToDoubleNodeLong()
        { }
        public ToDoubleNodeLong(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new ToDoubleNodeLong(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Float to Double")]
    public class ToDoubleNodeFloat : ToDoubleNode<float>
    {
        public override string Title => "FLoat to Double";

        public ToDoubleNodeFloat()
        { }
        public ToDoubleNodeFloat(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new ToDoubleNodeFloat(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("String to Double")]
    public class ToDoubleNodeString : ToDoubleNode<string>
    {
        public override string Title => "String to Double";

        public ToDoubleNodeString()
        { }
        public ToDoubleNodeString(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new ToDoubleNodeString(); }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Category("Maths/Cast/To Object")]
    public abstract class ToObjectNode<TIn> : MathCastOperatorNode<TIn, object>
    {
        public ToObjectNode(XmlNode node) : base(node) { }
        public ToObjectNode()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public override object DoActivateLogic(TIn a)
        {
            dynamic x = a;
            return Convert.ChangeType(x, typeof(object));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Byte to Object")]
    public class ToObjectNodeByte : ToObjectNode<sbyte>
    {
        public override string Title => "Byte to Object";

        public ToObjectNodeByte()
        { }
        public ToObjectNodeByte(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new ToObjectNodeByte(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Char to Object")]
    public class ToObjectNodeChar : ToObjectNode<char>
    {
        public override string Title => "Char to Object";

        public ToObjectNodeChar()
        { }
        public ToObjectNodeChar(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new ToObjectNodeChar(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Short to Object")]
    public class ToObjectNodeShort : ToObjectNode<short>
    {
        public override string Title => "Short to Object";

        public ToObjectNodeShort()
        { }
        public ToObjectNodeShort(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new ToObjectNodeShort(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Integer to Object")]
    public class ToObjectNodeInteger : ToObjectNode<int>
    {
        public override string Title => "Integer to Object";

        public ToObjectNodeInteger()
        { }
        public ToObjectNodeInteger(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new ToObjectNodeInteger(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Long to Object")]
    public class ToObjectNodeLong : ToObjectNode<long>
    {
        public override string Title => "Long to Object";

        public ToObjectNodeLong()
        { }
        public ToObjectNodeLong(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new ToObjectNodeLong(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Float to Object")]
    public class ToObjectNodeFloat : ToObjectNode<float>
    {
        public override string Title => "Float to Object";

        public ToObjectNodeFloat()
        { }
        public ToObjectNodeFloat(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new ToObjectNodeFloat(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Double to Object")]
    public class ToObjectNodeDouble : ToObjectNode<double>
    {
        public override string Title => "Double to Object";

        public ToObjectNodeDouble()
        { }
        public ToObjectNodeDouble(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new ToObjectNodeDouble(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("String to Object")]
    public class ToObjectNodeString : ToObjectNode<string>
    {
        public override string Title => "String to Object";

        public ToObjectNodeString()
        { }
        public ToObjectNodeString(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new ToObjectNodeString(); }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Category("Maths/Random")]
    public abstract class MathRandomNode<T> : ActionNode
    {
        public enum NodeSlotId
        {
            In,
            Out,
            VarMin,
            VarMax,
            VarResult
        }

        static readonly Random Random = new Random();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public MathRandomNode(XmlNode node)
            : base(node) { }

        /// <summary>
        /// 
        /// </summary>
        public MathRandomNode()
        { }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitializeSlots()
        {
            base.InitializeSlots();

            AddSlot((int)NodeSlotId.In, "", SlotType.NodeIn);
            AddSlot((int)NodeSlotId.Out, "", SlotType.NodeOut);

            AddSlot((int)NodeSlotId.VarMin, "Min", SlotType.VarIn, typeof(T));
            AddSlot((int)NodeSlotId.VarMax, "Max", SlotType.VarIn, typeof(T));
            AddSlot((int)NodeSlotId.VarResult, "Result", SlotType.VarOut, typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="min_"></param>
        /// <param name="max_"></param>
        /// <returns></returns>
        //public abstract T DoActivateLogic(T min_, T max_);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot)
        {
            ProcessingInfo info = new ProcessingInfo
            {
                State = LogicState.Ok
            };

            object objMin = GetValueFromSlot((int)NodeSlotId.VarMin);

            if (objMin == null)
            {
                info.ErrorMessage = "Please connect a variable node into the slot Min";
                info.State = LogicState.Warning;

                LogManager.Instance.WriteLine(LogVerbosity.Warning,
                    "{0} : {1}.",
                    Title, info.ErrorMessage);
            }

            object objMax = GetValueFromSlot((int)NodeSlotId.VarMax);

            if (objMax == null)
            {
                info.ErrorMessage = "Please connect a variable node into the slot Max";
                info.State = LogicState.Warning;

                LogManager.Instance.WriteLine(LogVerbosity.Warning,
                    "{0} : Random failed. {1}.",
                    Title, info.ErrorMessage);
            }

            if (objMin != null && objMax != null)
            {
                object result;
                Type typeVal = typeof(T);

                if (typeVal == typeof(double)
                    || typeVal == typeof(float))
                {
                    result = Random.NextDouble();

                    dynamic min = objMin;
                    dynamic max = objMax;
                    result = min + (T)result * (max - min);
                }
                else
                {
                    result = Random.Next((int)objMin, (int)objMax);
                }

                SetValueInSlot((int)NodeSlotId.VarResult, result);
            }

            ActivateOutputLink(context, (int)NodeSlotId.Out);

            return info;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Random Byte")]
    public class RandomByteNode : MathRandomNode<sbyte>
    {
        public override string Title => "Random Byte";

        public RandomByteNode()
        { }
        public RandomByteNode(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new RandomByteNode(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Random Short")]
    public class RandomShortNode : MathRandomNode<short>
    {
        public override string Title => "Random Short";

        public RandomShortNode()
        { }
        public RandomShortNode(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new RandomShortNode(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Random Integer")]
    public class RandomIntegerNode : MathRandomNode<int>
    {
        public override string Title => "Random Integer";

        public RandomIntegerNode()
        { }
        public RandomIntegerNode(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new RandomIntegerNode(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Random Long")]
    public class RandomLongNode : MathRandomNode<long>
    {
        public override string Title => "Random Long";

        public RandomLongNode()
        { }
        public RandomLongNode(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new RandomLongNode(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Random Float")]
    public class RandomFloatNode : MathRandomNode<float>
    {
        public override string Title => "Random Float";

        public RandomFloatNode()
        { }
        public RandomFloatNode(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new RandomFloatNode(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Random Double")]
    public class RandomDoubleNode : MathRandomNode<double>
    {
        public override string Title => "Random Double";

        public RandomDoubleNode()
        { }
        public RandomDoubleNode(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new RandomDoubleNode(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Category("String")]
    [Name("Concat")]
    public class StringConcatNode : ActionNode
    {
        public enum NodeSlotId
        {
            In,
            Out,
            VarA,
            VarB,
            VarResult
        }

        public override string Title => "String Concat";

        /// <summary>
        /// 
        /// </summary>
        public StringConcatNode()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public StringConcatNode(XmlNode node) : base(node) { }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitializeSlots()
        {
            base.InitializeSlots();

            AddSlot((int)NodeSlotId.In, "", SlotType.NodeIn);
            AddSlot((int)NodeSlotId.Out, "", SlotType.NodeOut);

            AddSlot((int)NodeSlotId.VarA, "A", SlotType.VarIn, typeof(string));
            AddSlot((int)NodeSlotId.VarB, "B", SlotType.VarIn, typeof(string));
            AddSlot((int)NodeSlotId.VarResult, "Result", SlotType.VarOut, typeof(string));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot)
        {
            ProcessingInfo info = new ProcessingInfo
            {
                State = LogicState.Ok
            };

            object objA = GetValueFromSlot((int)NodeSlotId.VarA);

            if (objA == null)
            {
                info.ErrorMessage = "Please connect a variable node into the slot A";
                info.State = LogicState.Warning;

                LogManager.Instance.WriteLine(LogVerbosity.Warning,
                    "{0} : {1}.",
                    Title, info.ErrorMessage);
            }

            object objB = GetValueFromSlot((int)NodeSlotId.VarB);

            if (objB == null)
            {
                info.ErrorMessage = "Please connect a variable node into the slot B";
                info.State = LogicState.Warning;

                LogManager.Instance.WriteLine(LogVerbosity.Warning,
                    "{0} : String Concat failed. {1}.",
                    Title, info.ErrorMessage);
            }

            if (objA != null && objB != null)
            {
                StringBuilder result = new StringBuilder();
                result.Append(objA);
                result.Append(objB);
                SetValueInSlot((int)NodeSlotId.VarResult, result.ToString());
            }

            ActivateOutputLink(context, (int)NodeSlotId.Out);

            return info;
        }

        protected override SequenceNode CopyImpl() { return new StringConcatNode(); }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Category("Variable/Set")]
    public abstract class VariableSetNode<T> : ActionNode
    {
        public enum NodeSlotId
        {
            In,
            Out,
            Variable,
            Value,
            VarResult
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public VariableSetNode(XmlNode node)
            : base(node) { }

        /// <summary>
        /// 
        /// </summary>
        public VariableSetNode()
        { }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitializeSlots()
        {
            base.InitializeSlots();

            AddSlot((int)NodeSlotId.In, "", SlotType.NodeIn);
            AddSlot((int)NodeSlotId.Out, "", SlotType.NodeOut);

            AddSlot((int)NodeSlotId.Variable, "Variable", SlotType.VarIn, typeof(T), false);
            AddSlot((int)NodeSlotId.Value, "Value", SlotType.VarIn, typeof(T));
            AddSlot((int)NodeSlotId.VarResult, "Result", SlotType.VarOut, typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot)
        {
            ProcessingInfo info = new ProcessingInfo
            {
                State = LogicState.Ok
            };

            object objVariable = GetValueFromSlot((int)NodeSlotId.Variable);
            object objValue = GetValueFromSlot((int)NodeSlotId.Value);

            if (objVariable == null)
            {
                info.State = LogicState.Warning;
                info.ErrorMessage = "Please connect a variable node into the slot Variable";
                LogManager.Instance.WriteLine(LogVerbosity.Warning,
                    "{0} : Addition failed. {1}.",
                    Title, info.ErrorMessage);
            }
            else if (objValue == null)
            {
                info.State = LogicState.Warning;
                info.ErrorMessage = "Please connect a variable node into the slot Value";

                LogManager.Instance.WriteLine(LogVerbosity.Warning,
                    "{0} : Addition failed. {1}.",
                    Title, info.ErrorMessage);
            }

            if (objVariable != null
                && objValue != null)
            {
                SetValueInSlot((int)NodeSlotId.Variable, objValue);
                SetValueInSlot((int)NodeSlotId.VarResult, objValue);
            }

            ActivateOutputLink(context, (int)NodeSlotId.Out);

            return info;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Byte")]
    public class VariableSetNodeByte : VariableSetNode<sbyte>
    {
        public override string Title => "Set Byte";

        public VariableSetNodeByte()
        { }
        public VariableSetNodeByte(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new VariableSetNodeByte(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Short")]
    public class VariableSetNodeShort : VariableSetNode<short>
    {
        public override string Title => "Set Short";

        public VariableSetNodeShort()
        { }
        public VariableSetNodeShort(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new VariableSetNodeShort(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Integer")]
    public class VariableSetNodeInt : VariableSetNode<int>
    {
        public override string Title => "Set Integer";

        public VariableSetNodeInt()
        { }
        public VariableSetNodeInt(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new VariableSetNodeInt(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Long")]
    public class VariableSetNodeLong : VariableSetNode<long>
    {
        public override string Title => "Set Long";

        public VariableSetNodeLong()
        { }
        public VariableSetNodeLong(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new VariableSetNodeLong(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Float")]
    public class VariableSetNodeFloat : VariableSetNode<float>
    {
        public override string Title => "Set Float";

        public VariableSetNodeFloat()
        { }
        public VariableSetNodeFloat(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new VariableSetNodeFloat(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Double")]
    public class VariableSetNodeDouble : VariableSetNode<double>
    {
        public override string Title => "Set Double";

        public VariableSetNodeDouble()
        { }
        public VariableSetNodeDouble(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new VariableSetNodeDouble(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("String")]
    public class VariableSetNodeString : VariableSetNode<double>
    {
        public override string Title => "Set String";

        public VariableSetNodeString()
        { }
        public VariableSetNodeString(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new VariableSetNodeString(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Object")]
    public class VariableSetNodeObject : VariableSetNode<object>
    {
        public override string Title => "Set Object";

        public VariableSetNodeObject()
        { }
        public VariableSetNodeObject(XmlNode node) : base(node) { }

        protected override SequenceNode CopyImpl() { return new VariableSetNodeObject(); }
    }
}
