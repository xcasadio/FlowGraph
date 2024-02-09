using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class TypeParameterSyntax : SyntaxNode
    {
        public SyntaxList<AttributeListSyntax> AttributeLists { get; private set; }
        
        public string Identifier { get; set; }
        
        public Variance Variance { get; set; }
        
        public TypeParameterSyntax()
            : base(SyntaxKind.TypeParameter)
        {
            AttributeLists = new SyntaxList<AttributeListSyntax>(this);
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var item in AttributeLists)
            {
                yield return item;
            }
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitTypeParameter(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitTypeParameter(this);
        }
    }
}
