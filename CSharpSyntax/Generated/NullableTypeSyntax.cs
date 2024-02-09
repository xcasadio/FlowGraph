using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class NullableTypeSyntax : TypeSyntax
    {
        private TypeSyntax _elementType;
        public TypeSyntax ElementType
        {
            get { return _elementType; }
            set
            {
                if (_elementType != null)
                    RemoveChild(_elementType);
                
                _elementType = value;
                
                if (_elementType != null)
                    AddChild(_elementType);
            }
        }
        
        public NullableTypeSyntax()
            : base(SyntaxKind.NullableType)
        {
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var child in base.ChildNodes())
            {
                yield return child;
            }
            
            if (ElementType != null)
                yield return ElementType;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitNullableType(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitNullableType(this);
        }
    }
}
