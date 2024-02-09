using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class DelegateDeclarationSyntax : MemberDeclarationSyntax
    {
        public SyntaxList<AttributeListSyntax> AttributeLists { get; private set; }
        
        public SyntaxList<TypeParameterConstraintClauseSyntax> ConstraintClauses { get; private set; }
        
        public string Identifier { get; set; }
        
        public Modifiers Modifiers { get; set; }
        
        private ParameterListSyntax _parameterList;
        public ParameterListSyntax ParameterList
        {
            get { return _parameterList; }
            set
            {
                if (_parameterList != null)
                    RemoveChild(_parameterList);
                
                _parameterList = value;
                
                if (_parameterList != null)
                    AddChild(_parameterList);
            }
        }
        
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
        
        public DelegateDeclarationSyntax()
            : base(SyntaxKind.DelegateDeclaration)
        {
            AttributeLists = new SyntaxList<AttributeListSyntax>(this);
            ConstraintClauses = new SyntaxList<TypeParameterConstraintClauseSyntax>(this);
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
            
            foreach (var item in ConstraintClauses)
            {
                yield return item;
            }
            
            if (ParameterList != null)
                yield return ParameterList;
            
            if (ReturnType != null)
                yield return ReturnType;
            
            if (TypeParameterList != null)
                yield return TypeParameterList;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitDelegateDeclaration(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitDelegateDeclaration(this);
        }
    }
}
