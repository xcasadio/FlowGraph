using System.Collections.Generic;

namespace FlowGraphBase.Process
{
    /// <summary>
    /// 
    /// </summary>
    public class MemoryStack
    {
        private int _freeId = int.MaxValue;
        private readonly Dictionary<int, MemoryStackItem> _variable = new Dictionary<int, MemoryStackItem>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_"></param>
        /// <param name="val_"></param>
        /// <returns></returns>
        public MemoryStackItem Allocate(int id, object val)
        {
            MemoryStackItem item = new MemoryStackItem(id, val);
            _variable.Add(id, item);
            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_"></param>
        /// <param name="val_"></param>
        /// <returns></returns>
        public MemoryStackItem Allocate(object val)
        {
            int id = GetUnusedId();

            MemoryStackItem item = new MemoryStackItem(id, val);
            _variable.Add(id, item);
            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ite_"></param>
        public void Deallocate(MemoryStackItem ite)
        {
            Deallocate(ite.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_"></param>
        public void Deallocate(int id)
        {
            _variable.Remove(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_"></param>
        /// <returns></returns>
        public MemoryStackItem GetValueFromId(int id)
        {
            MemoryStackItem item;

            if (_variable.TryGetValue(id, out item) == false)
            {
//                 LogManager.Instance.WriteLine(LogVerbosity.Error,
//                     "MemoryStack.GetValueFromID() can't find the memory with the id {0}", id_);
            }

            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int GetUnusedId()
        {
            _freeId--;
            return _freeId;
        }
    }
}
