using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class LockStatementSyntax : StatementSyntax
    {
        private ExpressionSyntax _expression;
        public ExpressionSyntax Expression
        {
            get { return _expression; }
            set
            {
                if (_expression != null)
                    RemoveChild(_expression);
                
                _expression = value;
                
                if (_expression != null)
                    AddChild(_expression);
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
        
        public LockStatementSyntax()
            : base(SyntaxKind.LockStatement)
        {
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var child in base.ChildNodes())
            {
                yield return child;
            }
            
            if (Expression != null)
                yield return Expression;
            
            if (Statement != null)
                yield return Statement;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitLockStatement(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitLockStatement(this);
        }
    }
}
