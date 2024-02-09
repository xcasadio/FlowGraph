using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSyntax.Printer
{
    public enum Syntax
    {
        [EnumText(";")]
        Semicolon,
        [EnumText(",")]
        Comma,
        [EnumText("{")]
        OpenBrace,
        [EnumText("}")]
        CloseBrace,
        [EnumText("<")]
        LessThan,
        [EnumText(">")]
        GreaterThan,
        [EnumText(".")]
        Dot,
        [EnumText("::")]
        ColonColon,
        [EnumText("[")]
        OpenBracket,
        [EnumText("]")]
        CloseBracket,
        [EnumText("?")]
        Question,
        [EnumText("=>")]
        Arrow,
        [EnumText("(")]
        OpenParen,
        [EnumText(")")]
        CloseParen,
        [EnumText(":")]
        Colon,
        [EnumText("~")]
        Tilde,
    }
}
