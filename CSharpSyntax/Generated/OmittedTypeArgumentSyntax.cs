using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class OmittedTypeArgumentSyntax : TypeSyntax
    {
        public OmittedTypeArgumentSyntax()
            : base(SyntaxKind.OmittedTypeArgument)
        {
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitOmittedTypeArgument(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitOmittedTypeArgument(this);
        }
    }
}
