using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSyntax
{
    public abstract partial class BaseParameterListSyntax : SyntaxNode
    {
        public SyntaxList<ParameterSyntax> Parameters { get; private set; }
        
        internal BaseParameterListSyntax(SyntaxKind syntaxKind)
            : base(syntaxKind)
        {
            Parameters = new SyntaxList<ParameterSyntax>(this);
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var item in Parameters)
            {
                yield return item;
            }
        }
    }
}
