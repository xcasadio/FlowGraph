using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using FlowGraphBase.Logger;
using FlowGraphBase.Process;

namespace FlowGraphBase.Node.StandardActionNode
{
    #region Math standard operator

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MathOperatorNode<T> : ActionNode
    {
        #region Enum

        public enum NodeSlotId
        {
            In,
            Out,
            VarA,
            VarB,
            VarResult
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public MathOperatorNode(XmlNode node_)
            : base(node_) { }

        /// <summary>
        /// 
        /// </summary>
        public MathOperatorNode()
            : base() { }

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
        public abstract T DoActivateLogic(T a_, T b_);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ProcessingInfo ActivateLogic(ProcessingContext context_, NodeSlot slot_)
        {
            ProcessingInfo info = new ProcessingInfo();
            info.State = ActionNode.LogicState.OK;

            object objA = GetValueFromSlot((int)NodeSlotId.VarA);
            object objB = GetValueFromSlot((int)NodeSlotId.VarB);

            if (objA == null)
            {
                info.State = ActionNode.LogicState.Warning;
                info.ErrorMessage = "Please connect a variable node into the slot A";
                LogManager.Instance.WriteLine(LogVerbosity.Warning,
                    "{0} : Addition failed. {1}.",
                    Title, info.ErrorMessage);
            }
            else if (objB == null)
            {
                info.State = ActionNode.LogicState.Warning;
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

            ActivateOutputLink(context_, (int)NodeSlotId.Out);

            return info;
        }
    }

    #region Addition

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Category("Maths/Addition")]
    public abstract class AdditionNode<T> : MathOperatorNode<T>
    {
        public AdditionNode(XmlNode node_) : base(node_) { }
        public AdditionNode() : base() { }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public override T DoActivateLogic(T a_, T b_)
        {
            dynamic A = a_;
            dynamic B = b_;
            return A + B;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Byte")]
    public class AdditionNodeByte : AdditionNode<sbyte>
    {
        public override string Title { get { return "Addition Byte"; } }

        public AdditionNodeByte() : base() { }
        public AdditionNodeByte(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new AdditionNodeByte(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Short")]
    public class AdditionNodeShort : AdditionNode<short>
    {
        public override string Title { get { return "Addition Short"; } }

        public AdditionNodeShort() : base() { }
        public AdditionNodeShort(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new AdditionNodeShort(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Integer")]
    public class AdditionNodeInt : AdditionNode<int>
    {
        public override string Title { get { return "Addition Integer"; } }

        public AdditionNodeInt() : base() { }
        public AdditionNodeInt(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new AdditionNodeInt(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Long")]
    public class AdditionNodeLong : AdditionNode<long>
    {
        public override string Title { get { return "Addition Long"; } }

        public AdditionNodeLong() : base() { }
        public AdditionNodeLong(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new AdditionNodeLong(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Float")]
    public class AdditionNodeFloat : AdditionNode<float>
    {
        public override string Title { get { return "Addition Float"; } }

        public AdditionNodeFloat() : base() { }
        public AdditionNodeFloat(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new AdditionNodeFloat(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Double")]
    public class AdditionNodeDouble : AdditionNode<double>
    {
        public override string Title { get { return "Addition Double"; } }

        public AdditionNodeDouble() : base() { }
        public AdditionNodeDouble(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new AdditionNodeDouble(); }
    }

    #endregion // Addition

    #region Substraction

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Category("Maths/Substraction")]
    public abstract class SubstractionNode<T> : MathOperatorNode<T>
    {
        public SubstractionNode(XmlNode node_) : base(node_) { }
        public SubstractionNode() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public override T DoActivateLogic(T a_, T b_)
        {
            dynamic A = a_;
            dynamic B = b_;
            return A - B;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Byte")]
    public class SubstractionNodeByte : SubstractionNode<sbyte>
    {
        public override string Title { get { return "Substraction Byte"; } }

        public SubstractionNodeByte() : base() { }
        public SubstractionNodeByte(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new SubstractionNodeByte(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Short")]
    public class SubstractionNodeShort : SubstractionNode<short>
    {
        public override string Title { get { return "Substraction Short"; } }

        public SubstractionNodeShort() : base() { }
        public SubstractionNodeShort(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new SubstractionNodeShort(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Integer")]
    public class SubstractionNodeInt : SubstractionNode<int>
    {
        public override string Title { get { return "Substraction Integer"; } }

        public SubstractionNodeInt() : base() { }
        public SubstractionNodeInt(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new SubstractionNodeInt(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Long")]
    public class SubstractionNodeLong : SubstractionNode<long>
    {
        public override string Title { get { return "Substraction Long"; } }

        public SubstractionNodeLong() : base() { }
        public SubstractionNodeLong(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new SubstractionNodeLong(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Float")]
    public class SubstractionNodeFloat : SubstractionNode<float>
    {
        public override string Title { get { return "Substraction Float"; } }

        public SubstractionNodeFloat() : base() { }
        public SubstractionNodeFloat(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new SubstractionNodeFloat(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Double")]
    public class SubstractionNodeDouble : SubstractionNode<double>
    {
        public override string Title { get { return "Substraction Double"; } }

        public SubstractionNodeDouble() : base() { }
        public SubstractionNodeDouble(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new SubstractionNodeDouble(); }
    }

    #endregion // Substraction

    #region Multiplication

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Category("Maths/Multiplication")]
    public abstract class MultiplicationNode<T> : MathOperatorNode<T>
    {
        public MultiplicationNode(XmlNode node_) : base(node_) { }
        public MultiplicationNode() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public override T DoActivateLogic(T a_, T b_)
        {
            dynamic A = a_;
            dynamic B = b_;
            return A * B;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Byte")]
    public class MultiplicationNodeByte : MultiplicationNode<sbyte>
    {
        public override string Title { get { return "Multiplication Byte"; } }

        public MultiplicationNodeByte() : base() { }
        public MultiplicationNodeByte(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new MultiplicationNodeByte(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Short")]
    public class MultiplicationNodeShort : MultiplicationNode<short>
    {
        public override string Title { get { return "Multiplication Short"; } }

        public MultiplicationNodeShort() : base() { }
        public MultiplicationNodeShort(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new MultiplicationNodeShort(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Integer")]
    public class MultiplicationNodeInt : MultiplicationNode<int>
    {
        public override string Title { get { return "Multiplication Integer"; } }

        public MultiplicationNodeInt() : base() { }
        public MultiplicationNodeInt(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new MultiplicationNodeInt(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Long")]
    public class MultiplicationNodeLong : MultiplicationNode<long>
    {
        public override string Title { get { return "Multiplication Long"; } }

        public MultiplicationNodeLong() : base() { }
        public MultiplicationNodeLong(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new MultiplicationNodeLong(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Float")]
    public class MultiplicationNodeFloat : MultiplicationNode<float>
    {
        public override string Title { get { return "Multiplication Float"; } }

        public MultiplicationNodeFloat() : base() { }
        public MultiplicationNodeFloat(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new MultiplicationNodeFloat(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Double")]
    public class MultiplicationNodeDouble : MultiplicationNode<double>
    {
        public override string Title { get { return "Multiplication Double"; } }

        public MultiplicationNodeDouble() : base() { }
        public MultiplicationNodeDouble(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new MultiplicationNodeDouble(); }
    }

    #endregion // Multiplication

    #region Division

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Category("Maths/Division")]
    public abstract class DivisionNode<T> : MathOperatorNode<T>
    {
        public DivisionNode(XmlNode node_) : base(node_) { }
        public DivisionNode() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public override T DoActivateLogic(T a_, T b_)
        {
            dynamic A = a_;
            dynamic B = b_;
            return A / B;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Byte")]
    public class DivisionNodeByte : DivisionNode<sbyte>
    {
        public override string Title { get { return "Division Byte"; } }

        public DivisionNodeByte() : base() { }
        public DivisionNodeByte(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new DivisionNodeByte(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Short")]
    public class DivisionNodeShort : DivisionNode<short>
    {
        public override string Title { get { return "Division Short"; } }

        public DivisionNodeShort() : base() { }
        public DivisionNodeShort(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new DivisionNodeShort(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Integer")]
    public class DivisionNodeInt : DivisionNode<int>
    {
        public override string Title { get { return "Division Integer"; } }

        public DivisionNodeInt() : base() { }
        public DivisionNodeInt(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new DivisionNodeInt(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Long")]
    public class DivisionNodeLong : DivisionNode<long>
    {
        public override string Title { get { return "Division Long"; } }

        public DivisionNodeLong() : base() { }
        public DivisionNodeLong(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new DivisionNodeLong(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Float")]
    public class DivisionNodeFloat : DivisionNode<float>
    {
        public override string Title { get { return "Division Float"; } }

        public DivisionNodeFloat() : base() { }
        public DivisionNodeFloat(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new DivisionNodeFloat(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Double")]
    public class DivisionNodeDouble : DivisionNode<double>
    {
        public override string Title { get { return "Division Double"; } }

        public DivisionNodeDouble() : base() { }
        public DivisionNodeDouble(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new DivisionNodeDouble(); }
    }

    #endregion // Division

    #endregion // Math standard operator

    #region Math logic operator

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MathLogicOperatorNode<T> : ActionNode
    {
        #region Enum

        public enum NodeSlotId
        {
            In,
            OutTrue,
            OutFalse,
            VarA,
            VarB
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public MathLogicOperatorNode(XmlNode node_)
            : base(node_) { }

        /// <summary>
        /// 
        /// </summary>
        public MathLogicOperatorNode()
            : base() { }

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
        public abstract bool DoActivateLogic(T a_, T b_);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ProcessingInfo ActivateLogic(ProcessingContext context_, NodeSlot slot_)
        {
            ProcessingInfo info = new ProcessingInfo();
            info.State = ActionNode.LogicState.OK;

            object objA = GetValueFromSlot((int)NodeSlotId.VarA);
            object objB = GetValueFromSlot((int)NodeSlotId.VarB);

            if (objA == null)
            {
                info.State = ActionNode.LogicState.Error;
                info.ErrorMessage = "Please connect a variable node into the slot A";
 
                LogManager.Instance.WriteLine(LogVerbosity.Warning,
                    "{0} : {1}.",
                    Title, info.ErrorMessage);
            }
            else if (objB == null)
            {
                info.State = ActionNode.LogicState.Error;
                info.ErrorMessage = "Please connect a variable node into the slot B";

                LogManager.Instance.WriteLine(LogVerbosity.Warning,
                    "{0} : {1}.",
                    Title, info.ErrorMessage);
            }

            if (objA != null
                && objB != null)
            {
                bool res = DoActivateLogic((T)objA, (T)objB);

                if (res) ActivateOutputLink(context_, (int)NodeSlotId.OutTrue);
                else ActivateOutputLink(context_, (int)NodeSlotId.OutFalse);
            }

            return info;
        }
    }

    #region Equal operator

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Category("Maths/Logic/Equal")]
    public abstract class EqualNode<T> : MathLogicOperatorNode<T>
    {
        public EqualNode(XmlNode node_) : base(node_) { }
        public EqualNode() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public override bool DoActivateLogic(T a_, T b_)
        {
            dynamic A = a_;
            dynamic B = b_;
            return A == B;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Byte")]
    public class EqualNodeByte : EqualNode<sbyte>
    {
        public override string Title { get { return "Equal Byte"; } }

        public EqualNodeByte() : base() { }
        public EqualNodeByte(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new EqualNodeByte(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Short")]
    public class EqualNodeShort : EqualNode<short>
    {
        public override string Title { get { return "Equal Short"; } }

        public EqualNodeShort() : base() { }
        public EqualNodeShort(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new EqualNodeShort(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Integer")]
    public class EqualNodeInt : EqualNode<int>
    {
        public override string Title { get { return "Equal Integer"; } }

        public EqualNodeInt() : base() { }
        public EqualNodeInt(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new EqualNodeInt(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Long")]
    public class EqualNodeLong : EqualNode<long>
    {
        public override string Title { get { return "Equal Long"; } }

        public EqualNodeLong() : base() { }
        public EqualNodeLong(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new EqualNodeLong(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Float")]
    public class EqualNodeFloat : EqualNode<float>
    {
        public override string Title { get { return "Equal Float"; } }

        public EqualNodeFloat() : base() { }
        public EqualNodeFloat(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new EqualNodeFloat(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Double")]
    public class EqualNodeDouble : EqualNode<double>
    {
        public override string Title { get { return "Equal Double"; } }

        public EqualNodeDouble() : base() { }
        public EqualNodeDouble(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new EqualNodeDouble(); }
    }

    #endregion // Equal

    #region NotEqual operator

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Category("Maths/Logic/NotEqual")]
    public abstract class NotEqualNode<T> : MathLogicOperatorNode<T>
    {
        public NotEqualNode(XmlNode node_) : base(node_) { }
        public NotEqualNode() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public override bool DoActivateLogic(T a_, T b_)
        {
            dynamic A = a_;
            dynamic B = b_;
            return A != B;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Byte")]
    public class NotEqualNodeByte : NotEqualNode<sbyte>
    {
        public override string Title { get { return "NotEqual Byte"; } }

        public NotEqualNodeByte() : base() { }
        public NotEqualNodeByte(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new NotEqualNodeByte(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Short")]
    public class NotEqualNodeShort : NotEqualNode<short>
    {
        public override string Title { get { return "NotEqual Short"; } }

        public NotEqualNodeShort() : base() { }
        public NotEqualNodeShort(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new NotEqualNodeShort(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Integer")]
    public class NotEqualNodeInt : NotEqualNode<int>
    {
        public override string Title { get { return "NotEqual Integer"; } }

        public NotEqualNodeInt() : base() { }
        public NotEqualNodeInt(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new NotEqualNodeInt(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Long")]
    public class NotEqualNodeLong : NotEqualNode<long>
    {
        public override string Title { get { return "NotEqual Long"; } }

        public NotEqualNodeLong() : base() { }
        public NotEqualNodeLong(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new NotEqualNodeLong(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Float")]
    public class NotEqualNodeFloat : NotEqualNode<float>
    {
        public override string Title { get { return "NotEqual Float"; } }

        public NotEqualNodeFloat() : base() { }
        public NotEqualNodeFloat(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new NotEqualNodeFloat(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Double")]
    public class NotEqualNodeDouble : NotEqualNode<double>
    {
        public override string Title { get { return "NotEqual Double"; } }

        public NotEqualNodeDouble() : base() { }
        public NotEqualNodeDouble(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new NotEqualNodeDouble(); }
    }

    #endregion // Not Equal

    #region LessThan operator

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Category("Maths/Logic/LessThan")]
    public abstract class LessThanNode<T> : MathLogicOperatorNode<T>
    {
        public LessThanNode(XmlNode node_) : base(node_) { }
        public LessThanNode() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public override bool DoActivateLogic(T a_, T b_)
        {
            dynamic A = a_;
            dynamic B = b_;
            return A < B;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Byte")]
    public class LessThanNodeByte : LessThanNode<sbyte>
    {
        public override string Title { get { return "LessThan Byte"; } }

        public LessThanNodeByte() : base() { }
        public LessThanNodeByte(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new LessThanNodeByte(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Short")]
    public class LessThanNodeShort : LessThanNode<short>
    {
        public override string Title { get { return "LessThan Short"; } }

        public LessThanNodeShort() : base() { }
        public LessThanNodeShort(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new LessThanNodeShort(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Integer")]
    public class LessThanNodeInt : LessThanNode<int>
    {
        public override string Title { get { return "LessThan Integer"; } }

        public LessThanNodeInt() : base() { }
        public LessThanNodeInt(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new LessThanNodeInt(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Long")]
    public class LessThanNodeLong : LessThanNode<long>
    {
        public override string Title { get { return "LessThan Long"; } }

        public LessThanNodeLong() : base() { }
        public LessThanNodeLong(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new LessThanNodeLong(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Float")]
    public class LessThanNodeFloat : LessThanNode<float>
    {
        public override string Title { get { return "LessThan Float"; } }

        public LessThanNodeFloat() : base() { }
        public LessThanNodeFloat(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new LessThanNodeFloat(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Double")]
    public class LessThanNodeDouble : LessThanNode<double>
    {
        public override string Title { get { return "LessThan Double"; } }

        public LessThanNodeDouble() : base() { }
        public LessThanNodeDouble(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new LessThanNodeDouble(); }
    }

    #endregion // LessThan

    #region LessThanOrEqual operator

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Category("Maths/Logic/LessThanOrEqual")]
    public abstract class LessThanOrEqualNode<T> : MathLogicOperatorNode<T>
    {
        public LessThanOrEqualNode(XmlNode node_) : base(node_) { }
        public LessThanOrEqualNode() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public override bool DoActivateLogic(T a_, T b_)
        {
            dynamic A = a_;
            dynamic B = b_;
            return A <= B;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Byte")]
    public class LessThanOrEqualNodeByte : LessThanOrEqualNode<sbyte>
    {
        public override string Title { get { return "LessThanOrEqual Byte"; } }

        public LessThanOrEqualNodeByte() : base() { }
        public LessThanOrEqualNodeByte(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new LessThanOrEqualNodeByte(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Short")]
    public class LessThanOrEqualNodeShort : LessThanOrEqualNode<short>
    {
        public override string Title { get { return "LessThanOrEqual Short"; } }

        public LessThanOrEqualNodeShort() : base() { }
        public LessThanOrEqualNodeShort(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new LessThanOrEqualNodeShort(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Integer")]
    public class LessThanOrEqualNodeInt : LessThanOrEqualNode<int>
    {
        public override string Title { get { return "LessThanOrEqual Integer"; } }

        public LessThanOrEqualNodeInt() : base() { }
        public LessThanOrEqualNodeInt(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new LessThanOrEqualNodeInt(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Long")]
    public class LessThanOrEqualNodeLong : LessThanOrEqualNode<long>
    {
        public override string Title { get { return "LessThanOrEqual Long"; } }

        public LessThanOrEqualNodeLong() : base() { }
        public LessThanOrEqualNodeLong(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new LessThanOrEqualNodeLong(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Float")]
    public class LessThanOrEqualNodeFloat : LessThanOrEqualNode<float>
    {
        public override string Title { get { return "LessThanOrEqual Float"; } }

        public LessThanOrEqualNodeFloat() : base() { }
        public LessThanOrEqualNodeFloat(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new LessThanOrEqualNodeFloat(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Double")]
    public class LessThanOrEqualNodeDouble : LessThanOrEqualNode<double>
    {
        public override string Title { get { return "LessThanOrEqual Double"; } }

        public LessThanOrEqualNodeDouble() : base() { }
        public LessThanOrEqualNodeDouble(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new LessThanOrEqualNodeDouble(); }
    }

    #endregion // LessThanOrEqual

    #region GreaterThan operator

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Category("Maths/Logic/GreaterThan")]
    public abstract class GreaterThanNode<T> : MathLogicOperatorNode<T>
    {
        public GreaterThanNode(XmlNode node_) : base(node_) { }
        public GreaterThanNode() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public override bool DoActivateLogic(T a_, T b_)
        {
            dynamic A = a_;
            dynamic B = b_;
            return A > B;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Byte")]
    public class GreaterThanNodeByte : GreaterThanNode<sbyte>
    {
        public override string Title { get { return "GreaterThan Byte"; } }

        public GreaterThanNodeByte() : base() { }
        public GreaterThanNodeByte(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new GreaterThanNodeByte(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Short")]
    public class GreaterThanNodeShort : GreaterThanNode<short>
    {
        public override string Title { get { return "GreaterThan Short"; } }

        public GreaterThanNodeShort() : base() { }
        public GreaterThanNodeShort(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new GreaterThanNodeShort(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Integer")]
    public class GreaterThanNodeInt : GreaterThanNode<int>
    {
        public override string Title { get { return "GreaterThan Integer"; } }

        public GreaterThanNodeInt() : base() { }
        public GreaterThanNodeInt(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new GreaterThanNodeInt(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Long")]
    public class GreaterThanNodeLong : GreaterThanNode<long>
    {
        public override string Title { get { return "GreaterThan Long"; } }

        public GreaterThanNodeLong() : base() { }
        public GreaterThanNodeLong(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new GreaterThanNodeLong(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Float")]
    public class GreaterThanNodeFloat : GreaterThanNode<float>
    {
        public override string Title { get { return "GreaterThan Float"; } }

        public GreaterThanNodeFloat() : base() { }
        public GreaterThanNodeFloat(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new GreaterThanNodeFloat(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Double")]
    public class GreaterThanNodeDouble : GreaterThanNode<double>
    {
        public override string Title { get { return "GreaterThan Double"; } }

        public GreaterThanNodeDouble() : base() { }
        public GreaterThanNodeDouble(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new GreaterThanNodeDouble(); }
    }

    #endregion // GreaterThan

    #region GreaterThanOrEqual operator

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Category("Maths/Logic/GreaterThanOrEqual")]
    public abstract class GreaterThanOrEqualNode<T> : MathLogicOperatorNode<T>
    {
        public GreaterThanOrEqualNode(XmlNode node_) : base(node_) { }
        public GreaterThanOrEqualNode() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public override bool DoActivateLogic(T a_, T b_)
        {
            dynamic A = a_;
            dynamic B = b_;
            return A >= B;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Byte")]
    public class GreaterThanOrEqualNodeByte : GreaterThanOrEqualNode<sbyte>
    {
        public override string Title { get { return "GreaterThanOrEqual Byte"; } }

        public GreaterThanOrEqualNodeByte() : base() { }
        public GreaterThanOrEqualNodeByte(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new GreaterThanOrEqualNodeByte(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Short")]
    public class GreaterThanOrEqualNodeShort : GreaterThanOrEqualNode<short>
    {
        public override string Title { get { return "GreaterThanOrEqual Short"; } }

        public GreaterThanOrEqualNodeShort() : base() { }
        public GreaterThanOrEqualNodeShort(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new GreaterThanOrEqualNodeShort(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Integer")]
    public class GreaterThanOrEqualNodeInt : GreaterThanOrEqualNode<int>
    {
        public override string Title { get { return "GreaterThanOrEqual Integer"; } }

        public GreaterThanOrEqualNodeInt() : base() { }
        public GreaterThanOrEqualNodeInt(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new GreaterThanOrEqualNodeInt(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Long")]
    public class GreaterThanOrEqualNodeLong : GreaterThanOrEqualNode<long>
    {
        public override string Title { get { return "GreaterThanOrEqual Long"; } }

        public GreaterThanOrEqualNodeLong() : base() { }
        public GreaterThanOrEqualNodeLong(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new GreaterThanOrEqualNodeLong(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Float")]
    public class GreaterThanOrEqualNodeFloat : GreaterThanOrEqualNode<float>
    {
        public override string Title { get { return "GreaterThanOrEqual Float"; } }

        public GreaterThanOrEqualNodeFloat() : base() { }
        public GreaterThanOrEqualNodeFloat(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new GreaterThanOrEqualNodeFloat(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Double")]
    public class GreaterThanOrEqualNodeDouble : GreaterThanOrEqualNode<double>
    {
        public override string Title { get { return "GreaterThanOrEqual Double"; } }

        public GreaterThanOrEqualNodeDouble() : base() { }
        public GreaterThanOrEqualNodeDouble(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new GreaterThanOrEqualNodeDouble(); }
    }

    #endregion // GreaterThanOrEqual

    #endregion // Math logic operator

    #region Math cast operator

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MathCastOperatorNode<IN, OUT> : ActionNode
    {
        #region Enum

        public enum NodeSlotId
        {
            In,
            Out,
            VarA,
            VarResult
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public MathCastOperatorNode(XmlNode node_)
            : base(node_)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public MathCastOperatorNode()
            : base()
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

            AddSlot((int)NodeSlotId.VarA, "A", SlotType.VarIn, typeof(IN), false);
            AddSlot((int)NodeSlotId.VarResult, "Result", SlotType.VarOut, typeof(OUT), false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public abstract OUT DoActivateLogic(IN a_);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override ProcessingInfo ActivateLogic(ProcessingContext context_, NodeSlot slot_)
        {
            ProcessingInfo info = new ProcessingInfo();
            info.State = ActionNode.LogicState.OK;

            object objA = GetValueFromSlot((int)NodeSlotId.VarA);

            if (objA == null)
            {
                info.State = ActionNode.LogicState.Warning;
                info.ErrorMessage = "Please connect a variable node into the slot A";

                LogManager.Instance.WriteLine(LogVerbosity.Warning,
                    "{0} : {1}.",
                    Title, info.ErrorMessage);
            }

            if (objA != null)
            {
                SetValueInSlot((int)NodeSlotId.VarResult, DoActivateLogic((IN)objA));
            }

            ActivateOutputLink(context_, (int)NodeSlotId.Out);

            return info;
        }
    }

    #region To String

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Category("Maths/Cast/To String")]
    public abstract class ToStringNode<IN> : MathCastOperatorNode<IN, string>
    {
        public ToStringNode(XmlNode node_) : base(node_) { }
        public ToStringNode() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public override string DoActivateLogic(IN a_)
        {
            dynamic A = a_;
            return A.ToString();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Byte to String")]
    public class ToStringNodeByte : ToStringNode<sbyte>
    {
        public override string Title { get { return "Byte to String"; } }

        public ToStringNodeByte() : base() { }
        public ToStringNodeByte(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new ToStringNodeByte(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Char to String")]
    public class ToStringNodeChar : ToStringNode<char>
    {
        public override string Title { get { return "Char to String"; } }

        public ToStringNodeChar() : base() { }
        public ToStringNodeChar(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new ToStringNodeChar(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Short to String")]
    public class ToStringNodeShort : ToStringNode<short>
    {
        public override string Title { get { return "Short to String"; } }

        public ToStringNodeShort() : base() { }
        public ToStringNodeShort(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new ToStringNodeShort(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Integer to String")]
    public class ToStringNodeInteger : ToStringNode<int>
    {
        public override string Title { get { return "Integer to String"; } }

        public ToStringNodeInteger() : base() { }
        public ToStringNodeInteger(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new ToStringNodeInteger(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Long to String")]
    public class ToStringNodeLong : ToStringNode<long>
    {
        public override string Title { get { return "Long to String"; } }

        public ToStringNodeLong() : base() { }
        public ToStringNodeLong(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new ToStringNodeLong(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Float to String")]
    public class ToStringNodeFloat : ToStringNode<float>
    {
        public override string Title { get { return "Float to String"; } }

        public ToStringNodeFloat() : base() { }
        public ToStringNodeFloat(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new ToStringNodeFloat(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Double to String")]
    public class ToStringNodeDouble : ToStringNode<double>
    {
        public override string Title { get { return "Double to String"; } }

        public ToStringNodeDouble() : base() { }
        public ToStringNodeDouble(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new ToStringNodeDouble(); }
    }

    #endregion // To String

    #region To Integer

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Category("Maths/Cast/To Integer")]
    public abstract class ToIntegerNode<IN> : MathCastOperatorNode<IN, int>
    {
        public ToIntegerNode(XmlNode node_) : base(node_) { }
        public ToIntegerNode() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public override int DoActivateLogic(IN a_)
        {
            dynamic A = a_;
            return (int) Convert.ChangeType(a_, typeof(int));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Byte to Integer")]
    public class ToIntegerNodeByte : ToIntegerNode<sbyte>
    {
        public override string Title { get { return "Byte to Integer"; } }

        public ToIntegerNodeByte() : base() { }
        public ToIntegerNodeByte(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new ToIntegerNodeByte(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Char to Integer")]
    public class ToIntegerNodeChar : ToIntegerNode<char>
    {
        public override string Title { get { return "Char to Integer"; } }

        public ToIntegerNodeChar() : base() { }
        public ToIntegerNodeChar(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new ToIntegerNodeChar(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Short to Integer")]
    public class ToIntegerNodeShort : ToIntegerNode<short>
    {
        public override string Title { get { return "Short to Integer"; } }

        public ToIntegerNodeShort() : base() { }
        public ToIntegerNodeShort(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new ToIntegerNodeShort(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Long to Integer")]
    public class ToIntegerNodeLong : ToIntegerNode<long>
    {
        public override string Title { get { return "Long to Integer"; } }

        public ToIntegerNodeLong() : base() { }
        public ToIntegerNodeLong(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new ToIntegerNodeLong(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Float to Integer")]
    public class ToIntegerNodeFloat : ToIntegerNode<float>
    {
        public override string Title { get { return "Float to Integer"; } }

        public ToIntegerNodeFloat() : base() { }
        public ToIntegerNodeFloat(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new ToIntegerNodeFloat(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Double to Integer")]
    public class ToIntegerNodeDouble : ToIntegerNode<double>
    {
        public override string Title { get { return "Double to Integer"; } }

        public ToIntegerNodeDouble() : base() { }
        public ToIntegerNodeDouble(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new ToIntegerNodeDouble(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("String to Integer")]
    public class ToIntegerNodeString : ToIntegerNode<string>
    {
        public override string Title { get { return "Short to Integer"; } }

        public ToIntegerNodeString() : base() { }
        public ToIntegerNodeString(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new ToIntegerNodeString(); }
    }

    #endregion // To Integer

    #region To Double

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Category("Maths/Cast/To Double")]
    public abstract class ToDoubleNode<IN> : MathCastOperatorNode<IN, double>
    {
        public ToDoubleNode(XmlNode node_) : base(node_) { }
        public ToDoubleNode() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public override double DoActivateLogic(IN a_)
        {
            dynamic A = a_;
            return (double)Convert.ChangeType(a_, typeof(double));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Byte to Double")]
    public class ToDoubleNodeByte : ToDoubleNode<sbyte>
    {
        public override string Title { get { return "Byte to Double"; } }

        public ToDoubleNodeByte() : base() { }
        public ToDoubleNodeByte(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new ToDoubleNodeByte(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Char to Double")]
    public class ToDoubleNodeChar : ToDoubleNode<char>
    {
        public override string Title { get { return "Char to Double"; } }

        public ToDoubleNodeChar() : base() { }
        public ToDoubleNodeChar(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new ToDoubleNodeChar(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Short to Double")]
    public class ToDoubleNodeShort : ToDoubleNode<short>
    {
        public override string Title { get { return "Short to Double"; } }

        public ToDoubleNodeShort() : base() { }
        public ToDoubleNodeShort(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new ToDoubleNodeShort(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Integer to Double")]
    public class ToDoubleNodeInteger : ToDoubleNode<int>
    {
        public override string Title { get { return "Integer to Double"; } }

        public ToDoubleNodeInteger() : base() { }
        public ToDoubleNodeInteger(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new ToDoubleNodeInteger(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Long to Double")]
    public class ToDoubleNodeLong : ToDoubleNode<long>
    {
        public override string Title { get { return "Long to Double"; } }

        public ToDoubleNodeLong() : base() { }
        public ToDoubleNodeLong(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new ToDoubleNodeLong(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Float to Double")]
    public class ToDoubleNodeFloat : ToDoubleNode<float>
    {
        public override string Title { get { return "FLoat to Double"; } }

        public ToDoubleNodeFloat() : base() { }
        public ToDoubleNodeFloat(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new ToDoubleNodeFloat(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("String to Double")]
    public class ToDoubleNodeString : ToDoubleNode<string>
    {
        public override string Title { get { return "String to Double"; } }

        public ToDoubleNodeString() : base() { }
        public ToDoubleNodeString(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new ToDoubleNodeString(); }
    }

    #endregion // To Double

    #region To Object

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Category("Maths/Cast/To Object")]
    public abstract class ToObjectNode<IN> : MathCastOperatorNode<IN, object>
    {
        public ToObjectNode(XmlNode node_) : base(node_) { }
        public ToObjectNode() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public override object DoActivateLogic(IN a_)
        {
            dynamic A = a_;
            return Convert.ChangeType(a_, typeof(object));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Byte to Object")]
    public class ToObjectNodeByte : ToObjectNode<sbyte>
    {
        public override string Title { get { return "Byte to Object"; } }

        public ToObjectNodeByte() : base() { }
        public ToObjectNodeByte(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new ToObjectNodeByte(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Char to Object")]
    public class ToObjectNodeChar : ToObjectNode<char>
    {
        public override string Title { get { return "Char to Object"; } }

        public ToObjectNodeChar() : base() { }
        public ToObjectNodeChar(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new ToObjectNodeChar(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Short to Object")]
    public class ToObjectNodeShort : ToObjectNode<short>
    {
        public override string Title { get { return "Short to Object"; } }

        public ToObjectNodeShort() : base() { }
        public ToObjectNodeShort(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new ToObjectNodeShort(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Integer to Object")]
    public class ToObjectNodeInteger : ToObjectNode<int>
    {
        public override string Title { get { return "Integer to Object"; } }

        public ToObjectNodeInteger() : base() { }
        public ToObjectNodeInteger(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new ToObjectNodeInteger(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Long to Object")]
    public class ToObjectNodeLong : ToObjectNode<long>
    {
        public override string Title { get { return "Long to Object"; } }

        public ToObjectNodeLong() : base() { }
        public ToObjectNodeLong(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new ToObjectNodeLong(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Float to Object")]
    public class ToObjectNodeFloat : ToObjectNode<float>
    {
        public override string Title { get { return "Float to Object"; } }

        public ToObjectNodeFloat() : base() { }
        public ToObjectNodeFloat(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new ToObjectNodeFloat(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Double to Object")]
    public class ToObjectNodeDouble : ToObjectNode<double>
    {
        public override string Title { get { return "Double to Object"; } }

        public ToObjectNodeDouble() : base() { }
        public ToObjectNodeDouble(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new ToObjectNodeDouble(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("String to Object")]
    public class ToObjectNodeString : ToObjectNode<string>
    {
        public override string Title { get { return "String to Object"; } }

        public ToObjectNodeString() : base() { }
        public ToObjectNodeString(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new ToObjectNodeString(); }
    }

    #endregion // To Double

    #endregion // Math cast operator

    #region Math Random

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Category("Maths/Random")]
    public abstract class MathRandomNode<T> : ActionNode
    {
        #region Enum

        public enum NodeSlotId
        {
            In,
            Out,
            VarMin,
            VarMax,
            VarResult
        }

        #endregion

        static Random m_Random = new Random();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public MathRandomNode(XmlNode node_)
            : base(node_) { }

        /// <summary>
        /// 
        /// </summary>
        public MathRandomNode()
            : base() { }

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
        public override ProcessingInfo ActivateLogic(ProcessingContext context_, NodeSlot slot_)
        {
            ProcessingInfo info = new ProcessingInfo();
            info.State = ActionNode.LogicState.OK;

            object objMin = GetValueFromSlot((int)NodeSlotId.VarMin);

            if (objMin == null)
            {
                info.ErrorMessage = "Please connect a variable node into the slot Min";
                info.State = ActionNode.LogicState.Warning;

                LogManager.Instance.WriteLine(LogVerbosity.Warning,
                    "{0} : {1}.",
                    Title, info.ErrorMessage);
            }

            object objMax = GetValueFromSlot((int)NodeSlotId.VarMax);

            if (objMax == null)
            {
                info.ErrorMessage = "Please connect a variable node into the slot Max";
                info.State = ActionNode.LogicState.Warning;

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
                    result = m_Random.NextDouble();

                    dynamic min = objMin;
                    dynamic max = objMax;
                    result = min + (T)result * (max - min);
                }
                else
                {
                    result = m_Random.Next((int)objMin, (int)objMax);
                }

                SetValueInSlot((int)NodeSlotId.VarResult, result);
            }

            ActivateOutputLink(context_, (int)NodeSlotId.Out);

            return info;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Random Byte")]
    public class RandomByteNode : MathRandomNode<sbyte>
    {
        public override string Title { get { return "Random Byte"; } }

        public RandomByteNode() : base() { }
        public RandomByteNode(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new RandomByteNode(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Random Short")]
    public class RandomShortNode : MathRandomNode<short>
    {
        public override string Title { get { return "Random Short"; } }

        public RandomShortNode() : base() { }
        public RandomShortNode(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new RandomShortNode(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Random Integer")]
    public class RandomIntegerNode : MathRandomNode<int>
    {
        public override string Title { get { return "Random Integer"; } }

        public RandomIntegerNode() : base() { }
        public RandomIntegerNode(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new RandomIntegerNode(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Random Long")]
    public class RandomLongNode : MathRandomNode<long>
    {
        public override string Title { get { return "Random Long"; } }

        public RandomLongNode() : base() { }
        public RandomLongNode(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new RandomLongNode(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Random Float")]
    public class RandomFloatNode : MathRandomNode<float>
    {
        public override string Title { get { return "Random Float"; } }

        public RandomFloatNode() : base() { }
        public RandomFloatNode(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new RandomFloatNode(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Random Double")]
    public class RandomDoubleNode : MathRandomNode<double>
    {
        public override string Title { get { return "Random Double"; } }

        public RandomDoubleNode() : base() { }
        public RandomDoubleNode(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new RandomDoubleNode(); }
    }

    #endregion // Math Random

    #region String operations

    /// <summary>
    /// 
    /// </summary>
    [Category("String")]
    [Name("Concat")]
    public class StringConcatNode : ActionNode
    {
        #region Enum

        public enum NodeSlotId
        {
            In,
            Out,
            VarA,
            VarB,
            VarResult
        }

        #endregion

		#region Fields
		
		#endregion //Fields
	
		#region Properties

        public override string Title { get { return "String Concat"; } }

		#endregion //Properties
	
		#region Constructors

        /// <summary>
        /// 
        /// </summary>
        public StringConcatNode() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public StringConcatNode(XmlNode node_) : base(node_) { }

		#endregion //Constructors
	
		#region Methods
		
		#endregion //Methods
        
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
        public override ProcessingInfo ActivateLogic(ProcessingContext context_, NodeSlot slot_)
        {
            ProcessingInfo info = new ProcessingInfo();
            info.State = ActionNode.LogicState.OK;

            object objA = GetValueFromSlot((int)NodeSlotId.VarA);

            if (objA == null)
            {
                info.ErrorMessage = "Please connect a variable node into the slot A";
                info.State = ActionNode.LogicState.Warning;

                LogManager.Instance.WriteLine(LogVerbosity.Warning,
                    "{0} : {1}.",
                    Title, info.ErrorMessage);
            }

            object objB = GetValueFromSlot((int)NodeSlotId.VarB);

            if (objB == null)
            {
                info.ErrorMessage = "Please connect a variable node into the slot B";
                info.State = ActionNode.LogicState.Warning;

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

            ActivateOutputLink(context_, (int)NodeSlotId.Out);

            return info;
        }

        protected override SequenceNode CopyImpl() { return new StringConcatNode(); }
    }

    #endregion // String operations

    #region Variable Set

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Category("Variable/Set")]
    public abstract class VariableSetNode<T> : ActionNode
    {
        #region Enum

        public enum NodeSlotId
        {
            In,
            Out,
            Variable,
            Value,
            VarResult
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public VariableSetNode(XmlNode node_)
            : base(node_) { }

        /// <summary>
        /// 
        /// </summary>
        public VariableSetNode()
            : base() { }

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
        public override ProcessingInfo ActivateLogic(ProcessingContext context_, NodeSlot slot_)
        {
            ProcessingInfo info = new ProcessingInfo();
            info.State = ActionNode.LogicState.OK;

            object objVariable = GetValueFromSlot((int)NodeSlotId.Variable);
            object objValue = GetValueFromSlot((int)NodeSlotId.Value);

            if (objVariable == null)
            {
                info.State = ActionNode.LogicState.Warning;
                info.ErrorMessage = "Please connect a variable node into the slot Variable";
                LogManager.Instance.WriteLine(LogVerbosity.Warning,
                    "{0} : Addition failed. {1}.",
                    Title, info.ErrorMessage);
            }
            else if (objValue == null)
            {
                info.State = ActionNode.LogicState.Warning;
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

            ActivateOutputLink(context_, (int)NodeSlotId.Out);

            return info;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Byte")]
    public class VariableSetNodeByte : VariableSetNode<sbyte>
    {
        public override string Title { get { return "Set Byte"; } }

        public VariableSetNodeByte() : base() { }
        public VariableSetNodeByte(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new VariableSetNodeByte(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Short")]
    public class VariableSetNodeShort : VariableSetNode<short>
    {
        public override string Title { get { return "Set Short"; } }

        public VariableSetNodeShort() : base() { }
        public VariableSetNodeShort(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new VariableSetNodeShort(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Integer")]
    public class VariableSetNodeInt : VariableSetNode<int>
    {
        public override string Title { get { return "Set Integer"; } }

        public VariableSetNodeInt() : base() { }
        public VariableSetNodeInt(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new VariableSetNodeInt(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Long")]
    public class VariableSetNodeLong : VariableSetNode<long>
    {
        public override string Title { get { return "Set Long"; } }

        public VariableSetNodeLong() : base() { }
        public VariableSetNodeLong(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new VariableSetNodeLong(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Float")]
    public class VariableSetNodeFloat : VariableSetNode<float>
    {
        public override string Title { get { return "Set Float"; } }

        public VariableSetNodeFloat() : base() { }
        public VariableSetNodeFloat(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new VariableSetNodeFloat(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Double")]
    public class VariableSetNodeDouble : VariableSetNode<double>
    {
        public override string Title { get { return "Set Double"; } }

        public VariableSetNodeDouble() : base() { }
        public VariableSetNodeDouble(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new VariableSetNodeDouble(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("String")]
    public class VariableSetNodeString : VariableSetNode<double>
    {
        public override string Title { get { return "Set String"; } }

        public VariableSetNodeString() : base() { }
        public VariableSetNodeString(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new VariableSetNodeString(); }
    }

    /// <summary>
    /// 
    /// </summary>
    [Name("Object")]
    public class VariableSetNodeObject : VariableSetNode<object>
    {
        public override string Title { get { return "Set Object"; } }

        public VariableSetNodeObject() : base() { }
        public VariableSetNodeObject(XmlNode node_) : base(node_) { }

        protected override SequenceNode CopyImpl() { return new VariableSetNodeObject(); }
    }

    #endregion // Variable Set
}
