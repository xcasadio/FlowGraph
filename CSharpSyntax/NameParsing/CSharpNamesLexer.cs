using System;
using System.Collections.Generic;
using System.Text;
using Antlr.Runtime;

namespace CSharpSyntax.NameParsing
{
    partial class CSharpNamesLexer
    {
        public override void ReportError(RecognitionException e)
        {
            throw new CSharpSyntaxException("Cannot parse name", e);
        }

        protected override object RecoverFromMismatchedToken(IIntStream input, int ttype, BitSet follow)
        {
            throw new MismatchedTokenException(ttype, input);
        }

        public override object RecoverFromMismatchedSet(IIntStream input, RecognitionException e, BitSet follow)
        {
            throw e;
        }

        private void ConsumeIdentifierUnicodeStart()
        {
            int c = input.LA(1);

            if (!IsIdentifierStartUnicode(c))
                throw new NoViableAltException();

            MatchAny();

            do
            {
                c = input.LA(1);

                if (
                    (c >= '0' && c <= '9') ||
                    (c >= 'A' && c <= 'Z') ||
                    (c >= 'a' && c <= 'z') ||
                    c == '_' ||
                    IsIdentifierPartUnicode(c))
                    mIDENTIFIER_PART();
                else
                    return;
            }
            while (true);
        }

        private bool IsIdentifierPartUnicode(int c)
        {
            return Char.IsLetterOrDigit((char)c);
        }

        private bool IsIdentifierStartUnicode(int c)
        {
            return Char.IsLetter((char)c);
        }
    }
}
