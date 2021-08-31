using System.Collections.Generic;

namespace FlowGraphBase.Process
{
    /// <summary>
    /// 
    /// </summary>
    public class MemoryStack
    {
        #region Fields

        private int m_FreeID = int.MaxValue;
        private Dictionary<int, MemoryStackItem> m_Variable = new Dictionary<int, MemoryStackItem>();

        #endregion //Fields

        #region Properties

        #endregion //Properties

        #region Constructors

        #endregion //Constructors

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_"></param>
        /// <param name="val_"></param>
        /// <returns></returns>
        public MemoryStackItem Allocate(int id_, object val_)
        {
            MemoryStackItem item = new MemoryStackItem(id_, val_);
            m_Variable.Add(id_, item);
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
            m_Variable.Add(id, item);
            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item_"></param>
        public void Deallocate(MemoryStackItem item_)
        {
            Deallocate(item_.ID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_"></param>
        public void Deallocate(int id_)
        {
            m_Variable.Remove(id_);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_"></param>
        /// <returns></returns>
        public MemoryStackItem GetValueFromID(int id_)
        {
            MemoryStackItem item;

            if (m_Variable.TryGetValue(id_, out item) == false)
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
            m_FreeID--;
            return m_FreeID;
        }

        #endregion //Methods
    }
}
