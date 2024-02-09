using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class ExternAliasDirectiveSyntax : SyntaxTriviaNode
    {
        public string Identifier { get; set; }
        
        public ExternAliasDirectiveSyntax()
            : base(SyntaxKind.ExternAliasDirective)
        {
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitExternAliasDirective(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitExternAliasDirective(this);
        }
    }
}
