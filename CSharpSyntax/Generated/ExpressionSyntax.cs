using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSyntax
{
    public abstract partial class ExpressionSyntax : SyntaxNode
    {
        internal ExpressionSyntax(SyntaxKind syntaxKind)
            : base(syntaxKind)
        {
        }
    }
}
