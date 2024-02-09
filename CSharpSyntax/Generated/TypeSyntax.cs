using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSyntax
{
    public abstract partial class TypeSyntax : ExpressionSyntax
    {
        internal TypeSyntax(SyntaxKind syntaxKind)
            : base(syntaxKind)
        {
        }
    }
}
