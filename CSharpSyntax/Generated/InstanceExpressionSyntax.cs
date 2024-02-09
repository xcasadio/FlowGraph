using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSyntax
{
    public abstract partial class InstanceExpressionSyntax : ExpressionSyntax
    {
        internal InstanceExpressionSyntax(SyntaxKind syntaxKind)
            : base(syntaxKind)
        {
        }
    }
}
