using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class OmittedArraySizeExpressionSyntax : ExpressionSyntax
    {
        public OmittedArraySizeExpressionSyntax()
            : base(SyntaxKind.OmittedArraySizeExpression)
        {
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitOmittedArraySizeExpression(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitOmittedArraySizeExpression(this);
        }
    }
}
