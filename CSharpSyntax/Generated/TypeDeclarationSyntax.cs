using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSyntax
{
    public abstract partial class TypeDeclarationSyntax : BaseTypeDeclarationSyntax
    {
        public SyntaxList<TypeParameterConstraintClauseSyntax> ConstraintClauses { get; private set; }
        
        public SyntaxList<MemberDeclarationSyntax> Members { get; private set; }
        
        private TypeParameterListSyntax _typeParameterList;
        public TypeParameterListSyntax TypeParameterList
        {
            get { return _typeParameterList; }
            set
            {
                if (_typeParameterList != null)
                    RemoveChild(_typeParameterList);
                
                _typeParameterList = value;
                
                if (_typeParameterList != null)
                    AddChild(_typeParameterList);
            }
        }
        
        internal TypeDeclarationSyntax(SyntaxKind syntaxKind)
            : base(syntaxKind)
        {
            ConstraintClauses = new SyntaxList<TypeParameterConstraintClauseSyntax>(this);
            Members = new SyntaxList<MemberDeclarationSyntax>(this);
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var child in base.ChildNodes())
            {
                yield return child;
            }
            
            foreach (var item in ConstraintClauses)
            {
                yield return item;
            }
            
            foreach (var item in Members)
            {
                yield return item;
            }
            
            if (TypeParameterList != null)
                yield return TypeParameterList;
        }
    }
}
