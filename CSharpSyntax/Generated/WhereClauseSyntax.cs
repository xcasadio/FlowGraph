using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class WhereClauseSyntax : QueryClauseSyntax
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
        
        public WhereClauseSyntax()
            : base(SyntaxKind.WhereClause)
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
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitWhereClause(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitWhereClause(this);
        }
    }
}
