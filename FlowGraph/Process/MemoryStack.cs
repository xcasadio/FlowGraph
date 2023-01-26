namespace FlowGraph.Process
{
    public class MemoryStack
    {
        private int _freeId = int.MaxValue;
        private readonly Dictionary<int, MemoryStackItem> _variable = new();

        public MemoryStackItem Allocate(int id, object? val)
        {
            var item = new MemoryStackItem(id, val);
            _variable.Add(id, item);
            return item;
        }

        public MemoryStackItem Allocate(object? val)
        {
            var id = GetUnusedId();

            var item = new MemoryStackItem(id, val);
            _variable.Add(id, item);
            return item;
        }

        public void Deallocate(MemoryStackItem ite)
        {
            Deallocate(ite.Id);
        }

        public void Deallocate(int id)
        {
            _variable.Remove(id);
        }

        public MemoryStackItem? GetValueFromId(int id)
        {
            if (_variable.TryGetValue(id, out var item) == false)
            {
                //                 LogManager.Instance.WriteLine(LogVerbosity.Error,
                //                     "MemoryStack.GetValueFromID() can't find the memory with the id {0}", id_);
            }

            return item;
        }

        private int GetUnusedId()
        {
            _freeId--;
            return _freeId;
        }
    }
}
