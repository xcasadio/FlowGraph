using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Common
{
    /// <summary>
    /// Arguments to the ItemsAdded and ItemsRemoved events.
    /// </summary>
    public class CollectionItemsEventArgs : EventArgs
    {
        /// <summary>
        /// The list of items that were cleared from the list.
        /// </summary>
        private ICollection items = null;

        public CollectionItemsEventArgs(ICollection items)
        {
            this.items = items;
        }

        /// <summary>
        /// The list of items that were cleared from the list.
        /// </summary>
        public ICollection Items
        {
            get
            {
                return items;
            }
        }
    }
}
