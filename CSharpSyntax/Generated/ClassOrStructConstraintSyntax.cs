using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class ClassOrStructConstraintSyntax : TypeParameterConstraintSyntax
    {
        public ClassOrStruct Kind { get; set; }
        
        public ClassOrStructConstraintSyntax()
            : base(SyntaxKind.ClassOrStructConstraint)
        {
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitClassOrStructConstraint(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitClassOrStructConstraint(this);
        }
    }
}
