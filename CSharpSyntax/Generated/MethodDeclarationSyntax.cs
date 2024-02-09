using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class MethodDeclarationSyntax : BaseMethodDeclarationSyntax
    {
        public SyntaxList<TypeParameterConstraintClauseSyntax> ConstraintClauses { get; private set; }
        
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
        
        public string Identifier { get; set; }
        
        private TypeSyntax _returnType;
        public TypeSyntax ReturnType
        {
            get { return _returnType; }
            set
            {
                if (_returnType != null)
                    RemoveChild(_returnType);
                
                _returnType = value;
                
                if (_returnType != null)
                    AddChild(_returnType);
            }
        }
        
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
        
        public MethodDeclarationSyntax()
            : base(SyntaxKind.MethodDeclaration)
        {
            ConstraintClauses = new SyntaxList<TypeParameterConstraintClauseSyntax>(this);
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
            
            if (ExplicitInterfaceSpecifier != null)
                yield return ExplicitInterfaceSpecifier;
            
            if (ReturnType != null)
                yield return ReturnType;
            
            if (TypeParameterList != null)
                yield return TypeParameterList;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitMethodDeclaration(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitMethodDeclaration(this);
        }
    }
}
