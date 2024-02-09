using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class JoinIntoClauseSyntax : SyntaxNode
    {
        public string Identifier { get; set; }
        
        public JoinIntoClauseSyntax()
            : base(SyntaxKind.JoinIntoClause)
        {
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitJoinIntoClause(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitJoinIntoClause(this);
        }
    }
}
