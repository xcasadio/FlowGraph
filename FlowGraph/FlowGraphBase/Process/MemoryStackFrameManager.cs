using System.Collections.Generic;

namespace FlowGraphBase.Process
{
    /// <summary>
    /// 
    /// </summary>
    public class MemoryStackFrameManager
    {
        #region Fields

        readonly Stack<MemoryStack> _StackFrames = new Stack<MemoryStack>(2);

        #endregion //Fields

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public MemoryStack CurrentFrame => _StackFrames.Count == 0 ? null : _StackFrames.Peek();

        #endregion //Properties

        #region Constructors

        #endregion //Constructors

        #region Methods

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

        #endregion //Methods
    }
}
