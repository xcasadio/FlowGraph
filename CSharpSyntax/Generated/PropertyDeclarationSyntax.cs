using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class PropertyDeclarationSyntax : BasePropertyDeclarationSyntax
    {
        public string Identifier { get; set; }
        
        public PropertyDeclarationSyntax()
            : base(SyntaxKind.PropertyDeclaration)
        {
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitPropertyDeclaration(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitPropertyDeclaration(this);
        }
    }
}
