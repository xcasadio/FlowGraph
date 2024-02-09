using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class GroupClauseSyntax : SelectOrGroupClauseSyntax
    {
        private ExpressionSyntax _byExpression;
        public ExpressionSyntax ByExpression
        {
            get { return _byExpression; }
            set
            {
                if (_byExpression != null)
                    RemoveChild(_byExpression);
                
                _byExpression = value;
                
                if (_byExpression != null)
                    AddChild(_byExpression);
            }
        }
        
        private ExpressionSyntax _groupExpression;
        public ExpressionSyntax GroupExpression
        {
            get { return _groupExpression; }
            set
            {
                if (_groupExpression != null)
                    RemoveChild(_groupExpression);
                
                _groupExpression = value;
                
                if (_groupExpression != null)
                    AddChild(_groupExpression);
            }
        }
        
        public GroupClauseSyntax()
            : base(SyntaxKind.GroupClause)
        {
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var child in base.ChildNodes())
            {
                yield return child;
            }
            
            if (ByExpression != null)
                yield return ByExpression;
            
            if (GroupExpression != null)
                yield return GroupExpression;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitGroupClause(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitGroupClause(this);
        }
    }
}
