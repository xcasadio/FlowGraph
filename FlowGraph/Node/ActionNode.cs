using System.Xml;
using FlowGraph.Process;

namespace FlowGraph.Node
{
    public abstract class ActionNode : SequenceNode
    {
#if EDITOR
        public override NodeType NodeType => NodeType.Action;

        public ActionNode()
        {
        }
#endif

        public enum LogicState
        {
            Ok,
            Warning,
            Error
        }

        public struct ProcessingInfo
        {
            public LogicState State;
            public string ErrorMessage;
        }

        private string _errorMessage = "";
        private ProcessingInfo _state;

        public ProcessingInfo State
        {
            get => _state;
            private set
            {
                _state = value;
                OnPropertyChanged("State");
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            internal set
            {
                _errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }

        protected ActionNode(XmlNode node)
            : base(node)
        {

        }

        protected override void InitializeSlots()
        {
            SlotFlag = SlotAvailableFlag.All;
        }

        public ProcessingInfo Activate(ProcessingContext context, NodeSlot slot)
        {
            State = ActivateLogic(context, slot);
            return State;
        }

        public abstract ProcessingInfo ActivateLogic(ProcessingContext context, NodeSlot slot);
    }
}
