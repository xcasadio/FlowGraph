using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class QueryExpressionSyntax : ExpressionSyntax
    {
        private QueryBodySyntax _body;
        public QueryBodySyntax Body
        {
            get { return _body; }
            set
            {
                if (_body != null)
                    RemoveChild(_body);
                
                _body = value;
                
                if (_body != null)
                    AddChild(_body);
            }
        }
        
        private FromClauseSyntax _fromClause;
        public FromClauseSyntax FromClause
        {
            get { return _fromClause; }
            set
            {
                if (_fromClause != null)
                    RemoveChild(_fromClause);
                
                _fromClause = value;
                
                if (_fromClause != null)
                    AddChild(_fromClause);
            }
        }
        
        public QueryExpressionSyntax()
            : base(SyntaxKind.QueryExpression)
        {
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var child in base.ChildNodes())
            {
                yield return child;
            }
            
            if (Body != null)
                yield return Body;
            
            if (FromClause != null)
                yield return FromClause;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitQueryExpression(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitQueryExpression(this);
        }
    }
}
