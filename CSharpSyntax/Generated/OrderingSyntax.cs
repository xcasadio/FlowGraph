using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class OrderingSyntax : SyntaxNode
    {
        public AscendingOrDescending Kind { get; set; }
        
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
        
        public OrderingSyntax()
            : base(SyntaxKind.Ordering)
        {
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            if (Expression != null)
                yield return Expression;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitOrdering(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitOrdering(this);
        }
    }
}
