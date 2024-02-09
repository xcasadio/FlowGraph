using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSyntax
{
    public class SyntaxTrivia
    {
        public SyntaxTriviaKind Kind { get; private set; }

        public string Text { get; private set; }

        internal SyntaxTrivia(SyntaxTriviaKind kind, string text)
        {
            Kind = kind;
            Text = text;
        }

        public SyntaxNode Parent { get; internal set; }
    }
}
