using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSyntax
{
    public abstract partial class MemberDeclarationSyntax : SyntaxTriviaNode
    {
        internal MemberDeclarationSyntax(SyntaxKind syntaxKind)
            : base(syntaxKind)
        {
        }
    }
}
