using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class ImplicitArrayCreationExpressionSyntax : ExpressionSyntax
    {
        public int? Commas { get; set; }
        
        private InitializerExpressionSyntax _initializer;
        public InitializerExpressionSyntax Initializer
        {
            get { return _initializer; }
            set
            {
                if (_initializer != null)
                    RemoveChild(_initializer);
                
                _initializer = value;
                
                if (_initializer != null)
                    AddChild(_initializer);
            }
        }
        
        public ImplicitArrayCreationExpressionSyntax()
            : base(SyntaxKind.ImplicitArrayCreationExpression)
        {
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var child in base.ChildNodes())
            {
                yield return child;
            }
            
            if (Initializer != null)
                yield return Initializer;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitImplicitArrayCreationExpression(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitImplicitArrayCreationExpression(this);
        }
    }
}
