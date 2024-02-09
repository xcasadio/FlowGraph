using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class ArrayRankSpecifierSyntax : SyntaxNode
    {
        public SyntaxList<ExpressionSyntax> Sizes { get; private set; }
        
        public ArrayRankSpecifierSyntax()
            : base(SyntaxKind.ArrayRankSpecifier)
        {
            Sizes = new SyntaxList<ExpressionSyntax>(this);
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var item in Sizes)
            {
                yield return item;
            }
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitArrayRankSpecifier(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitArrayRankSpecifier(this);
        }
    }
}
