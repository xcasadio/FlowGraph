using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSyntax
{
    public abstract class SyntaxTriviaNode : SyntaxNode
    {
        public SyntaxTriviaList LeadingTrivia { get; private set; }

        public SyntaxTriviaList TrailingTrivia { get; private set; }

        protected SyntaxTriviaNode(SyntaxKind syntaxKind)
            : base(syntaxKind)
        {
            LeadingTrivia = new SyntaxTriviaList(this);
            TrailingTrivia = new SyntaxTriviaList(this);
        }
    }
}
