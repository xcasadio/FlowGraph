using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class EventFieldDeclarationSyntax : BaseFieldDeclarationSyntax
    {
        public EventFieldDeclarationSyntax()
            : base(SyntaxKind.EventFieldDeclaration)
        {
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitEventFieldDeclaration(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitEventFieldDeclaration(this);
        }
    }
}
