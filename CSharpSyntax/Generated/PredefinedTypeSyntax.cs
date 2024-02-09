using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class PredefinedTypeSyntax : TypeSyntax
    {
        public PredefinedType Type { get; set; }
        
        public PredefinedTypeSyntax()
            : base(SyntaxKind.PredefinedType)
        {
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitPredefinedType(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitPredefinedType(this);
        }
    }
}
