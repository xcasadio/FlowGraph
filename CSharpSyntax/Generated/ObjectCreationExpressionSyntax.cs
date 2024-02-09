using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class ObjectCreationExpressionSyntax : ExpressionSyntax
    {
        private ArgumentListSyntax _argumentList;
        public ArgumentListSyntax ArgumentList
        {
            get { return _argumentList; }
            set
            {
                if (_argumentList != null)
                    RemoveChild(_argumentList);
                
                _argumentList = value;
                
                if (_argumentList != null)
                    AddChild(_argumentList);
            }
        }
        
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
        
        private TypeSyntax _type;
        public TypeSyntax Type
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
        
        public ObjectCreationExpressionSyntax()
            : base(SyntaxKind.ObjectCreationExpression)
        {
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var child in base.ChildNodes())
            {
                yield return child;
            }
            
            if (ArgumentList != null)
                yield return ArgumentList;
            
            if (Initializer != null)
                yield return Initializer;
            
            if (Type != null)
                yield return Type;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitObjectCreationExpression(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitObjectCreationExpression(this);
        }
    }
}
