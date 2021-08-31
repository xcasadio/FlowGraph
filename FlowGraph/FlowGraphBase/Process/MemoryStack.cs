using System.Collections.Generic;

namespace FlowGraphBase.Process
{
    /// <summary>
    /// 
    /// </summary>
    public class MemoryStack
    {
        private int _FreeID = int.MaxValue;
        private readonly Dictionary<int, MemoryStackItem> _Variable = new Dictionary<int, MemoryStackItem>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_"></param>
        /// <param name="val_"></param>
        /// <returns></returns>
        public MemoryStackItem Allocate(int id_, object val_)
        {
            MemoryStackItem item = new MemoryStackItem(id_, val_);
            _Variable.Add(id_, item);
            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_"></param>
        /// <param name="val_"></param>
        /// <returns></returns>
        public MemoryStackItem Allocate(object val_)
        {
            int id = GetUnusedId();

            MemoryStackItem item = new MemoryStackItem(id, val_);
            _Variable.Add(id, item);
            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ite_"></param>
        public void Deallocate(MemoryStackItem ite_)
        {
            Deallocate(ite_.ID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_"></param>
        public void Deallocate(int id_)
        {
            _Variable.Remove(id_);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_"></param>
        /// <returns></returns>
        public MemoryStackItem GetValueFromID(int id_)
        {
            MemoryStackItem item;

            if (_Variable.TryGetValue(id_, out item) == false)
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
            _FreeID--;
            return _FreeID;
        }
    }
}
