using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class InvocationExpressionSyntax : ExpressionSyntax
    {
        private ArgumentListSyntax _argumentList;
        public ArgumentListSyntax ArgumentList
        {
            get { return _argumentList; }
            set
            {
                if (_argumentList != null)
                    RemoveChild(_argumentList);
                
                _argumentList = value;
                
                if (_argumentList != null)
                    AddChild(_argumentList);
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
        
        public InvocationExpressionSyntax()
            : base(SyntaxKind.InvocationExpression)
        {
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var child in base.ChildNodes())
            {
                yield return child;
            }
            
            if (ArgumentList != null)
                yield return ArgumentList;
            
            if (Expression != null)
                yield return Expression;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitInvocationExpression(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitInvocationExpression(this);
        }
    }
}
