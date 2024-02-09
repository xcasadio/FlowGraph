using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSyntax
{
    public abstract partial class BasePropertyDeclarationSyntax : MemberDeclarationSyntax
    {
        private AccessorListSyntax _accessorList;
        public AccessorListSyntax AccessorList
        {
            get { return _accessorList; }
            set
            {
                if (_accessorList != null)
                    RemoveChild(_accessorList);
                
                _accessorList = value;
                
                if (_accessorList != null)
                    AddChild(_accessorList);
            }
        }
        
        public SyntaxList<AttributeListSyntax> AttributeLists { get; private set; }
        
        private ExplicitInterfaceSpecifierSyntax _explicitInterfaceSpecifier;
        public ExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier
        {
            get { return _explicitInterfaceSpecifier; }
            set
            {
                if (_explicitInterfaceSpecifier != null)
                    RemoveChild(_explicitInterfaceSpecifier);
                
                _explicitInterfaceSpecifier = value;
                
                if (_explicitInterfaceSpecifier != null)
                    AddChild(_explicitInterfaceSpecifier);
            }
        }
        
        public Modifiers Modifiers { get; set; }
        
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
        
        internal BasePropertyDeclarationSyntax(SyntaxKind syntaxKind)
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
            
            if (AccessorList != null)
                yield return AccessorList;
            
            foreach (var item in AttributeLists)
            {
                yield return item;
            }
            
            if (ExplicitInterfaceSpecifier != null)
                yield return ExplicitInterfaceSpecifier;
            
            if (Type != null)
                yield return Type;
        }
    }
}
