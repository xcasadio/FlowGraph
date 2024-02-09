using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class BracketedParameterListSyntax : BaseParameterListSyntax
    {
        public BracketedParameterListSyntax()
            : base(SyntaxKind.BracketedParameterList)
        {
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitBracketedParameterList(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitBracketedParameterList(this);
        }
    }
}
