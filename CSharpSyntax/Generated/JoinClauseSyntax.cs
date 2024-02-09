using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class JoinClauseSyntax : QueryClauseSyntax
    {
        public string Identifier { get; set; }
        
        private ExpressionSyntax _inExpression;
        public ExpressionSyntax InExpression
        {
            get { return _inExpression; }
            set
            {
                if (_inExpression != null)
                    RemoveChild(_inExpression);
                
                _inExpression = value;
                
                if (_inExpression != null)
                    AddChild(_inExpression);
            }
        }
        
        private JoinIntoClauseSyntax _into;
        public JoinIntoClauseSyntax Into
        {
            get { return _into; }
            set
            {
                if (_into != null)
                    RemoveChild(_into);
                
                _into = value;
                
                if (_into != null)
                    AddChild(_into);
            }
        }
        
        private ExpressionSyntax _leftExpression;
        public ExpressionSyntax LeftExpression
        {
            get { return _leftExpression; }
            set
            {
                if (_leftExpression != null)
                    RemoveChild(_leftExpression);
                
                _leftExpression = value;
                
                if (_leftExpression != null)
                    AddChild(_leftExpression);
            }
        }
        
        private ExpressionSyntax _rightExpression;
        public ExpressionSyntax RightExpression
        {
            get { return _rightExpression; }
            set
            {
                if (_rightExpression != null)
                    RemoveChild(_rightExpression);
                
                _rightExpression = value;
                
                if (_rightExpression != null)
                    AddChild(_rightExpression);
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
        
        public JoinClauseSyntax()
            : base(SyntaxKind.JoinClause)
        {
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var child in base.ChildNodes())
            {
                yield return child;
            }
            
            if (InExpression != null)
                yield return InExpression;
            
            if (Into != null)
                yield return Into;
            
            if (LeftExpression != null)
                yield return LeftExpression;
            
            if (RightExpression != null)
                yield return RightExpression;
            
            if (Type != null)
                yield return Type;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitJoinClause(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitJoinClause(this);
        }
    }
}
