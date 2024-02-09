using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class BaseListSyntax : SyntaxNode
    {
        public SyntaxList<TypeSyntax> Types { get; private set; }
        
        public BaseListSyntax()
            : base(SyntaxKind.BaseList)
        {
            Types = new SyntaxList<TypeSyntax>(this);
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var item in Types)
            {
                yield return item;
            }
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitBaseList(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitBaseList(this);
        }
    }
}
