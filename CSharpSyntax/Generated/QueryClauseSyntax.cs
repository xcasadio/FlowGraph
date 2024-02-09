using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSyntax
{
    public abstract partial class QueryClauseSyntax : SyntaxNode
    {
        internal QueryClauseSyntax(SyntaxKind syntaxKind)
            : base(syntaxKind)
        {
        }
    }
}
