using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class IfStatementSyntax : StatementSyntax
    {
        private ExpressionSyntax _condition;
        public ExpressionSyntax Condition
        {
            get { return _condition; }
            set
            {
                if (_condition != null)
                    RemoveChild(_condition);
                
                _condition = value;
                
                if (_condition != null)
                    AddChild(_condition);
            }
        }
        
        private ElseClauseSyntax _else;
        public ElseClauseSyntax Else
        {
            get { return _else; }
            set
            {
                if (_else != null)
                    RemoveChild(_else);
                
                _else = value;
                
                if (_else != null)
                    AddChild(_else);
            }
        }
        
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
        
        public IfStatementSyntax()
            : base(SyntaxKind.IfStatement)
        {
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var child in base.ChildNodes())
            {
                yield return child;
            }
            
            if (Condition != null)
                yield return Condition;
            
            if (Else != null)
                yield return Else;
            
            if (Statement != null)
                yield return Statement;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitIfStatement(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitIfStatement(this);
        }
    }
}
