using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class BracketedArgumentListSyntax : BaseArgumentListSyntax
    {
        public BracketedArgumentListSyntax()
            : base(SyntaxKind.BracketedArgumentList)
        {
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitBracketedArgumentList(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitBracketedArgumentList(this);
        }
    }
}
