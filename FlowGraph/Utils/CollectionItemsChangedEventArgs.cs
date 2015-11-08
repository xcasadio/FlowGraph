using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Utils
{
    /// <summary>
    /// Arguments to the ItemsAdded and ItemsRemoved events.
    /// </summary>
    public class CollectionItemsChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The collection of items that changed.
        /// </summary>
        private ICollection items = null;

        public CollectionItemsChangedEventArgs(ICollection items)
        {
            this.items = items;
        }

        /// <summary>
        /// The collection of items that changed.
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
