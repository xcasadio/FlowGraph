using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class AttributeArgumentListSyntax : SyntaxNode
    {
        public SyntaxList<AttributeArgumentSyntax> Arguments { get; private set; }
        
        public AttributeArgumentListSyntax()
            : base(SyntaxKind.AttributeArgumentList)
        {
            Arguments = new SyntaxList<AttributeArgumentSyntax>(this);
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
                visitor.VisitAttributeArgumentList(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitAttributeArgumentList(this);
        }
    }
}
