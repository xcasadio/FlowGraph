using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class DestructorDeclarationSyntax : BaseMethodDeclarationSyntax
    {
        public string Identifier { get; set; }
        
        public DestructorDeclarationSyntax()
            : base(SyntaxKind.DestructorDeclaration)
        {
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitDestructorDeclaration(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitDestructorDeclaration(this);
        }
    }
}
