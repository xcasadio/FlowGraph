using System.Xml;
using FlowGraph.Process;

namespace FlowGraph.Node
{
#if EDITOR
    public enum VariableControlType
    {
        Numeric,
        Selectable,
        Checkable,
        Text,
        ReadOnly,
        Custom
    }
#endif

    public abstract class VariableNode : SequenceNode
    {
#if EDITOR
        public override NodeType NodeType => NodeType.Variable;
        public NodeSlot VariableSlot => NodeSlots[0];
        public override string Title => "";

        public VariableNode()
        {
        }

        public override void Save(XmlNode node)
        {
            base.Save(node);

            XmlNode valueNode = node.OwnerDocument!.CreateElement("Value");
            node.AppendChild(valueNode);
            SaveValue(valueNode);
        }

        protected abstract object LoadValue(XmlNode node);

        protected abstract void SaveValue(XmlNode node);
#endif

        protected VariableNode(XmlNode node)
            : base(node)
        {
        }

        protected override void InitializeSlots()
        {
            SlotFlag = SlotAvailableFlag.DefaultFlagVariable;
        }

        public abstract object? Value
        {
            get;
            set;
        }

        public void Allocate(MemoryStack memoryStack)
        {
            // TODO : create function => object abstract CopyValue(object val_) ?????
            // do this only to copy the value
            var clone = (VariableNode)CopyImpl();
            memoryStack.Allocate(Id, clone.Value);
        }

        public void Deallocate(MemoryStack memoryStack)
        {
            memoryStack.Deallocate(Id);
        }
    }
}
