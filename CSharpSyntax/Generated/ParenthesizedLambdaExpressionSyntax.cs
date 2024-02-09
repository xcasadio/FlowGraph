using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class ParenthesizedLambdaExpressionSyntax : ExpressionSyntax
    {
        private SyntaxNode _body;
        public SyntaxNode Body
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
        
        public Modifiers Modifiers { get; set; }
        
        private ParameterListSyntax _parameterList;
        public ParameterListSyntax ParameterList
        {
            get { return _parameterList; }
            set
            {
                if (_parameterList != null)
                    RemoveChild(_parameterList);
                
                _parameterList = value;
                
                if (_parameterList != null)
                    AddChild(_parameterList);
            }
        }
        
        public ParenthesizedLambdaExpressionSyntax()
            : base(SyntaxKind.ParenthesizedLambdaExpression)
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
            
            if (ParameterList != null)
                yield return ParameterList;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitParenthesizedLambdaExpression(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitParenthesizedLambdaExpression(this);
        }
    }
}
