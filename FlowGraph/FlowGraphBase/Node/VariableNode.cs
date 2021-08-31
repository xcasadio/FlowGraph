using System.Xml;
using FlowGraphBase.Process;

namespace FlowGraphBase.Node
{
    /// <summary>
    /// 
    /// </summary>
    public abstract
#if EDITOR
 partial
#endif
    class VariableNode
        : SequenceNode
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        protected VariableNode(XmlNode node)
            : base(node)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitializeSlots()
        {
            SlotFlag = SlotAvailableFlag.DefaultFlagVariable;
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract object Value
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStack"></param>
        public void Allocate(MemoryStack memoryStack)
        {
            // TODO : create function => object abstract CopyValue(object val_) ?????
            // do this only to copy the value
            VariableNode clone = (VariableNode)CopyImpl();
            memoryStack.Allocate(Id, clone.Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memoryStack"></param>
        public void Deallocate(MemoryStack memoryStack)
        {
            memoryStack.Deallocate(Id);
        }
    }
}
