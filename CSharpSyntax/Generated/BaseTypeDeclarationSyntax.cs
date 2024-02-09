using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSyntax
{
    public abstract partial class BaseTypeDeclarationSyntax : MemberDeclarationSyntax
    {
        public SyntaxList<AttributeListSyntax> AttributeLists { get; private set; }
        
        private BaseListSyntax _baseList;
        public BaseListSyntax BaseList
        {
            get { return _baseList; }
            set
            {
                if (_baseList != null)
                    RemoveChild(_baseList);
                
                _baseList = value;
                
                if (_baseList != null)
                    AddChild(_baseList);
            }
        }
        
        public string Identifier { get; set; }
        
        public Modifiers Modifiers { get; set; }
        
        internal BaseTypeDeclarationSyntax(SyntaxKind syntaxKind)
            : base(syntaxKind)
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
            
            if (BaseList != null)
                yield return BaseList;
        }
    }
}
