using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class ParenthesizedExpressionSyntax : ExpressionSyntax
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
        
        public ParenthesizedExpressionSyntax()
            : base(SyntaxKind.ParenthesizedExpression)
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
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitParenthesizedExpression(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitParenthesizedExpression(this);
        }
    }
}
