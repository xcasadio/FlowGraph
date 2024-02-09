using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class EqualsValueClauseSyntax : SyntaxNode
    {
        private ExpressionSyntax _value;
        public ExpressionSyntax Value
        {
            get { return _value; }
            set
            {
                if (_value != null)
                    RemoveChild(_value);
                
                _value = value;
                
                if (_value != null)
                    AddChild(_value);
            }
        }
        
        public EqualsValueClauseSyntax()
            : base(SyntaxKind.EqualsValueClause)
        {
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            if (Value != null)
                yield return Value;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitEqualsValueClause(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitEqualsValueClause(this);
        }
    }
}
