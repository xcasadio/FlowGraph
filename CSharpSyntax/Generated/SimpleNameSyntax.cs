using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSyntax
{
    public abstract partial class SimpleNameSyntax : NameSyntax
    {
        public string Identifier { get; set; }
        
        internal SimpleNameSyntax(SyntaxKind syntaxKind)
            : base(syntaxKind)
        {
        }
    }
}
