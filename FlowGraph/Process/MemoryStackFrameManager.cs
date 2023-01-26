namespace FlowGraph.Process
{
    public class MemoryStackFrameManager
    {
        readonly Stack<MemoryStack> _stackFrames = new(2);

        public MemoryStack? CurrentFrame => _stackFrames.Count == 0 ? null : _stackFrames.Peek();

        public void AddStackFrame()
        {
            _stackFrames.Push(new MemoryStack());
        }

        public void RemoveStackFrame()
        {
            _stackFrames.Pop();
        }

        public void Clear()
        {
            _stackFrames.Clear();
        }
    }
}
