using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class ForStatementSyntax : StatementSyntax
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
        
        public SyntaxList<ExpressionSyntax> Incrementors { get; private set; }
        
        public SyntaxList<ExpressionSyntax> Initializers { get; private set; }
        
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
        
        public ForStatementSyntax()
            : base(SyntaxKind.ForStatement)
        {
            Incrementors = new SyntaxList<ExpressionSyntax>(this);
            Initializers = new SyntaxList<ExpressionSyntax>(this);
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var child in base.ChildNodes())
            {
                yield return child;
            }
            
            if (Condition != null)
                yield return Condition;
            
            if (Declaration != null)
                yield return Declaration;
            
            foreach (var item in Incrementors)
            {
                yield return item;
            }
            
            foreach (var item in Initializers)
            {
                yield return item;
            }
            
            if (Statement != null)
                yield return Statement;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitForStatement(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitForStatement(this);
        }
    }
}
