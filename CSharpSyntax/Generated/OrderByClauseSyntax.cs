using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class OrderByClauseSyntax : QueryClauseSyntax
    {
        public SyntaxList<OrderingSyntax> Orderings { get; private set; }
        
        public OrderByClauseSyntax()
            : base(SyntaxKind.OrderByClause)
        {
            Orderings = new SyntaxList<OrderingSyntax>(this);
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var child in base.ChildNodes())
            {
                yield return child;
            }
            
            foreach (var item in Orderings)
            {
                yield return item;
            }
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitOrderByClause(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitOrderByClause(this);
        }
    }
}
