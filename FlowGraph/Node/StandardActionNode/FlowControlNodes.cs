using System.Xml;
using FlowGraph.Logger;
using FlowGraph.Process;

namespace FlowGraph.Node.StandardActionNode
{
    [Category("Flow Control"), Name("Branch")]
    public class BranchNode : ActionNode
    {
        public enum NodeSlotId
        {
            In,
            OutTrue,
            OutFalse,
            VarCond,
        }

        public override string Title => "Branch";

        public BranchNode(XmlNode node)
            : base(node) { }

        public BranchNode()
        { }

        protected override void InitializeSlots()
        {
            base.InitializeSlots();

            AddSlot((int)NodeSlotId.In, "", SlotType.NodeIn);
            AddSlot((int)NodeSlotId.VarCond, "Condition", SlotType.VarIn, typeof(bool));

            AddSlot((int)NodeSlotId.OutTrue, "True", SlotType.NodeOut);
            AddSlot((int)NodeSlotId.OutFalse, "False", SlotType.NodeOut);
        }

        public override ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot)
        {
            var info = new ProcessingInfo
            {
                State = LogicState.Ok
            };

            var objCond = GetValueFromSlot((int)NodeSlotId.VarCond);

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
                var cond = (bool)objCond;

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

        protected override SequenceNode CopyImpl()
        {
            return new BranchNode();
        }
    }

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

        public DoNNode(XmlNode node)
            : base(node) { }

        public DoNNode()
        { }

        protected override void InitializeSlots()
        {
            base.InitializeSlots();

            AddSlot((int)NodeSlotId.InEnter, "Enter", SlotType.NodeIn);
            AddSlot((int)NodeSlotId.VarInN, "N", SlotType.VarIn, typeof(int));
            AddSlot((int)NodeSlotId.InReset, "Reset", SlotType.NodeIn);

            AddSlot((int)NodeSlotId.Out, "Exit", SlotType.NodeOut);
            AddSlot((int)NodeSlotId.VarCounter, "Counter", SlotType.VarOut, typeof(int));
        }

        public override ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot)
        {
            var info = new ProcessingInfo
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
                var memoryItem = context.CurrentFrame.GetValueFromId(Id);

                if (_isInitial == false)
                {
                    var objN = GetValueFromSlot((int)NodeSlotId.VarInN);

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
                        var n = (int)objN;

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

        protected override SequenceNode CopyImpl()
        {
            return new DoNNode();
        }
    }

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

        public override string? Title => "Do Once";

        public DoOnceNode(XmlNode node) : base(node)
        { }

        public DoOnceNode()
        { }

        protected override void InitializeSlots()
        {
            base.InitializeSlots();

            AddSlot((int)NodeSlotId.InEnter, "", SlotType.NodeIn);
            AddSlot((int)NodeSlotId.InReset, "Reset", SlotType.NodeIn);
            AddSlot((int)NodeSlotId.Out, "Completed", SlotType.NodeOut);
        }

        public override ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot)
        {
            var info = new ProcessingInfo
            {
                State = LogicState.Ok
            };

            var memoryItem = context.CurrentFrame.GetValueFromId(Id);

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

        protected override SequenceNode CopyImpl()
        {
            return new DoOnceNode();
        }
    }

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

        public override string? Title => "Flip Flop";

        public FlipFlopNode(XmlNode node)
            : base(node) { }

        public FlipFlopNode()
        { }

        protected override void InitializeSlots()
        {
            base.InitializeSlots();

            AddSlot((int)NodeSlotId.InEnter, "", SlotType.NodeIn);
            AddSlot((int)NodeSlotId.OutA, "A", SlotType.NodeOut);
            AddSlot((int)NodeSlotId.OutB, "B", SlotType.NodeOut);
            AddSlot((int)NodeSlotId.VarOutIsA, "IsA", SlotType.VarOut, typeof(bool));
        }

        public override ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot)
        {
            var info = new ProcessingInfo
            {
                State = LogicState.Ok
            };

            var memoryItem = context.CurrentFrame.GetValueFromId(Id);

            if (memoryItem == null)
            {
                memoryItem = context.CurrentFrame.Allocate(Id, true);
            }

            if (slot.Id == (int)NodeSlotId.InEnter)
            {
                var val = (bool)memoryItem.Value;
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

        protected override SequenceNode CopyImpl()
        {
            return new FlipFlopNode();
        }
    }

    [Category("Flow Control"), Name("For Loop")]
    public class ForLoopNode :
        ActionNode
    {
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

        public override string? Title => "For Loop";

        public ForLoopNode(XmlNode node) : base(node) { }

        public ForLoopNode() { }

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

        public override ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot)
        {
            var info = new ProcessingInfo
            {
                State = LogicState.Ok
            };

            if (slot.Id == (int)NodeSlotId.In)
            {
                int firstIndex = 0, lastIndex = -1;

                var objFirstIndex = GetValueFromSlot((int)NodeSlotId.VarInFirstIndex);

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

                var objLastIndex = GetValueFromSlot((int)NodeSlotId.VarInLastIndex);

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

                var memoryItem = context.CurrentFrame.GetValueFromId(Id);

                if (memoryItem == null)
                {
                    memoryItem = context.CurrentFrame.Allocate(Id, new ForLoopNodeInfo { Counter = firstIndex, IsWaitingLoopBody = false });
                }

                var memoryInfo = (ForLoopNodeInfo)memoryItem.Value;

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
                        var newContext = context.PushNewContext();
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

        void OnLoopBodyFinished(object sender, EventArgs e)
        {
            var context = sender as ProcessingContext;
            context.Finished -= OnLoopBodyFinished;

            var memoryItem = context.Parent.CurrentFrame.GetValueFromId(Id);
            var memoryInfo = (ForLoopNodeInfo)memoryItem.Value;
            memoryInfo.IsWaitingLoopBody = false;
            memoryItem.Value = memoryInfo;
        }

        protected override SequenceNode CopyImpl()
        {
            return new ForLoopNode();
        }
    }

    [Category("Flow Control"), Name("For Loop With Break")]
    public class ForLoopWithBreakNode :
        ActionNode
    {

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

        public override string? Title => "For Loop With Break";

        public ForLoopWithBreakNode(XmlNode node) : base(node)
        { }

        public ForLoopWithBreakNode()
        { }

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

        public override ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot)
        {
            var info = new ProcessingInfo
            {
                State = LogicState.Ok
            };

            if (slot.Id == (int)NodeSlotId.In)
            {
                int firstIndex = 0, lastIndex = -1;

                var objFirstIndex = GetValueFromSlot((int)NodeSlotId.VarInFirstIndex);

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

                var objLastIndex = GetValueFromSlot((int)NodeSlotId.VarInLastIndex);

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

                var memoryItem = context.CurrentFrame.GetValueFromId(Id);

                if (memoryItem == null)
                {
                    memoryItem = context.CurrentFrame.Allocate(Id, new ForLoopNodeInfo { Counter = firstIndex, IsWaitingLoopBody = false });
                }

                var memoryInfo = (ForLoopNodeInfo)memoryItem.Value;

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

                        var newContext = context.PushNewContext();
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
                var memoryItem = context.CurrentFrame.GetValueFromId(Id);
                var memoryInfo = (ForLoopNodeInfo)memoryItem.Value;
                context.RemoveExecution(context, memoryInfo.Step);
                context.CurrentFrame.Deallocate(Id);
                ActivateOutputLink(context, (int)NodeSlotId.OutCompleted);
            }

            return info;
        }

        void OnLoopBodyFinished(object sender, EventArgs e)
        {
            var context = sender as ProcessingContext;
            context.Finished -= OnLoopBodyFinished;

            var memoryItem = context.Parent.CurrentFrame.GetValueFromId(Id);
            if (memoryItem != null)
            {
                var memoryInfo = (ForLoopNodeInfo)memoryItem.Value;
                memoryInfo.IsWaitingLoopBody = false;
                memoryItem.Value = memoryInfo;
            }
        }

        protected override SequenceNode CopyImpl()
        {
            return new ForLoopWithBreakNode();
        }
    }

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

        public override string? Title => "Gate";

        public GateNode(XmlNode node) : base(node)
        { }

        public GateNode()
        { }

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

        public override ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot)
        {
            var info = new ProcessingInfo
            {
                State = LogicState.Ok
            };

            var memoryItem = context.CurrentFrame.GetValueFromId(Id);

            if (memoryItem == null)
            {
                var a = GetValueFromSlot((int)NodeSlotId.VarInStartClosed);
                var state = a != null ? (bool)a : true;
                memoryItem = context.CurrentFrame.Allocate(Id, state);
            }

            var val = (bool)memoryItem.Value;

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

        protected override SequenceNode CopyImpl()
        {
            return new GateNode();
        }
    }
}
