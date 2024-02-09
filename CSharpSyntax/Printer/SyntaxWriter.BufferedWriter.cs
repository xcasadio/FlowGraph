using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CSharpSyntax.Printer.Configuration;

namespace CSharpSyntax.Printer
{
    partial class SyntaxWriter
    {
        private class BufferedWriter
        {
            private readonly TextWriter _writer;
            private readonly SyntaxPrinterConfiguration _configuration;
            private readonly int _rightMargin;
            private readonly List<Fragment> _fragments = new List<Fragment>();
            private int _indent;

            public BufferedWriter(TextWriter writer, SyntaxPrinterConfiguration configuration)
            {
                if (writer == null)
                    throw new ArgumentNullException("writer");

                _writer = writer;
                _configuration = configuration;

                _rightMargin =
                    configuration.LineBreaksAndWrapping.LineWrapping.WrapLongLines
                    ? configuration.LineBreaksAndWrapping.LineWrapping.RightMargin
                    : int.MaxValue;
            }

            public void Write(FragmentType type)
            {
                Write(type, null);
            }

            public void Write(FragmentType type, string text)
            {
                _fragments.Add(new Fragment(text, type));
            }

            public void WriteLine(bool suppressIndentMultiplier)
            {
                Flush(suppressIndentMultiplier);

                _writer.WriteLine();
            }

            public void Flush()
            {
                Flush(false);
            }

            private void Flush(bool suppressIndentMultiplier)
            {
                int offset = 0;
                int length = _indent;
                int lastBreak = -1;
                bool hadOneText = true; // Only applies after a line break.
                bool hadAlignment = false; // Optimization

                for (int i = offset; i < _fragments.Count; i++)
                {
                    var fragment = _fragments[i];
                    bool performFlush = false;

                    switch (fragment.Type)
                    {
                        case FragmentType.ForcedBreak:
                            performFlush = true;
                            break;

                        case FragmentType.Break:
                            lastBreak = i;
                            break;

                        case FragmentType.AlignmentStart:
                            _fragments[i] = _fragments[i].WithIndent(length);
                            hadAlignment = true;
                            break;

                        case FragmentType.AlignmentEnd:
                            break;

                        case FragmentType.WhiteSpace:
                        case FragmentType.Text:
                            // Skip over leading whitespaces after a break.

                            if (fragment.Type == FragmentType.Text)
                            {
                                hadOneText = true;
                            }
                            else if (!hadOneText)
                            {
                                offset = i + 1;
                                break;
                            }

                            length += fragment.Text.Length;

                            if (length > _rightMargin && i > offset + 1)
                            {
                                if (lastBreak != -1)
                                {
                                    i = lastBreak;
                                }
                                else if (i > offset)
                                {
                                    i--;

                                    while (
                                        i >= offset &&
                                        _fragments[i].Type != FragmentType.Text
                                    ) {
                                        i--;
                                    }
                                }

                                performFlush = true;
                                    
                            }
                            break;

                        default: throw ThrowHelper.InvalidEnumValue(fragment.Type);
                    }

                    if (performFlush || i == _fragments.Count - 1)
                    {
                        PerformFlush(offset, i, suppressIndentMultiplier);

                        if (i < _fragments.Count - 1)
                        {
                            _writer.WriteLine();

                            if (hadAlignment)
                            {
                                int indent = FindIndent(i);
                                if (indent != -1)
                                    _indent = indent;
                            }
                        }

                        offset = i + 1;
                        length = _indent;
                        lastBreak = -1;
                        hadOneText = false;
                    }
                }

                _fragments.Clear();

                _indent = 0;
            }

            private int FindIndent(int end)
            {
                int stack = 0;

                for (int i = end; i >= 0; i--)
                {
                    var fragment = _fragments[i];

                    switch (fragment.Type)
                    {
                        case FragmentType.AlignmentEnd:
                            stack++;
                            break;

                        case FragmentType.AlignmentStart:
                            if (stack == 0)
                                return fragment.Indent;
                            stack--;
                            break;
                    }
                }

                return -1;
            }

            private void PerformFlush(int start, int end, bool suppressIndentMultiplier)
            {
                WriteIndent();

                if (start == 0 && !suppressIndentMultiplier)
                {
                    _indent +=
                        _configuration.Other.Indentation.ContinuousLineIndentMultiplier *
                        _configuration.Indentation;
                }

                while (end > start && _fragments[end].Type != FragmentType.Text)
                {
                    end--;
                }

                for (int j = start; j <= end; j++)
                {
                    var fragment = _fragments[j];

                    if (fragment.Text != null)
                        _writer.Write(fragment.Text);
                }
            }

            public void WriteIndent(int indent)
            {
                _indent = indent;
            }

            private void WriteIndent()
            {
                if (_indent == 0)
                    return;

                if (_configuration.IndentationStyle == IndentationStyle.Spaces)
                {
                    _writer.Write(new string(' ', _indent));
                }
                else
                {
                    int tabs = _indent / _configuration.Indentation;
                    int spaces = _indent % _configuration.Indentation;

                    if (tabs > 0)
                        _writer.Write(new string('\t', tabs));
                    if (spaces > 0)
                        _writer.Write(new string(' ', spaces));
                }
            }

            private struct Fragment
            {
                private readonly string _text;
                private readonly FragmentType _type;
                private readonly int _indent;

                public string Text
                {
                    get { return _text; }
                }

                public FragmentType Type
                {
                    get { return _type; }
                }

                public int Indent
                {
                    get { return _indent; }
                }

                public Fragment(string text, FragmentType type)
                    : this(text, type, -1)
                {
                }

                private Fragment(string text, FragmentType type, int indent)
                {
                    _text = text;
                    _type = type;
                    _indent = indent;
                }

                public override string ToString()
                {
                    if (_text == null)
                        return _type.ToString();
                    else
                        return _text + " [" + _type + "]";
                }

                public Fragment WithIndent(int indent)
                {
                    return new Fragment(_text, _type, indent);
                }
            }
        }

        private enum FragmentType
        {
            WhiteSpace,
            Text,
            Break,
            ForcedBreak,
            AlignmentStart,
            AlignmentEnd
        }
    }
}
