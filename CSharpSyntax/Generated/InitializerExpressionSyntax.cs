using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class InitializerExpressionSyntax : ExpressionSyntax
    {
        public SyntaxList<ExpressionSyntax> Expressions { get; private set; }
        
        public InitializerExpressionSyntax()
            : base(SyntaxKind.InitializerExpression)
        {
            Expressions = new SyntaxList<ExpressionSyntax>(this);
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var child in base.ChildNodes())
            {
                yield return child;
            }
            
            foreach (var item in Expressions)
            {
                yield return item;
            }
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitInitializerExpression(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitInitializerExpression(this);
        }
    }
}
