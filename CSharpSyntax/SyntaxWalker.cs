using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public class SyntaxWalker : SyntaxVisitorBase
    {
        [DebuggerStepThrough]
        public override void DefaultVisit(SyntaxNode node)
        {
            foreach (var child in node.ChildNodes())
            {
                child.Accept(this);
            }
        }
    }
}
