using System;
using System.Collections.Generic;
using System.Text;
using Antlr.Runtime;

namespace CSharpSyntax.NameParsing
{
    partial class CSharpNamesParser
    {
        public static TypeSyntax Parse(string name)
        {
            try
            {
                var inputStream = new ANTLRStringStream(name);

                var lexer = new CSharpNamesLexer(inputStream);
                var tokenStream = new CommonTokenStream(lexer);
                var parser = new CSharpNamesParser(tokenStream);

                return parser.prog().value;
            }
            catch (Exception ex)
            {
                throw new CSharpSyntaxException("Cannot parse name", ex);
            }
        }

        public override object RecoverFromMismatchedSet(IIntStream input, RecognitionException e, BitSet follow)
        {
            throw e;
        }

        protected override object RecoverFromMismatchedToken(IIntStream input, int ttype, BitSet follow)
        {
            throw new MismatchedTokenException(ttype, input);
        }

        public override void ReportError(RecognitionException ex)
        {
            throw new CSharpSyntaxException("Cannot parse name", ex);
        }

        private NameSyntax BuildAliasQualifiedName(IdentifierNameSyntax left, NameSyntax right)
        {
            var result = new AliasQualifiedNameSyntax
            {
                Alias = left
            };

            return InsertLeft(result, right, (r, i) => r.Name = i);
        }

        private NameSyntax BuildQualifiedName(IdentifierNameSyntax left, NameSyntax right)
        {
            var result = new QualifiedNameSyntax
            {
                Left = left
            };

            return InsertLeft(result, right, (r, i) => r.Right = i);
        }

        private NameSyntax InsertLeft<T>(T result, NameSyntax right, Action<T, SimpleNameSyntax> callback)
            where T : NameSyntax
        {
            var fixup = right as QualifiedNameSyntax;

            if (fixup != null)
            {
                while (fixup.Left is QualifiedNameSyntax)
                {
                    fixup = (QualifiedNameSyntax)fixup.Left;
                }

                var qualified = fixup.Left;
                fixup.Left = result;
                callback(result, (SimpleNameSyntax)qualified);

                return right;
            }
            else
            {
                callback(result, (SimpleNameSyntax)right);

                return result;
            }
        }

        private TypeSyntax FixupTree<T>(TypeSyntax tree, T result, Action<T, IdentifierNameSyntax> callback)
            where T : SimpleNameSyntax
        {
            if (tree is QualifiedNameSyntax)
            {
                var qualified = (QualifiedNameSyntax)tree;
                var identifier = qualified.Right;
                qualified.Right = result;
                callback(result, (IdentifierNameSyntax)identifier);
                return qualified;
            }
            else if (tree is AliasQualifiedNameSyntax)
            {
                var qualified = (AliasQualifiedNameSyntax)tree;
                var identifier = qualified.Name;
                qualified.Name = result;
                callback(result, (IdentifierNameSyntax)identifier);
                return qualified;
            }
            else
            {
                callback(result, (IdentifierNameSyntax)tree);
                return result;
            }
        }

        private ArrayRankSpecifierSyntax BuildArrayRankSpecifier(int rank)
        {
            var result = new ArrayRankSpecifierSyntax();

            for (int i = 0; i < rank; i++)
            {
                result.Sizes.Add(new OmittedArraySizeExpressionSyntax());
            }

            return result;
        }
    }
}
