using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class EnumMemberDeclarationSyntax : MemberDeclarationSyntax
    {
        public SyntaxList<AttributeListSyntax> AttributeLists { get; private set; }
        
        private EqualsValueClauseSyntax _equalsValue;
        public EqualsValueClauseSyntax EqualsValue
        {
            get { return _equalsValue; }
            set
            {
                if (_equalsValue != null)
                    RemoveChild(_equalsValue);
                
                _equalsValue = value;
                
                if (_equalsValue != null)
                    AddChild(_equalsValue);
            }
        }
        
        public string Identifier { get; set; }
        
        public EnumMemberDeclarationSyntax()
            : base(SyntaxKind.EnumMemberDeclaration)
        {
            AttributeLists = new SyntaxList<AttributeListSyntax>(this);
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var child in base.ChildNodes())
            {
                yield return child;
            }
            
            foreach (var item in AttributeLists)
            {
                yield return item;
            }
            
            if (EqualsValue != null)
                yield return EqualsValue;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitEnumMemberDeclaration(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitEnumMemberDeclaration(this);
        }
    }
}
