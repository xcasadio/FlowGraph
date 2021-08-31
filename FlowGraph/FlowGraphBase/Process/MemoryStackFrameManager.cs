using System.Collections.Generic;

namespace FlowGraphBase.Process
{
    /// <summary>
    /// 
    /// </summary>
    public class MemoryStackFrameManager
    {
        readonly Stack<MemoryStack> _stackFrames = new Stack<MemoryStack>(2);

        /// <summary>
        /// 
        /// </summary>
        public MemoryStack CurrentFrame => _stackFrames.Count == 0 ? null : _stackFrames.Peek();

        /// <summary>
        /// 
        /// </summary>
        public void AddStackFrame()
        {
            _stackFrames.Push(new MemoryStack());
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveStackFrame()
        {
            _stackFrames.Pop();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            _stackFrames.Clear();
        }
    }
}
