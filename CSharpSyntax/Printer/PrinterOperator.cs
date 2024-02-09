﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSyntax.Printer
{
    public enum PrinterOperator
    {
        [EnumText("&")]
        Ampersand,
        [EnumText("&&")]
        AmpersandAmpersand,
        [EnumText("&=")]
        AmpersandEquals,
        [EnumText("*")]
        Asterisk,
        [EnumText("*=")]
        AsteriskEquals,
        [EnumText("|")]
        Bar,
        [EnumText("||")]
        BarBar,
        [EnumText("|=")]
        BarEquals,
        [EnumText("^")]
        Caret,
        [EnumText("^=")]
        CaretEquals,
        [EnumText("=")]
        Equals,
        [EnumText("==")]
        EqualsEquals,
        [EnumText("!=")]
        ExclamationEquals,
        [EnumText(">")]
        GreaterThan,
        [EnumText(">=")]
        GreaterThanEquals,
        [EnumText(">>")]
        GreaterThanGreaterThan,
        [EnumText(">>=")]
        GreaterThanGreaterThanEquals,
        [EnumText("<")]
        LessThan,
        [EnumText("<=")]
        LessThanEquals,
        [EnumText("<<")]
        LessThanLessThan,
        [EnumText("<<=")]
        LessThanLessThanEquals,
        [EnumText("-")]
        Minus,
        [EnumText("-=")]
        MinusEquals,
        [EnumText("%")]
        Percent,
        [EnumText("%=")]
        PercentEquals,
        [EnumText("+")]
        Plus,
        [EnumText("+=")]
        PlusEquals,
        [EnumText("??")]
        QuestionQuestion,
        [EnumText("/")]
        Slash,
        [EnumText("/=")]
        SlashEquals,
        [EnumText("!")]
        Exclamation,
        [EnumText("--")]
        MinusMinus,
        [EnumText("++")]
        PlusPlus,
        [EnumText("~")]
        Tilde
    }
}
