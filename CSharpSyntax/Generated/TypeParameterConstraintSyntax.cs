using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSyntax
{
    public abstract partial class TypeParameterConstraintSyntax : SyntaxNode
    {
        internal TypeParameterConstraintSyntax(SyntaxKind syntaxKind)
            : base(syntaxKind)
        {
        }
    }
}
