using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSharpSyntax.NameParsing;

namespace CSharpSyntax
{
    public static partial class Syntax
    {
        public static TypeSyntax ParseName(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            return CSharpNamesParser.Parse(name);
        }

        public static SyntaxTrivia Comment(string text)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            return new SyntaxTrivia(SyntaxTriviaKind.Comment, text);
        }

        public static SyntaxTrivia NewLine()
        {
            return new SyntaxTrivia(SyntaxTriviaKind.NewLine, Environment.NewLine);
        }

        public static SyntaxTrivia BlockComment(string text)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            return new SyntaxTrivia(SyntaxTriviaKind.BlockComment, text);
        }

        public static SyntaxTrivia XmlComment(string text)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            return new SyntaxTrivia(SyntaxTriviaKind.XmlComment, text);
        }

        private static IEnumerable<T> ParseNames<T>(IEnumerable<string> names)
            where T : TypeSyntax
        {
            if (names == null)
                throw new ArgumentNullException("names");

            return names.Select(p => (T)ParseName(p));
        }

        public static JoinClauseSyntax JoinClause(string identifier = null, ExpressionSyntax inExpression = null, ExpressionSyntax leftExpression = null, ExpressionSyntax rightExpression = null, JoinIntoClauseSyntax into = null)
        {
            var result = new JoinClauseSyntax();

            result.Identifier = identifier;
            result.InExpression = inExpression;
            result.LeftExpression = leftExpression;
            result.RightExpression = rightExpression;
            result.Into = into;

            return result;
        }
    }
}
