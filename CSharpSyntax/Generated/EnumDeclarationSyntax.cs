using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class EnumDeclarationSyntax : BaseTypeDeclarationSyntax
    {
        public SyntaxList<EnumMemberDeclarationSyntax> Members { get; private set; }
        
        public EnumDeclarationSyntax()
            : base(SyntaxKind.EnumDeclaration)
        {
            Members = new SyntaxList<EnumMemberDeclarationSyntax>(this);
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var child in base.ChildNodes())
            {
                yield return child;
            }
            
            foreach (var item in Members)
            {
                yield return item;
            }
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitEnumDeclaration(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitEnumDeclaration(this);
        }
    }
}
