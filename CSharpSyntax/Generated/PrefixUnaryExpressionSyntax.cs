using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class PrefixUnaryExpressionSyntax : ExpressionSyntax
    {
        private ExpressionSyntax _operand;
        public ExpressionSyntax Operand
        {
            get { return _operand; }
            set
            {
                if (_operand != null)
                    RemoveChild(_operand);
                
                _operand = value;
                
                if (_operand != null)
                    AddChild(_operand);
            }
        }
        
        public PrefixUnaryOperator Operator { get; set; }
        
        public PrefixUnaryExpressionSyntax()
            : base(SyntaxKind.PrefixUnaryExpression)
        {
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var child in base.ChildNodes())
            {
                yield return child;
            }
            
            if (Operand != null)
                yield return Operand;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitPrefixUnaryExpression(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitPrefixUnaryExpression(this);
        }
    }
}
