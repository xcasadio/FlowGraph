using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class AnonymousObjectCreationExpressionSyntax : ExpressionSyntax
    {
        public SyntaxList<AnonymousObjectMemberDeclaratorSyntax> Initializers { get; private set; }
        
        public AnonymousObjectCreationExpressionSyntax()
            : base(SyntaxKind.AnonymousObjectCreationExpression)
        {
            Initializers = new SyntaxList<AnonymousObjectMemberDeclaratorSyntax>(this);
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var child in base.ChildNodes())
            {
                yield return child;
            }
            
            foreach (var item in Initializers)
            {
                yield return item;
            }
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitAnonymousObjectCreationExpression(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitAnonymousObjectCreationExpression(this);
        }
    }
}
