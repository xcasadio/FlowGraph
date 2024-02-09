using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class TypeArgumentListSyntax : SyntaxNode
    {
        public SyntaxList<TypeSyntax> Arguments { get; private set; }
        
        public TypeArgumentListSyntax()
            : base(SyntaxKind.TypeArgumentList)
        {
            Arguments = new SyntaxList<TypeSyntax>(this);
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var item in Arguments)
            {
                yield return item;
            }
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitTypeArgumentList(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitTypeArgumentList(this);
        }
    }
}
