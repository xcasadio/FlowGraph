using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CSharpSyntax
{
    public abstract class SyntaxNode
    {
        private static readonly SyntaxNode[] EmptyList = new SyntaxNode[0];

        public abstract void Accept(ISyntaxVisitor visitor);

        public abstract T Accept<T>(ISyntaxVisitor<T> visitor);

        public SyntaxNode Parent { get; private set; }

        public SyntaxKind SyntaxKind { get; private set; }

        protected SyntaxNode(SyntaxKind syntaxKind)
        {
            SyntaxKind = syntaxKind;
        }

        internal protected void AddChild(SyntaxNode item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            if (item.Parent != null)
                throw new InvalidOperationException("Syntax node has already been parented");

            item.Parent = this;
        }

        internal protected void RemoveChild(SyntaxNode item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            Debug.Assert(item.Parent == this);

            item.Parent = null;
        }

        public virtual IEnumerable<SyntaxNode> ChildNodes()
        {
            return EmptyList;
        }

        internal virtual void Validate()
        {
        }

        internal void ValidateNotNull(object value, string name)
        {
            if (value == null)
            {
                throw new CSharpSyntaxException(String.Format(
                    "Property '{0}' of node '{1}' cannot be null",
                    name, this
                ));
            }
        }

        internal void ValidateNotEmpty<T>(SyntaxList<T> list, string name)
            where T : SyntaxNode
        {
            if (list.Count == 0)
            {
                throw new CSharpSyntaxException(String.Format(
                    "List '{0}' of node '{1}' cannot be empty",
                    name, this
                ));
            }
        }

        internal void ValidateModifiers(Modifiers modifiers, Modifiers allowed)
        {
            if ((modifiers & ~allowed) != 0)
            {
                throw new CSharpSyntaxException(String.Format(
                    "Modifier '{0}' are illegal for node '{1}'",
                    modifiers, this
                ));
            }
        }
    }
}
