using System.Collections.Generic;

namespace FlowGraphBase.Process
{
    /// <summary>
    /// 
    /// </summary>
    public class MemoryStackFrameManager
    {
        readonly Stack<MemoryStack> _StackFrames = new Stack<MemoryStack>(2);

        /// <summary>
        /// 
        /// </summary>
        public MemoryStack CurrentFrame => _StackFrames.Count == 0 ? null : _StackFrames.Peek();

        /// <summary>
        /// 
        /// </summary>
        public void AddStackFrame()
        {
            _StackFrames.Push(new MemoryStack());
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveStackFrame()
        {
            _StackFrames.Pop();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            _StackFrames.Clear();
        }
    }
}
