using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSyntax
{
    public abstract partial class StatementSyntax : SyntaxTriviaNode
    {
        internal StatementSyntax(SyntaxKind syntaxKind)
            : base(syntaxKind)
        {
        }
    }
}
