using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class AccessorListSyntax : SyntaxNode
    {
        public SyntaxList<AccessorDeclarationSyntax> Accessors { get; private set; }
        
        public AccessorListSyntax()
            : base(SyntaxKind.AccessorList)
        {
            Accessors = new SyntaxList<AccessorDeclarationSyntax>(this);
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var item in Accessors)
            {
                yield return item;
            }
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitAccessorList(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitAccessorList(this);
        }
    }
}
