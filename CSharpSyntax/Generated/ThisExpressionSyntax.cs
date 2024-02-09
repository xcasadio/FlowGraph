using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class ThisExpressionSyntax : InstanceExpressionSyntax
    {
        public ThisExpressionSyntax()
            : base(SyntaxKind.ThisExpression)
        {
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitThisExpression(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitThisExpression(this);
        }
    }
}
