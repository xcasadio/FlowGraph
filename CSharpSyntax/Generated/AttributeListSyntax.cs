using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class AttributeListSyntax : SyntaxNode
    {
        public SyntaxList<AttributeSyntax> Attributes { get; private set; }
        
        public AttributeTarget Target { get; set; }
        
        public AttributeListSyntax()
            : base(SyntaxKind.AttributeList)
        {
            Attributes = new SyntaxList<AttributeSyntax>(this);
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var item in Attributes)
            {
                yield return item;
            }
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitAttributeList(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitAttributeList(this);
        }
    }
}
