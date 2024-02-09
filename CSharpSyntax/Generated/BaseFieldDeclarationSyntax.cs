using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSyntax
{
    public abstract partial class BaseFieldDeclarationSyntax : MemberDeclarationSyntax
    {
        public SyntaxList<AttributeListSyntax> AttributeLists { get; private set; }
        
        private VariableDeclarationSyntax _declaration;
        public VariableDeclarationSyntax Declaration
        {
            get { return _declaration; }
            set
            {
                if (_declaration != null)
                    RemoveChild(_declaration);
                
                _declaration = value;
                
                if (_declaration != null)
                    AddChild(_declaration);
            }
        }
        
        public Modifiers Modifiers { get; set; }
        
        internal BaseFieldDeclarationSyntax(SyntaxKind syntaxKind)
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
            
            if (Declaration != null)
                yield return Declaration;
        }
    }
}
