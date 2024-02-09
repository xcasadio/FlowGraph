using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class TypeParameterListSyntax : SyntaxNode
    {
        public SyntaxList<TypeParameterSyntax> Parameters { get; private set; }
        
        public TypeParameterListSyntax()
            : base(SyntaxKind.TypeParameterList)
        {
            Parameters = new SyntaxList<TypeParameterSyntax>(this);
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var item in Parameters)
            {
                yield return item;
            }
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitTypeParameterList(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitTypeParameterList(this);
        }
    }
}
