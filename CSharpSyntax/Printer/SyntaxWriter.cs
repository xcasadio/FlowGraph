using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using CSharpSyntax.Printer.Configuration;

namespace CSharpSyntax.Printer
{
    public partial class SyntaxWriter
    {
        private static readonly EnumTextCache<PrinterKeyword> _keywords = new EnumTextCache<PrinterKeyword>();
        private static readonly EnumTextCache<PrinterOperator> _operators = new EnumTextCache<PrinterOperator>();
        private static readonly EnumTextCache<Syntax> _syntaxes = new EnumTextCache<Syntax>();

        private readonly Stack<int> _indentStack = new Stack<int>();
        private int _indent;
        private readonly Modifiers[] _modifierOrder;
        private readonly Stack<BraceFormattingConfig> _braceFormatting = new Stack<BraceFormattingConfig>();
        private readonly Stack<bool> _singleLineBody = new Stack<bool>();
        private readonly Stack<WrapStyle> _wrapStyle = new Stack<WrapStyle>();
        private readonly BufferedWriter _writer;

        public SyntaxPrinterConfiguration Configuration { get; private set; }

        public SyntaxWriter(TextWriter writer)
            : this(writer, new SyntaxPrinterConfiguration())
        {
        }

        public SyntaxWriter(TextWriter writer, SyntaxPrinterConfiguration configuration)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            _writer = new BufferedWriter(writer, configuration);

            Configuration = configuration;

            _modifierOrder = BuildModifierOrder();
            PushBraceFormatting(configuration.BracesLayout.Other, true);
            PushSingleLineBody(false);
            PushWrapStyle(WrapStyle.SimpleWrap);
        }

        private Modifiers[] BuildModifierOrder()
        {
            var seen = Enum.GetValues(typeof(Modifiers)).Cast<Modifiers>().ToDictionary(p => p, p => false);

            // Remove infra structure modifiers.

            seen.Remove(Modifiers.None);
            seen.Remove(Modifiers.All);

            // The following modifiers are handled special.

            seen.Remove(Modifiers.Const);
            seen.Remove(Modifiers.Partial);

            var result = new List<Modifiers>();

            foreach (var modifier in Configuration.Other.Modifiers.ModifiersOrder)
            {
                var declarationModifier = (Modifiers)Enum.Parse(typeof(Modifiers), modifier.ToString());

                if (!seen[declarationModifier])
                {
                    result.Add(declarationModifier);
                    seen[declarationModifier] = true;
                }
            }

            foreach (Modifiers declarationModifier in Enum.GetValues(typeof(Modifiers)))
            {
                bool modifierSeen;

                if (
                    seen.TryGetValue(declarationModifier, out modifierSeen) &&
                    !modifierSeen
                )
                    result.Add(declarationModifier);
            }

            return result.ToArray();
        }

        internal void WriteIndent()
        {
            if (_singleLineBody.Peek())
            {
                WriteSpace();
                return;
            }

            if (_indent > 0)
                _writer.WriteIndent(_indent);
        }

        internal void EndStatement()
        {
            if (Configuration.Spaces.Other.BeforeSemicolon)
                WriteSpace();

            WriteSyntax(Syntax.Semicolon);

            if (
                !_singleLineBody.Peek() &&
                _braceFormatting.Peek().EndNewline
            )
                WriteLine();
        }

        internal void WriteLine()
        {
            WriteLine(false);
        }

        internal void WriteLine(bool suppressIndentMultiplier)
        {
            WriteLine(1, suppressIndentMultiplier);
        }

        internal void WriteLine(int count)
        {
            WriteLine(count, false);
        }

        internal void WriteLine(int count, bool suppressIndentMultiplier)
        {
            for (int i = 0; i < count; i++)
            {
                _writer.WriteLine(suppressIndentMultiplier);
            }
        }

        internal void WriteSyntax(Syntax syntax)
        {
            WriteSyntax(syntax, WrapStyle.SimpleWrap);
        }

        internal void WriteSyntax(Syntax syntax, WrapStyle wrapStyle)
        {
            bool writeSpace = false;

            if (syntax == Syntax.Dot)
                writeSpace = Configuration.Spaces.Other.AroundDot;

            if (writeSpace)
                WriteSpace();

            if (
                wrapStyle != WrapStyle.SimpleWrap &&
                !Configuration.LineBreaksAndWrapping.LineWrapping.PreferWrapAfterDotInMethodCalls
            )
                Break(wrapStyle == WrapStyle.ChopAlways);

            _writer.Write(FragmentType.Text, _syntaxes[syntax]);

            if (
                wrapStyle != WrapStyle.SimpleWrap &&
                Configuration.LineBreaksAndWrapping.LineWrapping.PreferWrapAfterDotInMethodCalls
            )
                Break(wrapStyle == WrapStyle.ChopAlways);

            if (writeSpace)
                WriteSpace();
        }

        internal void WriteSpace()
        {
            _writer.Write(FragmentType.WhiteSpace, " ");
        }

        internal void WriteListSeparator()
        {
            if (Configuration.Spaces.Other.BeforeComma)
                WriteSpace();

            WriteSyntax(Syntax.Comma);

            switch (_wrapStyle.Peek())
            {
                case WrapStyle.ChopAlways: Break(true); break;
                case WrapStyle.ChopIfLong: Break(false); break;
            }

            if (Configuration.Spaces.Other.AfterComma)
                WriteSpace();
        }

        internal void WriteStatementSeparator(WrapStyle wrapStyle)
        {
            if (Configuration.Spaces.Other.BeforeForSemicolon)
                WriteSpace();

            WriteSyntax(Syntax.Semicolon);

            if (Configuration.Spaces.Other.AfterForSemicolon)
                WriteSpace();

            if (wrapStyle != WrapStyle.SimpleWrap)
                Break(wrapStyle == WrapStyle.ChopAlways);
        }

        internal void WriteKeyword(PrinterKeyword keyword)
        {
            _writer.Write(FragmentType.Text, _keywords[keyword]);
        }

        internal void WriteIdentifier(string identifier)
        {
            _writer.Write(FragmentType.Text, identifier);
        }

        internal void WriteOperator(PrinterOperator @operator)
        {
            _writer.Write(FragmentType.Text, _operators[@operator]);
        }

        internal void WriteLiteral(object value)
        {
            _writer.Write(FragmentType.Text, CodeUtil.Encode(value));
        }

        internal void WriteModifiers(Modifiers modifiers)
        {
            bool hadOne = false;

            foreach (var modifier in _modifierOrder)
            {
                if ((modifiers & modifier) != 0)
                {
                    if (hadOne)
                        WriteSpace();
                    else
                        hadOne = true;

                    switch (modifier)
                    {
                        case Modifiers.Async: WriteKeyword(PrinterKeyword.Async); break;
                        case Modifiers.Abstract: WriteKeyword(PrinterKeyword.Abstract); break;
                        case Modifiers.Extern: WriteKeyword(PrinterKeyword.Extern); break;
                        case Modifiers.Internal: WriteKeyword(PrinterKeyword.Internal); break;
                        case Modifiers.New: WriteKeyword(PrinterKeyword.New); break;
                        case Modifiers.Override: WriteKeyword(PrinterKeyword.Override); break;
                        case Modifiers.Private: WriteKeyword(PrinterKeyword.Private); break;
                        case Modifiers.Protected: WriteKeyword(PrinterKeyword.Protected); break;
                        case Modifiers.Public: WriteKeyword(PrinterKeyword.Public); break;
                        case Modifiers.ReadOnly: WriteKeyword(PrinterKeyword.ReadOnly); break;
                        case Modifiers.Sealed: WriteKeyword(PrinterKeyword.Sealed); break;
                        case Modifiers.Static: WriteKeyword(PrinterKeyword.Static); break;
                        case Modifiers.Virtual: WriteKeyword(PrinterKeyword.Virtual); break;
                        case Modifiers.Volatile: WriteKeyword(PrinterKeyword.Volatile); break;
                        default: Debug.Fail("Unexpected modifier"); break;
                    }
                }
            }
        }

        internal void WriteComment(string text)
        {
            _writer.Write(FragmentType.Text, text);
        }

        internal void BeginBlock()
        {
            BeginBlock(true, true);
        }

        internal void BeginBlock(bool newLineBefore, bool newLineAfter)
        {
            BeginBlock(newLineBefore, newLineAfter, false);
        }

        internal void BeginBlock(bool newLineBefore, bool newLineAfter, bool suppressIndent)
        {
            if (_singleLineBody.Peek())
            {
                WriteSpace();
                WriteSyntax(Syntax.OpenBrace);
                return;
            }

            switch (_braceFormatting.Peek().BraceFormatting)
            {
                case BraceFormatting.EndOfLine:
                    WriteSyntax(Syntax.OpenBrace);
                    if (newLineAfter)
                        WriteLine();
                    if (!suppressIndent)
                        PushIndent();
                    break;

                case BraceFormatting.EndOfLineKr:
                    WriteSpace();
                    WriteSyntax(Syntax.OpenBrace);
                    if (newLineAfter)
                        WriteLine();
                    if (!suppressIndent)
                        PushIndent();
                    break;

                case BraceFormatting.NextLine:
                    if (newLineBefore)
                        WriteLine();
                    WriteIndent();
                    WriteSyntax(Syntax.OpenBrace);
                    if (newLineAfter)
                        WriteLine();
                    if (!suppressIndent)
                        PushIndent();
                    break;

                case BraceFormatting.NextLineIndented:
                    if (newLineBefore)
                        WriteLine();
                    if (!suppressIndent)
                        PushIndent();
                    WriteIndent();
                    WriteSyntax(Syntax.OpenBrace);
                    if (newLineAfter)
                        WriteLine();
                    break;

                case BraceFormatting.NextLineIndented2:
                    if (newLineBefore)
                        WriteLine();
                    PushIndent();
                    WriteIndent();
                    WriteSyntax(Syntax.OpenBrace);
                    if (newLineAfter)
                        WriteLine();
                    if (!suppressIndent)
                        PushIndent();
                    break;
            }
        }

        internal void EndBlock()
        {
            EndBlock(true);
        }

        internal void EndBlock(bool writeIndent)
        {
            EndBlock(writeIndent, false);
        }

        internal void EndBlock(bool writeIndent, bool suppressIndent)
        {
            if (_singleLineBody.Peek())
            {
                WriteSpace();
                WriteSyntax(Syntax.CloseBrace);
                return;
            }

            bool endNewline = _braceFormatting.Peek().EndNewline;

            switch (_braceFormatting.Peek().BraceFormatting)
            {
                case BraceFormatting.EndOfLine:
                case BraceFormatting.EndOfLineKr:
                case BraceFormatting.NextLine:
                    if (!suppressIndent)
                        PopIndent();
                    if (writeIndent)
                        WriteIndent();
                    WriteSyntax(Syntax.CloseBrace);
                    if (endNewline)
                        WriteLine();
                    break;

                case BraceFormatting.NextLineIndented:
                    if (writeIndent)
                        WriteIndent();
                    WriteSyntax(Syntax.CloseBrace);
                    if (endNewline)
                        WriteLine();
                    if (!suppressIndent)
                        PopIndent();
                    break;

                case BraceFormatting.NextLineIndented2:
                    if (!suppressIndent)
                        PopIndent();
                    if (writeIndent)
                        WriteIndent();
                    WriteSyntax(Syntax.CloseBrace);
                    if (endNewline)
                        WriteLine();
                    PopIndent();
                    break;
            }
        }

        internal void PushIndent()
        {
            PushIndent(Configuration.Indentation);
        }

        internal void PushIndent(int indent)
        {
            if (indent < 0)
                indent = Math.Min(-_indent, indent);

            _indentStack.Push(indent);
            _indent += indent;
        }

        internal void PopIndent()
        {
            _indent -= _indentStack.Pop();
        }

        internal void PushBraceFormatting(BraceFormatting braceFormatting)
        {
            PushBraceFormatting(braceFormatting, true);
        }

        internal void PushBraceFormatting(BraceFormatting braceFormatting, bool endNewline)
        {
            _braceFormatting.Push(new BraceFormattingConfig(braceFormatting, endNewline));
        }

        internal void PopBraceFormatting()
        {
            _braceFormatting.Pop();
        }

        internal void EmptyBlock()
        {
            EmptyBlock(0, true);
        }

        internal void EmptyBlock(bool writeNewline)
        {
            EmptyBlock(0, writeNewline);
        }

        internal void EmptyBlock(int blankLines)
        {
            EmptyBlock(blankLines, true);
        }

        internal void EmptyBlock(int blankLines, bool newLineBefore)
        {
            switch (Configuration.BracesLayout.EmptyBraceFormatting)
            {
                case EmptyBraceFormatting.OnDifferentLines:
                    BeginBlock(newLineBefore, true);
                    WriteLine(blankLines);
                    EndBlock();
                    break;

                case EmptyBraceFormatting.PlaceTogether:
                    BeginBlock(true, false);

                    if (Configuration.Spaces.Other.SpacesBetweenEmptyBraces)
                        WriteSpace();

                    EndBlock(false);
                    break;

                case EmptyBraceFormatting.TogetherOnSameLine:
                    WriteSpace();
                    WriteSyntax(Syntax.OpenBrace);
                    
                    if (Configuration.Spaces.Other.SpacesBetweenEmptyBraces)
                        WriteSpace();

                    WriteSyntax(Syntax.CloseBrace);
                    if (_braceFormatting.Peek().EndNewline)
                        WriteLine();
                    break;
            }
        }

        internal void PushSingleLineBody(bool singleLine)
        {
            _singleLineBody.Push(singleLine);
        }

        internal void PopSingleLineBody()
        {
            _singleLineBody.Pop();
        }

        internal void Break(bool force)
        {
            _writer.Write(force ? FragmentType.ForcedBreak : FragmentType.Break);
        }

        internal void SetAlignmentBreak(bool set)
        {
            _writer.Write(set ? FragmentType.AlignmentStart : FragmentType.AlignmentEnd);
        }

        internal void Flush()
        {
            _writer.Flush();

            Debug.Assert(_braceFormatting.Count == 1);
            Debug.Assert(_singleLineBody.Count == 1);
            Debug.Assert(_wrapStyle.Count == 1);
        }

        internal void PushWrapStyle(WrapStyle wrapStyle)
        {
            _wrapStyle.Push(wrapStyle);
        }

        internal void PopWrapStyle()
        {
            _wrapStyle.Pop();
        }

        private struct BraceFormattingConfig
        {
            private readonly BraceFormatting _braceFormatting;
            private readonly bool _endNewline;

            public BraceFormatting BraceFormatting
            {
                get { return _braceFormatting; }
            }

            public bool EndNewline
            {
                get { return _endNewline; }
            }

            public BraceFormattingConfig(BraceFormatting braceFormatting, bool endNewline)
            {
                _braceFormatting = braceFormatting;
                _endNewline = endNewline;
            }
        }
    }
}
