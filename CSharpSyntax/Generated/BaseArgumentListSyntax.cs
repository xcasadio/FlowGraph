using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSyntax
{
    public abstract partial class BaseArgumentListSyntax : SyntaxNode
    {
        public SyntaxList<ArgumentSyntax> Arguments { get; private set; }
        
        internal BaseArgumentListSyntax(SyntaxKind syntaxKind)
            : base(syntaxKind)
        {
            Arguments = new SyntaxList<ArgumentSyntax>(this);
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var item in Arguments)
            {
                yield return item;
            }
        }
    }
}
