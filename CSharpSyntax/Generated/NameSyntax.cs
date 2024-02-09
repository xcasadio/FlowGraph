using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSyntax
{
    public abstract partial class NameSyntax : TypeSyntax
    {
        internal NameSyntax(SyntaxKind syntaxKind)
            : base(syntaxKind)
        {
        }
    }
}
