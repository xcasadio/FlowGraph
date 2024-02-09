using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class NamespaceDeclarationSyntax : MemberDeclarationSyntax
    {
        public SyntaxList<ExternAliasDirectiveSyntax> Externs { get; private set; }
        
        public SyntaxList<MemberDeclarationSyntax> Members { get; private set; }
        
        private NameSyntax _name;
        public NameSyntax Name
        {
            get { return _name; }
            set
            {
                if (_name != null)
                    RemoveChild(_name);
                
                _name = value;
                
                if (_name != null)
                    AddChild(_name);
            }
        }
        
        public SyntaxList<UsingDirectiveSyntax> Usings { get; private set; }
        
        public NamespaceDeclarationSyntax()
            : base(SyntaxKind.NamespaceDeclaration)
        {
            Externs = new SyntaxList<ExternAliasDirectiveSyntax>(this);
            Members = new SyntaxList<MemberDeclarationSyntax>(this);
            Usings = new SyntaxList<UsingDirectiveSyntax>(this);
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var child in base.ChildNodes())
            {
                yield return child;
            }
            
            foreach (var item in Externs)
            {
                yield return item;
            }
            
            foreach (var item in Members)
            {
                yield return item;
            }
            
            if (Name != null)
                yield return Name;
            
            foreach (var item in Usings)
            {
                yield return item;
            }
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitNamespaceDeclaration(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitNamespaceDeclaration(this);
        }
    }
}
