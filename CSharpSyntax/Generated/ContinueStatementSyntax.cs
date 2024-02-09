using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class ContinueStatementSyntax : StatementSyntax
    {
        public ContinueStatementSyntax()
            : base(SyntaxKind.ContinueStatement)
        {
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitContinueStatement(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitContinueStatement(this);
        }
    }
}
