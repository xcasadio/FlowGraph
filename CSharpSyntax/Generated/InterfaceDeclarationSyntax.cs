using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class InterfaceDeclarationSyntax : TypeDeclarationSyntax
    {
        public InterfaceDeclarationSyntax()
            : base(SyntaxKind.InterfaceDeclaration)
        {
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitInterfaceDeclaration(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitInterfaceDeclaration(this);
        }
    }
}
