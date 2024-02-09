using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class SimpleLambdaExpressionSyntax : ExpressionSyntax
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
        
        private ParameterSyntax _parameter;
        public ParameterSyntax Parameter
        {
            get { return _parameter; }
            set
            {
                if (_parameter != null)
                    RemoveChild(_parameter);
                
                _parameter = value;
                
                if (_parameter != null)
                    AddChild(_parameter);
            }
        }
        
        public SimpleLambdaExpressionSyntax()
            : base(SyntaxKind.SimpleLambdaExpression)
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
            
            if (Parameter != null)
                yield return Parameter;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitSimpleLambdaExpression(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitSimpleLambdaExpression(this);
        }
    }
}
