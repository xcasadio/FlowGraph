using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CSharpSyntax
{
    public class SyntaxList<T> : Collection<T>
        where T : SyntaxNode
    {
        public SyntaxNode Parent { get; private set; }

        public SyntaxList(SyntaxNode parent)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");

            Parent = parent;
        }

        protected override void ClearItems()
        {
            foreach (var item in this)
            {
                Parent.RemoveChild(item);
            }

            base.ClearItems();
        }

        protected override void InsertItem(int index, T item)
        {
            Parent.AddChild(item);

            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            Parent.RemoveChild(this[index]);

            base.RemoveItem(index);
        }

        protected override void SetItem(int index, T item)
        {
            Parent.RemoveChild(this[index]);
            Parent.AddChild(item);

            base.SetItem(index, item);
        }
    }
}
