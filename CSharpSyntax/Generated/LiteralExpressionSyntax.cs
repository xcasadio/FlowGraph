using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class LiteralExpressionSyntax : ExpressionSyntax
    {
        public object Value { get; set; }
        
        public LiteralExpressionSyntax()
            : base(SyntaxKind.LiteralExpression)
        {
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitLiteralExpression(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitLiteralExpression(this);
        }
    }
}
