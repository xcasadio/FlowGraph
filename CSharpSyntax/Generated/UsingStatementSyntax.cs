using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class UsingStatementSyntax : StatementSyntax
    {
        private VariableDeclarationSyntax _declaration;
        public VariableDeclarationSyntax Declaration
        {
            get { return _declaration; }
            set
            {
                if (_declaration != null)
                    RemoveChild(_declaration);
                
                _declaration = value;
                
                if (_declaration != null)
                    AddChild(_declaration);
            }
        }
        
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
        
        public UsingStatementSyntax()
            : base(SyntaxKind.UsingStatement)
        {
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var child in base.ChildNodes())
            {
                yield return child;
            }
            
            if (Declaration != null)
                yield return Declaration;
            
            if (Expression != null)
                yield return Expression;
            
            if (Statement != null)
                yield return Statement;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitUsingStatement(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitUsingStatement(this);
        }
    }
}
