using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class ArrayTypeSyntax : TypeSyntax
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
        
        public SyntaxList<ArrayRankSpecifierSyntax> RankSpecifiers { get; private set; }
        
        public ArrayTypeSyntax()
            : base(SyntaxKind.ArrayType)
        {
            RankSpecifiers = new SyntaxList<ArrayRankSpecifierSyntax>(this);
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var child in base.ChildNodes())
            {
                yield return child;
            }
            
            if (ElementType != null)
                yield return ElementType;
            
            foreach (var item in RankSpecifiers)
            {
                yield return item;
            }
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitArrayType(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitArrayType(this);
        }
    }
}
