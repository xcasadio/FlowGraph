using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CSharpSyntax
{
    public class SyntaxTriviaList : Collection<SyntaxTrivia>
    {
        public SyntaxNode Parent { get; private set; }

        public SyntaxTriviaList(SyntaxNode parent)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");

            Parent = parent;
        }

        protected override void ClearItems()
        {
            foreach (var item in this)
            {
                item.Parent = null;
            }

            base.ClearItems();
        }

        protected override void InsertItem(int index, SyntaxTrivia item)
        {
            if (item.Parent != null)
                throw new InvalidOperationException("Syntax trivia has already been parented");

            item.Parent = Parent;

            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            this[index].Parent = null;

            base.RemoveItem(index);
        }

        protected override void SetItem(int index, SyntaxTrivia item)
        {
            this[index].Parent = null;

            if (item.Parent != null)
                throw new InvalidOperationException("Syntax trivia has already been parented");

            item.Parent = Parent;

            base.SetItem(index, item);
        }
    }
}
