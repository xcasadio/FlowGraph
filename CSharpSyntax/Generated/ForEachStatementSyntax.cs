using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class ForEachStatementSyntax : StatementSyntax
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
        
        public string Identifier { get; set; }
        
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
        
        private TypeSyntax _type;
        public TypeSyntax Type
        {
            get { return _type; }
            set
            {
                if (_type != null)
                    RemoveChild(_type);
                
                _type = value;
                
                if (_type != null)
                    AddChild(_type);
            }
        }
        
        public ForEachStatementSyntax()
            : base(SyntaxKind.ForEachStatement)
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
            
            if (Type != null)
                yield return Type;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitForEachStatement(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitForEachStatement(this);
        }
    }
}
