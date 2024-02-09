using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class ConditionalExpressionSyntax : ExpressionSyntax
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
        
        private ExpressionSyntax _whenFalse;
        public ExpressionSyntax WhenFalse
        {
            get { return _whenFalse; }
            set
            {
                if (_whenFalse != null)
                    RemoveChild(_whenFalse);
                
                _whenFalse = value;
                
                if (_whenFalse != null)
                    AddChild(_whenFalse);
            }
        }
        
        private ExpressionSyntax _whenTrue;
        public ExpressionSyntax WhenTrue
        {
            get { return _whenTrue; }
            set
            {
                if (_whenTrue != null)
                    RemoveChild(_whenTrue);
                
                _whenTrue = value;
                
                if (_whenTrue != null)
                    AddChild(_whenTrue);
            }
        }
        
        public ConditionalExpressionSyntax()
            : base(SyntaxKind.ConditionalExpression)
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
            
            if (WhenFalse != null)
                yield return WhenFalse;
            
            if (WhenTrue != null)
                yield return WhenTrue;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitConditionalExpression(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitConditionalExpression(this);
        }
    }
}
