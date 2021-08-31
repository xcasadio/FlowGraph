using System.Collections.Generic;

namespace FlowGraphBase.Process
{
    /// <summary>
    /// 
    /// </summary>
    public class MemoryStackFrameManager
    {
        #region Fields

        Stack<MemoryStack> m_StackFrames = new Stack<MemoryStack>(2);

        #endregion //Fields

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public MemoryStack CurrentFrame => m_StackFrames.Count == 0 ? null : m_StackFrames.Peek();

        #endregion //Properties

        #region Constructors

        #endregion //Constructors

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public void AddStackFrame()
        {
            m_StackFrames.Push(new MemoryStack());
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveStackFrame()
        {
            m_StackFrames.Pop();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            m_StackFrames.Clear();
        }

        #endregion //Methods
    }
}
