using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class ElseClauseSyntax : SyntaxNode
    {
        private StatementSyntax _statement;
        public StatementSyntax Statement
        {
            get { return _statement; }
            set
            {
                if (_statement != null)
                    RemoveChild(_statement);
                
                _statement = value;
                
                if (_statement != null)
                    AddChild(_statement);
            }
        }
        
        public ElseClauseSyntax()
            : base(SyntaxKind.ElseClause)
        {
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            if (Statement != null)
                yield return Statement;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitElseClause(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitElseClause(this);
        }
    }
}
