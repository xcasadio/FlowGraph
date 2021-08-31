using System;
using System.Xml;
using FlowGraphBase.Logger;
using FlowGraphBase.Process;

namespace FlowGraphBase.Node.StandardActionNode
{
    /// <summary>
    /// The Branch node serves as a simple way to create decision-based flow from 
    /// a single true/false condition. Once executed, the Branch node looks at 
    /// the incoming value of the attached Boolean, and outputs an execution pulse 
    /// down the appropriate output. 
    /// </summary>
    [Category("Flow Control"), Name("Branch")]
    public class BranchNode : 
        ActionNode
    {
        public enum NodeSlotId
        {
            In,
            OutTrue,
            OutFalse,
            VarCond,
        }

        public override string Title => "Branch";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public BranchNode(XmlNode node)
            : base(node) { }

        /// <summary>
        /// 
        /// </summary>
        public BranchNode()
        { }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitializeSlots()
        {
            base.InitializeSlots();

            AddSlot((int)NodeSlotId.In, "", SlotType.NodeIn);
            AddSlot((int)NodeSlotId.VarCond, "Condition", SlotType.VarIn, typeof(bool));

            AddSlot((int)NodeSlotId.OutTrue, "True", SlotType.NodeOut);
            AddSlot((int)NodeSlotId.OutFalse, "False", SlotType.NodeOut);
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

            object objCond = GetValueFromSlot((int)NodeSlotId.VarCond);

            if (objCond == null)
            {
                info.State = LogicState.Warning;
                info.ErrorMessage = "Please connect a variable node into the slot Condition";
                LogManager.Instance.WriteLine(LogVerbosity.Warning,
                    "{0} : Branch failed. {1}.",
                    Title, info.ErrorMessage);
            }

            if (objCond != null)
            {
                bool cond = (bool)objCond;

                if (cond)
                {
                    ActivateOutputLink(context, (int)NodeSlotId.OutTrue);
                }
                else
                {
                    ActivateOutputLink(context, (int)NodeSlotId.OutFalse);
                }
            }

            return info;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override SequenceNode CopyImpl()
        {
            return new BranchNode();
        }
    }

    /// <summary>
    /// The DoN node will fire off an execution pulse N times. 
    /// After the limit has been reached, it will cease all outgoing 
    /// execution until a pulse is sent into its Reset input. 
    /// </summary>
    [Category("Flow Control"), Name("Do N")]
    public class DoNNode :
        ActionNode
    {
        public enum NodeSlotId
        {
            InEnter,
            InReset,
            Out,
            VarInN,
            VarCounter,
        }

        bool _isInitial;
        int _counter;

        public override string Title => "Do N";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public DoNNode(XmlNode node)
            : base(node) { }

        /// <summary>
        /// 
        /// </summary>
        public DoNNode()
        { }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitializeSlots()
        {
            base.InitializeSlots();

            AddSlot((int)NodeSlotId.InEnter, "Enter", SlotType.NodeIn);
            AddSlot((int)NodeSlotId.VarInN, "N", SlotType.VarIn, typeof(int));
            AddSlot((int)NodeSlotId.InReset, "Reset", SlotType.NodeIn);

            AddSlot((int)NodeSlotId.Out, "Exit", SlotType.NodeOut);
            AddSlot((int)NodeSlotId.VarCounter, "Counter", SlotType.VarOut, typeof(int));
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

            if (slot.Id == (int)NodeSlotId.InReset)
            {
                _counter = 0;
                _isInitial = false;
            }
            else if (slot.Id == (int)NodeSlotId.InEnter)
            {
                MemoryStackItem memoryItem = context.CurrentFrame.GetValueFromId(Id);

                if (_isInitial == false)
                {
                    object objN = GetValueFromSlot((int)NodeSlotId.VarInN);

                    if (objN == null)
                    {
                        info.State = LogicState.Warning;
                        info.ErrorMessage = "Please connect a variable node into the slot N";
                        LogManager.Instance.WriteLine(LogVerbosity.Warning,
                            "{0} : DoN failed. {1}.",
                            Title, info.ErrorMessage);
                    }

                    if (objN != null)
                    {
                        int n = (int)objN;

                        if (memoryItem == null)
                        {
                            memoryItem = context.CurrentFrame.Allocate(Id, n);
                        }

                        memoryItem.Value = n;
                    }

                    _isInitial = true;
                }

                if (_counter < (int)memoryItem.Value)
                {
                    _counter++;
                    ActivateOutputLink(context, (int)NodeSlotId.Out);
                }
            }

            return info;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override SequenceNode CopyImpl()
        {
            return new DoNNode();
        }
    }

    /// <summary>
    /// The DoOnce node - as the name suggests - will fire off an execution pulse just once. 
    /// From that point forward, it will cease all outgoing execution until a pulse is sent into its Reset input. 
    /// This node is equivalent to a DoN node where N = 1.  
    /// </summary>
    [Category("Flow Control"), Name("Do Once")]
    public class DoOnceNode :
        ActionNode
    {
        public enum NodeSlotId
        {
            InEnter,
            InReset,
            Out,
        }

        public override string Title => "Do Once";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public DoOnceNode(XmlNode node)
            : base(node) { }

        /// <summary>
        /// 
        /// </summary>
        public DoOnceNode()
        { }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitializeSlots()
        {
            base.InitializeSlots();

            AddSlot((int)NodeSlotId.InEnter, "", SlotType.NodeIn);
            AddSlot((int)NodeSlotId.InReset, "Reset", SlotType.NodeIn);
            AddSlot((int)NodeSlotId.Out, "Completed", SlotType.NodeOut);
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

            MemoryStackItem memoryItem = context.CurrentFrame.GetValueFromId(Id);

            if (memoryItem == null)
            {
                memoryItem = context.CurrentFrame.Allocate(Id, false);
            }

            if (slot.Id == (int)NodeSlotId.InReset)
            {
                memoryItem.Value = false;
            }
            else if (slot.Id == (int)NodeSlotId.InEnter)
            {
                if ((bool)memoryItem.Value == false)
                {
                    memoryItem.Value = true;
                    ActivateOutputLink(context, (int)NodeSlotId.Out);
                }
            }

            return info;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override SequenceNode CopyImpl()
        {
            return new DoOnceNode();
        }
    }

    /// <summary>
    /// The FlipFlop node takes in an execution output and toggles between two execution outputs. 
    /// The first time it is called, output A executes. 
    /// The second time, B. Then A, then B, and so on. 
    /// The node also has a boolean output allowing you to track when Output A has been called. 
    /// </summary>
    [Category("Flow Control"), Name("Flip Flop")]
    public class FlipFlopNode :
        ActionNode
    {
        public enum NodeSlotId
        {
            InEnter,
            OutA,
            OutB,
            VarOutIsA,
        }

        public override string Title => "Flip Flop";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public FlipFlopNode(XmlNode node)
            : base(node) { }

        /// <summary>
        /// 
        /// </summary>
        public FlipFlopNode()
        { }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitializeSlots()
        {
            base.InitializeSlots();

            AddSlot((int)NodeSlotId.InEnter, "", SlotType.NodeIn);
            AddSlot((int)NodeSlotId.OutA, "A", SlotType.NodeOut);
            AddSlot((int)NodeSlotId.OutB, "B", SlotType.NodeOut);
            AddSlot((int)NodeSlotId.VarOutIsA, "IsA", SlotType.VarOut, typeof(bool));
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

            MemoryStackItem memoryItem = context.CurrentFrame.GetValueFromId(Id);

            if (memoryItem == null)
            {
                memoryItem = context.CurrentFrame.Allocate(Id, true);
            }
            
            if (slot.Id == (int)NodeSlotId.InEnter)
            {
                bool val = (bool)memoryItem.Value;
                memoryItem.Value = !(bool)memoryItem.Value;

                SetValueInSlot((int)NodeSlotId.VarOutIsA, val);

                if (val)
                {
                    ActivateOutputLink(context, (int)NodeSlotId.OutA);
                }
                else
                {
                    ActivateOutputLink(context, (int)NodeSlotId.OutB);
                }
            }

            return info;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override SequenceNode CopyImpl()
        {
            return new FlipFlopNode();
        }
    }

    /// <summary>
    /// The ForLoop node works like a standard code loop, firing off an execution pulse 
    /// for each index between a start and end. 
    /// </summary>
    [Category("Flow Control"), Name("For Loop")]
    public class ForLoopNode :
        ActionNode
    {
        /// <summary>
        /// 
        /// </summary>
        struct ForLoopNodeInfo
        {
            public int Counter;
            public bool IsWaitingLoopBody;
        }

        public enum NodeSlotId
        {
            In,
            OutLoop,
            OutCompleted,
            VarInFirstIndex,
            VarInLastIndex,
            VarOutIndex,
        }

        public override string Title => "For Loop";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public ForLoopNode(XmlNode node)
            : base(node) { }

        /// <summary>
        /// 
        /// </summary>
        public ForLoopNode()
        { }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitializeSlots()
        {
            base.InitializeSlots();

            AddSlot((int)NodeSlotId.In, "", SlotType.NodeIn);
            AddSlot((int)NodeSlotId.VarInFirstIndex, "First Index", SlotType.VarIn, typeof(int));
            AddSlot((int)NodeSlotId.VarInLastIndex, "Last Index", SlotType.VarIn, typeof(int));

            AddSlot((int)NodeSlotId.OutLoop, "Loop Body", SlotType.NodeOut);
            AddSlot((int)NodeSlotId.VarOutIndex, "Index", SlotType.VarOut, typeof(int));
            AddSlot((int)NodeSlotId.OutCompleted, "Completed", SlotType.NodeOut);
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

            if (slot.Id == (int)NodeSlotId.In)
            {
                int firstIndex = 0, lastIndex = -1;

                object objFirstIndex = GetValueFromSlot((int)NodeSlotId.VarInFirstIndex);

                if (objFirstIndex == null)
                {
                    info.State = LogicState.Warning;
                    info.ErrorMessage = "Please connect a variable node into the slot First Index";
                    LogManager.Instance.WriteLine(LogVerbosity.Warning,
                        "{0} : For Loop failed. {1}.",
                        Title, info.ErrorMessage);
                    return info;
                }

                if (objFirstIndex != null)
                {
                    firstIndex = (int)objFirstIndex;
                }

                object objLastIndex = GetValueFromSlot((int)NodeSlotId.VarInLastIndex);

                if (objLastIndex == null)
                {
                    info.State = LogicState.Warning;
                    info.ErrorMessage = "Please connect a variable node into the slot Last Index";
                    LogManager.Instance.WriteLine(LogVerbosity.Warning,
                        "{0} : For Loop failed. {1}.",
                        Title, info.ErrorMessage);
                    return info;
                }

                if (objLastIndex != null)
                {
                    lastIndex = (int)objLastIndex;
                }

                MemoryStackItem memoryItem = context.CurrentFrame.GetValueFromId(Id);

                if (memoryItem == null)
                {
                    memoryItem = context.CurrentFrame.Allocate(Id, new ForLoopNodeInfo { Counter = firstIndex, IsWaitingLoopBody = false });
                }

                ForLoopNodeInfo memoryInfo = (ForLoopNodeInfo)memoryItem.Value;

                if (memoryInfo.IsWaitingLoopBody == false)
                {
                    SetValueInSlot((int)NodeSlotId.VarOutIndex, memoryInfo.Counter);

                    if (memoryInfo.Counter <= lastIndex)
                    {
                        memoryInfo.IsWaitingLoopBody = true;
                        memoryInfo.Counter++;
                        memoryItem.Value = memoryInfo;

                        // register again this node in order to active itself
                        // after the sequence activated by the loop body slot
                        // is finished
                        context.RegisterNextExecution(GetSlotById((int)NodeSlotId.In)); 
                        ProcessingContext newContext = context.PushNewContext();
                        newContext.Finished += OnLoopBodyFinished;
                        ActivateOutputLink(newContext, (int)NodeSlotId.OutLoop);
                    }
                    else
                    {
                        context.CurrentFrame.Deallocate(Id);
                        ActivateOutputLink(context, (int)NodeSlotId.OutCompleted);
                    }
                }
            }

            return info;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnLoopBodyFinished(object sender, EventArgs e)
        {            
            ProcessingContext context = sender as ProcessingContext;
            context.Finished -= OnLoopBodyFinished;

            MemoryStackItem memoryItem = context.Parent.CurrentFrame.GetValueFromId(Id);
            ForLoopNodeInfo memoryInfo = (ForLoopNodeInfo)memoryItem.Value;
            memoryInfo.IsWaitingLoopBody = false;
            memoryItem.Value = memoryInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override SequenceNode CopyImpl()
        {
            return new ForLoopNode();
        }
    }

    /// <summary>
    /// The ForLoopWithBreak node works in a very similar manner to the ForLoop node, 
    /// except that it includes an input pin that allows the loop to be broken. 
    /// </summary>
    [Category("Flow Control"), Name("For Loop With Break")]
    public class ForLoopWithBreakNode :
        ActionNode
    {
        /// <summary>
        /// 
        /// </summary>
        struct ForLoopNodeInfo
        {
            public int Counter;
            public bool IsWaitingLoopBody;
            public ProcessingContextStep Step;
        }

        public enum NodeSlotId
        {
            In,
            OutLoop,
            OutCompleted,
            VarInFirstIndex,
            VarInLastIndex,
            VarOutIndex,
            InBreak
        }

        public override string Title => "For Loop With Break";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public ForLoopWithBreakNode(XmlNode node)
            : base(node) { }

        /// <summary>
        /// 
        /// </summary>
        public ForLoopWithBreakNode()
        { }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitializeSlots()
        {
            base.InitializeSlots();

            AddSlot((int)NodeSlotId.In, "", SlotType.NodeIn);
            AddSlot((int)NodeSlotId.VarInFirstIndex, "First Index", SlotType.VarIn, typeof(int));
            AddSlot((int)NodeSlotId.VarInLastIndex, "Last Index", SlotType.VarIn, typeof(int));
            AddSlot((int)NodeSlotId.InBreak, "Break", SlotType.NodeIn);

            AddSlot((int)NodeSlotId.OutLoop, "Loop Body", SlotType.NodeOut);
            AddSlot((int)NodeSlotId.VarOutIndex, "Index", SlotType.VarOut, typeof(int));
            AddSlot((int)NodeSlotId.OutCompleted, "Completed", SlotType.NodeOut);
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

            if (slot.Id == (int)NodeSlotId.In)
            {
                int firstIndex = 0, lastIndex = -1;

                object objFirstIndex = GetValueFromSlot((int)NodeSlotId.VarInFirstIndex);

                if (objFirstIndex == null)
                {
                    info.State = LogicState.Warning;
                    info.ErrorMessage = "Please connect a variable node into the slot First Index";
                    LogManager.Instance.WriteLine(LogVerbosity.Warning,
                        "{0} : For Loop failed. {1}.",
                        Title, info.ErrorMessage);
                    return info;
                }

                if (objFirstIndex != null)
                {
                    firstIndex = (int)objFirstIndex;
                }

                object objLastIndex = GetValueFromSlot((int)NodeSlotId.VarInLastIndex);

                if (objLastIndex == null)
                {
                    info.State = LogicState.Warning;
                    info.ErrorMessage = "Please connect a variable node into the slot Last Index";
                    LogManager.Instance.WriteLine(LogVerbosity.Warning,
                        "{0} : For Loop failed. {1}.",
                        Title, info.ErrorMessage);
                    return info;
                }

                if (objLastIndex != null)
                {
                    lastIndex = (int)objLastIndex;
                }

                MemoryStackItem memoryItem = context.CurrentFrame.GetValueFromId(Id);

                if (memoryItem == null)
                {
                    memoryItem = context.CurrentFrame.Allocate(Id, new ForLoopNodeInfo { Counter = firstIndex, IsWaitingLoopBody = false });
                }

                ForLoopNodeInfo memoryInfo = (ForLoopNodeInfo)memoryItem.Value;

                if (memoryInfo.IsWaitingLoopBody == false)
                {
                    SetValueInSlot((int)NodeSlotId.VarOutIndex, memoryInfo.Counter);

                    if (memoryInfo.Counter <= lastIndex)
                    {
                        memoryInfo.IsWaitingLoopBody = true;
                        memoryInfo.Counter++;
                        // register again this node in order to active itself
                        // after the sequence activated by the loop body slot
                        // is finished
                        memoryInfo.Step = context.RegisterNextExecution(GetSlotById((int)NodeSlotId.In));
                        memoryItem.Value = memoryInfo;
                        
                        ProcessingContext newContext = context.PushNewContext();
                        newContext.Finished += OnLoopBodyFinished;
                        ActivateOutputLink(newContext, (int)NodeSlotId.OutLoop);
                    }
                    else
                    {
                        context.CurrentFrame.Deallocate(Id);
                        ActivateOutputLink(context, (int)NodeSlotId.OutCompleted);
                    }
                }
            }
            else if (slot.Id == (int)NodeSlotId.InBreak)
            {
                MemoryStackItem memoryItem = context.CurrentFrame.GetValueFromId(Id);
                ForLoopNodeInfo memoryInfo = (ForLoopNodeInfo)memoryItem.Value;
                context.RemoveExecution(context, memoryInfo.Step);
                context.CurrentFrame.Deallocate(Id);
                ActivateOutputLink(context, (int)NodeSlotId.OutCompleted);
            }

            return info;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnLoopBodyFinished(object sender, EventArgs e)
        {
            ProcessingContext context = sender as ProcessingContext;
            context.Finished -= OnLoopBodyFinished;

            MemoryStackItem memoryItem = context.Parent.CurrentFrame.GetValueFromId(Id);
            if (memoryItem != null)
            {
                ForLoopNodeInfo memoryInfo = (ForLoopNodeInfo)memoryItem.Value;
                memoryInfo.IsWaitingLoopBody = false;
                memoryItem.Value = memoryInfo;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override SequenceNode CopyImpl()
        {
            return new ForLoopWithBreakNode();
        }
    }

    /// <summary>
    /// A Gate node is used as a way to open and close a stream of execution. 
    /// Simply put, the Enter input takes in execution pulses, and the current 
    /// state of the gate (open or closed) determines whether those pulses 
    /// pass out of the Exit output or not. 
    /// </summary>
    [Category("Flow Control"), Name("Gate")]
    public class GateNode :
        ActionNode
    {
        public enum NodeSlotId
        {
            InEnter,
            InOpen,
            InClose,
            InToggle,
            VarInStartClosed,
            Out
        }

        public override string Title => "Gate";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public GateNode(XmlNode node)
            : base(node) { }

        /// <summary>
        /// 
        /// </summary>
        public GateNode()
        { }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitializeSlots()
        {
            base.InitializeSlots();

            AddSlot((int)NodeSlotId.InEnter, "Enter", SlotType.NodeIn);
            AddSlot((int)NodeSlotId.InOpen, "Open", SlotType.NodeIn);
            AddSlot((int)NodeSlotId.InClose, "Close", SlotType.NodeIn);
            AddSlot((int)NodeSlotId.InToggle, "Toggle", SlotType.NodeIn);
            AddSlot((int)NodeSlotId.VarInStartClosed, "Start Closed", SlotType.VarIn, typeof(bool));

            AddSlot((int)NodeSlotId.Out, "Exit", SlotType.NodeOut);
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

            MemoryStackItem memoryItem = context.CurrentFrame.GetValueFromId(Id);

            if (memoryItem == null)
            {
                object a = GetValueFromSlot((int)NodeSlotId.VarInStartClosed);
                bool state = a != null ? (bool)a : true;
                memoryItem = context.CurrentFrame.Allocate(Id, state);
            }

            bool val = (bool)memoryItem.Value;

            if (slot.Id == (int)NodeSlotId.InEnter)
            {
                if (val)
                {
                    ActivateOutputLink(context, (int)NodeSlotId.Out);
                }
            }
            else if (slot.Id == (int)NodeSlotId.InOpen)
            {
                memoryItem.Value = true;
            }
            else if (slot.Id == (int)NodeSlotId.InClose)
            {
                memoryItem.Value = false;
            }
            else if (slot.Id == (int)NodeSlotId.InToggle)
            {
                memoryItem.Value = !val;
            }

            return info;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override SequenceNode CopyImpl()
        {
            return new GateNode();
        }
    }
}
