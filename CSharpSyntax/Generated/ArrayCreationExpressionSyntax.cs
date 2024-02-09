using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class ArrayCreationExpressionSyntax : ExpressionSyntax
    {
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
        
        private ArrayTypeSyntax _type;
        public ArrayTypeSyntax Type
        {
            get { return _type; }
            set
            {
                if (_type != null)
                    RemoveChild(_type);
                
                _type = value;
                
                if (_type != null)
                    AddChild(_type);
            }
        }
        
        public ArrayCreationExpressionSyntax()
            : base(SyntaxKind.ArrayCreationExpression)
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
            
            if (Type != null)
                yield return Type;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitArrayCreationExpression(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitArrayCreationExpression(this);
        }
    }
}
