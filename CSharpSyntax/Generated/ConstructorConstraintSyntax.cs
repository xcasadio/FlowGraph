using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class ConstructorConstraintSyntax : TypeParameterConstraintSyntax
    {
        public ConstructorConstraintSyntax()
            : base(SyntaxKind.ConstructorConstraint)
        {
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitConstructorConstraint(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitConstructorConstraint(this);
        }
    }
}
