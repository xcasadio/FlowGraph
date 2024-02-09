using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSyntax
{
    public abstract partial class SelectOrGroupClauseSyntax : SyntaxNode
    {
        internal SelectOrGroupClauseSyntax(SyntaxKind syntaxKind)
            : base(syntaxKind)
        {
        }
    }
}
