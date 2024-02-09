using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSyntax.Printer
{
    public partial class SyntaxPrinter : ISyntaxVisitor, IDisposable
    {
        private const Modifiers AllAccessModifiers = Modifiers.Private | Modifiers.Protected | Modifiers.Public | Modifiers.Internal;

        private SyntaxWriter _writer;
        private bool _disposed;
        private bool _wrapCompound;

        public bool Done
        {
            get { return false; }
        }

        public SyntaxPrinter(SyntaxWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            _writer = writer;
        }

        public void Visit(SyntaxNode node)
        {
            if (node != null)
                node.Accept(this);
        }

        public void VisitAccessorDeclaration(AccessorDeclarationSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            var accessorList = node.Parent as AccessorListSyntax;

            bool isSimple =
                _writer.Configuration.LineBreaksAndWrapping.Other.PlaceAbstractAutoPropertyIndexerEventDeclarationOnSingleLine &&
                accessorList != null &&
                accessorList.Accessors.All(p =>
                    (!HasExtras(p) || _writer.Configuration.LineBreaksAndWrapping.Other.PlaceSingleLineAccessorAttributeOnSameLine) &&
                    p.Body == null
                );

            if (!isSimple)
            {
                isSimple =
                    _writer.Configuration.LineBreaksAndWrapping.Other.PlaceSimplePropertyIndexerEventDeclarationOnSingleLine &&
                    accessorList != null &&
                    accessorList.Accessors.All(p =>
                        (!HasExtras(p) || _writer.Configuration.LineBreaksAndWrapping.Other.PlaceSingleLineAccessorAttributeOnSameLine) &&
                        IsSimpleBody(p.Body)
                    );
            }

            if (!isSimple)
                _writer.WriteIndent();

            bool isBodySimple =
                node.Body == null ||
                IsSimpleBody(node.Body);

            WriteAttributes(
                node,
                _writer.Configuration.LineBreaksAndWrapping.Other.PlaceMethodAttributeOnSameLine ||
                (
                    isBodySimple
                    ? _writer.Configuration.LineBreaksAndWrapping.Other.PlaceSingleLineAccessorAttributeOnSameLine
                    : _writer.Configuration.LineBreaksAndWrapping.Other.PlaceMultiLineAccessorAttributeOnSameLine
                )
            );

            if (node.Modifiers != Modifiers.None)
            {
                _writer.WriteModifiers(node.Modifiers);
                _writer.WriteSpace();
            }

            switch (node.Kind)
            {
                case AccessorDeclarationKind.Add: _writer.WriteKeyword(PrinterKeyword.Add); break;
                case AccessorDeclarationKind.Get: _writer.WriteKeyword(PrinterKeyword.Get); break;
                case AccessorDeclarationKind.Remove: _writer.WriteKeyword(PrinterKeyword.Remove); break;
                case AccessorDeclarationKind.Set: _writer.WriteKeyword(PrinterKeyword.Set); break;
                default: throw new InvalidOperationException("Invalid enum value");
            }

            if (node.Body != null)
            {
                if (
                    isSimple || (
                        _writer.Configuration.LineBreaksAndWrapping.Other.PlaceSimpleAccessorOnSingleLine &&
                        isBodySimple
                    )
                )
                {
                    _writer.PushSingleLineBody(true);

                    node.Body.Accept(this);

                    if (!isSimple)
                        _writer.WriteLine();

                    _writer.PopSingleLineBody();
                }
                else
                {
                    node.Body.Accept(this);
                }
            }
            else
            {
                _writer.WriteSyntax(Syntax.Semicolon);

                if (!isSimple)
                    _writer.WriteLine();
            }

            WriteTrailingTrivia(node);
        }

        private bool HasExtras(SyntaxNode node)
        {
            var hasAttributeLists = node as IHasAttributeLists;

            if (hasAttributeLists != null && hasAttributeLists.AttributeLists.Count > 0)
                return true;

            var triviaNode = node as SyntaxTriviaNode;

            if (
                triviaNode != null &&
                (triviaNode.LeadingTrivia.Count > 0 || triviaNode.TrailingTrivia.Count > 0)
            )
                return true;

            return false;
        }

        private bool IsSimpleBody(BlockSyntax blockSyntax)
        {
            return
                blockSyntax.LeadingTrivia.Count == 0 &&
                blockSyntax.TrailingTrivia.Count == 0 && (
                    blockSyntax.Statements.Count == 0 || (
                        blockSyntax.Statements.Count == 1 &&
                        !HasExtras(blockSyntax.Statements[0]) &&
                        !(blockSyntax.Statements[0] is BlockSyntax)
                    )
                );
        }

        public void VisitAccessorList(AccessorListSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            bool isSimple =
                _writer.Configuration.LineBreaksAndWrapping.Other.PlaceAbstractAutoPropertyIndexerEventDeclarationOnSingleLine &&
                node.Accessors.All(p =>
                    (!HasExtras(p) || _writer.Configuration.LineBreaksAndWrapping.Other.PlaceSingleLineAccessorAttributeOnSameLine) &&
                    p.Body == null
                );

            if (!isSimple)
            {
                isSimple =
                    _writer.Configuration.LineBreaksAndWrapping.Other.PlaceSimplePropertyIndexerEventDeclarationOnSingleLine &&
                    node.Accessors.All(p =>
                        (!HasExtras(p) || _writer.Configuration.LineBreaksAndWrapping.Other.PlaceSingleLineAccessorAttributeOnSameLine) &&
                        IsSimpleBody(p.Body)
                    );
            }

            if (isSimple)
            {
                _writer.WriteSpace();
                _writer.WriteSyntax(Syntax.OpenBrace);
                _writer.WriteSpace();
            }
            else
            {
                _writer.BeginBlock();
            }

            bool hadOne = false;

            foreach (var accessor in node.Accessors)
            {
                if (hadOne && isSimple)
                    _writer.WriteSpace();
                else
                    hadOne = true;

                accessor.Accept(this);
            }

            if (isSimple)
            {
                _writer.WriteSpace();
                _writer.WriteSyntax(Syntax.CloseBrace);
                _writer.WriteLine();
            }
            else
            {
                _writer.EndBlock();
            }
        }

        public void VisitAliasQualifiedName(AliasQualifiedNameSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            ExpressionStart(node);

            node.Alias.Accept(this);
            _writer.WriteSyntax(Syntax.ColonColon);
            node.Name.Accept(this);

            ExpressionEnd(node);
        }

        public void VisitAnonymousMethodExpression(AnonymousMethodExpressionSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            ExpressionStart(node);

            if (node.Modifiers != Modifiers.None)
            {
                _writer.WriteModifiers(node.Modifiers);
                _writer.WriteSpace();
            }

            _writer.WriteKeyword(PrinterKeyword.Delegate);
            node.ParameterList.Accept(this);

            _writer.PushBraceFormatting(_writer.Configuration.BracesLayout.AnonymousMethodDeclaration, false);

            bool isSimple =
                _writer.Configuration.LineBreaksAndWrapping.Other.PlaceSimpleAnonymousMethodOnSingleLine &&
                IsSimpleBody(node.Block);

            if (isSimple)
                _writer.PushSingleLineBody(true);

            node.Block.Accept(this);

            if (isSimple)
                _writer.PopSingleLineBody();

            _writer.PopBraceFormatting();

            ExpressionEnd(node);
        }

        public void VisitAnonymousObjectCreationExpression(AnonymousObjectCreationExpressionSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            ExpressionStart(node);

            if (_writer.Configuration.Other.AlignMultiLineConstructs.ArrayObjectCollectionInitializer)
                _writer.SetAlignmentBreak(true);

            _writer.WriteKeyword(PrinterKeyword.New);

            _writer.PushBraceFormatting(_writer.Configuration.BracesLayout.ArrayAndObjectInitializer, false);

            _writer.BeginBlock();

            var wrapStyle = _writer.Configuration.LineBreaksAndWrapping.LineWrapping.WrapObjectAndCollectionInitializers;

            for (int i = 0; i < node.Initializers.Count; i++)
            {
                _writer.WriteIndent();

                node.Initializers[i].Accept(this);

                if (i != node.Initializers.Count - 1)
                {
                    _writer.WriteSyntax(Syntax.Comma);

                    if (wrapStyle != Configuration.WrapStyle.SimpleWrap)
                        _writer.Break(wrapStyle == Configuration.WrapStyle.ChopAlways);

                    _writer.WriteSpace();
                }
                else
                {
                    _writer.WriteLine(true);
                }
            }

            _writer.EndBlock();

            _writer.PopBraceFormatting();

            if (_writer.Configuration.Other.AlignMultiLineConstructs.ArrayObjectCollectionInitializer)
                _writer.SetAlignmentBreak(false);

            ExpressionEnd(node);
        }

        public void VisitAnonymousObjectMemberDeclarator(AnonymousObjectMemberDeclaratorSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            if (node.NameEquals != null)
                node.NameEquals.Accept(this);

            node.Expression.Accept(this);
        }

        public void VisitArgumentList(ArgumentListSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            bool spacesWithin;

            if (node.Arguments.Count == 0)
            {
                if (_writer.Configuration.Spaces.BeforeParentheses.MethodCallEmptyParentheses)
                    _writer.WriteSpace();
                spacesWithin = _writer.Configuration.Spaces.WithinParentheses.MethodCallEmptyParentheses;
            }
            else
            {
                if (_writer.Configuration.Spaces.BeforeParentheses.MethodCallParentheses)
                    _writer.WriteSpace();
                spacesWithin = _writer.Configuration.Spaces.WithinParentheses.MethodCallParentheses;
            }

            _writer.WriteSyntax(Syntax.OpenParen);

            if (_writer.Configuration.Other.AlignMultiLineConstructs.CallArguments)
                _writer.SetAlignmentBreak(true);

            if (spacesWithin)
                _writer.WriteSpace();

            bool hadOne = false;

            foreach (var argument in node.Arguments)
            {
                if (hadOne)
                    _writer.WriteListSeparator();
                else
                    hadOne = true;

                argument.Accept(this);
            }

            if (spacesWithin && node.Arguments.Count > 0)
                _writer.WriteSpace();

            if (_writer.Configuration.Other.AlignMultiLineConstructs.CallArguments)
                _writer.SetAlignmentBreak(false);

            _writer.WriteSyntax(Syntax.CloseParen);
        }

        public void VisitArgument(ArgumentSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            if (node.NameColon != null)
                node.NameColon.Accept(this);

            switch (node.Modifier)
            {
                case ParameterModifier.Ref:
                    _writer.WriteKeyword(PrinterKeyword.Ref);
                    _writer.WriteSpace();
                    break;

                case ParameterModifier.Out:
                    _writer.WriteKeyword(PrinterKeyword.Out);
                    _writer.WriteSpace();
                    break;
            }

            node.Expression.Accept(this);
        }

        public void VisitArrayCreationExpression(ArrayCreationExpressionSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            ExpressionStart(node);

            if (_writer.Configuration.Other.AlignMultiLineConstructs.ArrayObjectCollectionInitializer)
                _writer.SetAlignmentBreak(true);

            _writer.WriteKeyword(PrinterKeyword.New);
            _writer.WriteSpace();
            node.Type.Accept(this);

            if (node.Initializer != null)
            {
                _writer.PushBraceFormatting(_writer.Configuration.BracesLayout.ArrayAndObjectInitializer, false);

                node.Initializer.Accept(this);

                _writer.PopBraceFormatting();
            }

            if (_writer.Configuration.Other.AlignMultiLineConstructs.ArrayObjectCollectionInitializer)
                _writer.SetAlignmentBreak(false);

            ExpressionEnd(node);
        }

        public void VisitArrayRankSpecifier(ArrayRankSpecifierSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            if (_writer.Configuration.Spaces.Other.BeforeArrayRankBrackets)
                _writer.WriteSpace();

            _writer.WriteSyntax(Syntax.OpenBracket);

            if (
                node.Sizes.Count == 0 ||
                (node.Sizes.Count == 1 && node.Sizes[0].SyntaxKind == SyntaxKind.OmittedArraySizeExpression)
            ) {
                if (_writer.Configuration.Spaces.Other.WithinArrayRankEmptyBrackets)
                    _writer.WriteSpace();
            }
            else
            {
                if (_writer.Configuration.Spaces.Other.WithinArrayRankBrackets)
                    _writer.WriteSpace();

                bool hadOne = false;

                foreach (var size in node.Sizes)
                {
                    if (hadOne)
                    {
                        if (size.SyntaxKind == SyntaxKind.OmittedArraySizeExpression)
                            _writer.WriteSyntax(Syntax.Comma);
                        else
                            _writer.WriteListSeparator();
                    }
                    else
                    {
                        hadOne = true;
                    }

                    size.Accept(this);
                }

                if (_writer.Configuration.Spaces.Other.WithinArrayRankBrackets)
                    _writer.WriteSpace();
            }

            _writer.WriteSyntax(Syntax.CloseBracket);
        }

        public void VisitArrayType(ArrayTypeSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            ExpressionStart(node);

            node.ElementType.Accept(this);

            foreach (var rankSpecifier in node.RankSpecifiers)
            {
                rankSpecifier.Accept(this);
            }

            ExpressionEnd(node);
        }

        public void VisitAttributeArgumentList(AttributeArgumentListSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            _writer.WriteSyntax(Syntax.OpenParen);

            bool hadOne = false;

            foreach (var argument in node.Arguments)
            {
                if (hadOne)
                    _writer.WriteListSeparator();
                else
                    hadOne = true;

                argument.Accept(this);
            }

            _writer.WriteSyntax(Syntax.CloseParen);
        }

        public void VisitAttributeArgument(AttributeArgumentSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            if (node.NameColon != null)
                node.NameColon.Accept(this);
            if (node.NameEquals != null)
                node.NameEquals.Accept(this);

            node.Expression.Accept(this);
        }

        public void VisitAttributeList(AttributeListSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            _writer.WriteSyntax(Syntax.OpenBracket);

            if (_writer.Configuration.Spaces.Other.WithinAttributeBrackets)
                _writer.WriteSpace();

            if (node.Target != AttributeTarget.None)
            {
                switch (node.Target)
                {
                    case AttributeTarget.Assembly: _writer.WriteKeyword(PrinterKeyword.Assembly); break;
                    case AttributeTarget.Event: _writer.WriteKeyword(PrinterKeyword.Event); break;
                    case AttributeTarget.Field: _writer.WriteKeyword(PrinterKeyword.Field); break;
                    case AttributeTarget.Method: _writer.WriteKeyword(PrinterKeyword.Method); break;
                    case AttributeTarget.Module: _writer.WriteKeyword(PrinterKeyword.Module); break;
                    case AttributeTarget.Param: _writer.WriteKeyword(PrinterKeyword.Param); break;
                    case AttributeTarget.Property: _writer.WriteKeyword(PrinterKeyword.Property); break;
                    case AttributeTarget.Return: _writer.WriteKeyword(PrinterKeyword.Return); break;
                    case AttributeTarget.Type: _writer.WriteKeyword(PrinterKeyword.Type); break;
                    default: throw ThrowHelper.InvalidEnumValue(node.Target);
                }

                if (_writer.Configuration.Spaces.Other.BeforeColonInAttribute)
                    _writer.WriteSpace();

                _writer.WriteSyntax(Syntax.Colon);

                if (_writer.Configuration.Spaces.Other.AfterColonInAttribute)
                    _writer.WriteSpace();
            }

            bool hadOne = false;

            foreach (var attribute in node.Attributes)
            {
                if (hadOne)
                    _writer.WriteListSeparator();
                else
                    hadOne = true;

                attribute.Accept(this);
            }

            if (_writer.Configuration.Spaces.Other.WithinAttributeBrackets)
                _writer.WriteSpace();

            _writer.WriteSyntax(Syntax.CloseBracket);
        }

        public void VisitAttribute(AttributeSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            node.Name.Accept(this);

            if (node.ArgumentList != null)
                node.ArgumentList.Accept(this);
        }

        public void VisitBaseExpression(BaseExpressionSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            _writer.WriteKeyword(PrinterKeyword.Base);
        }

        public void VisitBaseList(BaseListSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            if (_writer.Configuration.Spaces.Other.BeforeBaseTypesListColon)
                _writer.WriteSpace();

            _writer.WriteSyntax(Syntax.Colon);

            if (_writer.Configuration.Spaces.Other.AfterBaseTypesListColon)
                _writer.WriteSpace();

            if (_writer.Configuration.Other.AlignMultiLineConstructs.ListOfBaseClassesAndInterfaces)
                _writer.SetAlignmentBreak(true);

            _writer.PushWrapStyle(_writer.Configuration.LineBreaksAndWrapping.LineWrapping.WrapExtendsImplementsList);

            bool hadOne = false;

            foreach (var type in node.Types)
            {
                if (hadOne)
                    _writer.WriteListSeparator();
                else
                    hadOne = true;

                type.Accept(this);
            }

            if (_writer.Configuration.Other.AlignMultiLineConstructs.ListOfBaseClassesAndInterfaces)
                _writer.SetAlignmentBreak(false);

            _writer.PopWrapStyle();
        }

        public void VisitBinaryExpression(BinaryExpressionSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            ExpressionStart(node);

            node.Left.Accept(this);

            bool writeSpace;
            bool wrap = false;

            switch (node.Operator)
            {
                case BinaryOperator.AsKeyword:
                case BinaryOperator.IsKeyword:
                    writeSpace = true;
                    break;

                case BinaryOperator.AmpersandEquals:
                case BinaryOperator.AsteriskEquals:
                case BinaryOperator.BarEquals:
                case BinaryOperator.CaretEquals:
                case BinaryOperator.Equals:
                case BinaryOperator.GreaterThanGreaterThanEquals:
                case BinaryOperator.LessThanLessThanEquals:
                case BinaryOperator.MinusEquals:
                case BinaryOperator.PercentEquals:
                case BinaryOperator.PlusEquals:
                case BinaryOperator.SlashEquals:
                    writeSpace = _writer.Configuration.Spaces.AroundOperators.AssignmentOperators;
                    break;

                case BinaryOperator.AmpersandAmpersand:
                case BinaryOperator.BarBar:
                    writeSpace = _writer.Configuration.Spaces.AroundOperators.LogicalOperators;
                    wrap = _wrapCompound;
                    break;

                case BinaryOperator.EqualsEquals:
                case BinaryOperator.ExclamationEquals:
                    writeSpace = _writer.Configuration.Spaces.AroundOperators.EqualityOperators;
                    break;

                case BinaryOperator.GreaterThan:
                case BinaryOperator.GreaterThanEquals:
                case BinaryOperator.LessThan:
                case BinaryOperator.LessThanEquals:
                    writeSpace = _writer.Configuration.Spaces.AroundOperators.RelationalOperators;
                    break;

                case BinaryOperator.Ampersand:
                case BinaryOperator.Bar:
                case BinaryOperator.Caret:
                    writeSpace = _writer.Configuration.Spaces.AroundOperators.BitwiseOperators;
                    break;

                case BinaryOperator.Minus:
                case BinaryOperator.Plus:
                    writeSpace = _writer.Configuration.Spaces.AroundOperators.AdditiveOperators;
                    break;

                case BinaryOperator.Asterisk:
                case BinaryOperator.Slash:
                case BinaryOperator.Percent:
                    writeSpace = _writer.Configuration.Spaces.AroundOperators.MultiplicativeOperators;
                    break;

                case BinaryOperator.GreaterThanGreaterThan:
                case BinaryOperator.LessThanLessThan:
                    writeSpace = _writer.Configuration.Spaces.AroundOperators.ShiftOperators;
                    break;

                case BinaryOperator.QuestionQuestion:
                    writeSpace = _writer.Configuration.Spaces.AroundOperators.NullCoalescingOperator;
                    break;

                default: throw ThrowHelper.InvalidEnumValue(node.Operator);
            }

            if (writeSpace)
                _writer.WriteSpace();

            if (_writer.Configuration.Other.AlignMultiLineConstructs.ChainedBinaryExpressions)
                _writer.SetAlignmentBreak(true);

            if (wrap && _writer.Configuration.LineBreaksAndWrapping.LineWrapping.PreferWrapBeforeOperatorInBinaryExpression)
                _writer.Break(true);

            switch (node.Operator)
            {
                case BinaryOperator.Ampersand: _writer.WriteOperator(PrinterOperator.Ampersand); break;
                case BinaryOperator.AmpersandAmpersand: _writer.WriteOperator(PrinterOperator.AmpersandAmpersand); break;
                case BinaryOperator.AmpersandEquals: _writer.WriteOperator(PrinterOperator.AmpersandEquals); break;
                case BinaryOperator.AsKeyword: _writer.WriteKeyword(PrinterKeyword.As); break;
                case BinaryOperator.Asterisk: _writer.WriteOperator(PrinterOperator.Asterisk); break;
                case BinaryOperator.AsteriskEquals: _writer.WriteOperator(PrinterOperator.AsteriskEquals); break;
                case BinaryOperator.Bar: _writer.WriteOperator(PrinterOperator.Bar); break;
                case BinaryOperator.BarBar: _writer.WriteOperator(PrinterOperator.BarBar); break;
                case BinaryOperator.BarEquals: _writer.WriteOperator(PrinterOperator.BarEquals); break;
                case BinaryOperator.Caret: _writer.WriteOperator(PrinterOperator.Caret); break;
                case BinaryOperator.CaretEquals: _writer.WriteOperator(PrinterOperator.CaretEquals); break;
                case BinaryOperator.Equals: _writer.WriteOperator(PrinterOperator.Equals); break;
                case BinaryOperator.EqualsEquals: _writer.WriteOperator(PrinterOperator.EqualsEquals); break;
                case BinaryOperator.ExclamationEquals: _writer.WriteOperator(PrinterOperator.ExclamationEquals); break;
                case BinaryOperator.GreaterThan: _writer.WriteOperator(PrinterOperator.GreaterThan); break;
                case BinaryOperator.GreaterThanEquals: _writer.WriteOperator(PrinterOperator.GreaterThanEquals); break;
                case BinaryOperator.GreaterThanGreaterThan: _writer.WriteOperator(PrinterOperator.GreaterThanGreaterThan); break;
                case BinaryOperator.GreaterThanGreaterThanEquals: _writer.WriteOperator(PrinterOperator.GreaterThanGreaterThanEquals); break;
                case BinaryOperator.IsKeyword: _writer.WriteKeyword(PrinterKeyword.Is); break;
                case BinaryOperator.LessThan: _writer.WriteOperator(PrinterOperator.LessThan); break;
                case BinaryOperator.LessThanEquals: _writer.WriteOperator(PrinterOperator.LessThanEquals); break;
                case BinaryOperator.LessThanLessThan: _writer.WriteOperator(PrinterOperator.LessThanLessThan); break;
                case BinaryOperator.LessThanLessThanEquals: _writer.WriteOperator(PrinterOperator.LessThanLessThanEquals); break;
                case BinaryOperator.Minus: _writer.WriteOperator(PrinterOperator.Minus); break;
                case BinaryOperator.MinusEquals: _writer.WriteOperator(PrinterOperator.MinusEquals); break;
                case BinaryOperator.Percent: _writer.WriteOperator(PrinterOperator.Percent); break;
                case BinaryOperator.PercentEquals: _writer.WriteOperator(PrinterOperator.PercentEquals); break;
                case BinaryOperator.Plus: _writer.WriteOperator(PrinterOperator.Plus); break;
                case BinaryOperator.PlusEquals: _writer.WriteOperator(PrinterOperator.PlusEquals); break;
                case BinaryOperator.QuestionQuestion: _writer.WriteOperator(PrinterOperator.QuestionQuestion); break;
                case BinaryOperator.Slash: _writer.WriteOperator(PrinterOperator.Slash); break;
                case BinaryOperator.SlashEquals: _writer.WriteOperator(PrinterOperator.SlashEquals); break;
                default: throw ThrowHelper.InvalidEnumValue(node.Operator);
            }

            if (wrap && !_writer.Configuration.LineBreaksAndWrapping.LineWrapping.PreferWrapBeforeOperatorInBinaryExpression)
                _writer.Break(true);

            if (writeSpace)
                _writer.WriteSpace();

            node.Right.Accept(this);

            if (_writer.Configuration.Other.AlignMultiLineConstructs.ChainedBinaryExpressions)
                _writer.SetAlignmentBreak(false);

            ExpressionEnd(node);
        }

        private void ExpressionStart(ExpressionSyntax node)
        {
            if (
                _writer.Configuration.Other.AlignMultiLineConstructs.Expression &&
                !(node.Parent is ExpressionSyntax)
            )
                _writer.SetAlignmentBreak(true);
        }

        private void ExpressionEnd(ExpressionSyntax node)
        {
            if (
                _writer.Configuration.Other.AlignMultiLineConstructs.Expression &&
                !(node.Parent is ExpressionSyntax)
            )
                _writer.SetAlignmentBreak(false);
        }

        public void VisitBlock(BlockSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            bool writeNewlineBefore = !(
                node.Parent is BlockSyntax ||
                node.Parent is LabeledStatementSyntax
            );

            if (!node.ChildNodes().Any())
            {
                _writer.EmptyBlock(writeNewlineBefore);
            }
            else
            {
                _writer.BeginBlock(writeNewlineBefore, true);

                _writer.PushBraceFormatting(_writer.Configuration.BracesLayout.Other);

                foreach (var statement in node.Statements)
                {
                    statement.Accept(this);
                }

                _writer.PopBraceFormatting();

                _writer.EndBlock();
            }

            WriteTrailingTrivia(node);
        }

        public void VisitBracketedArgumentList(BracketedArgumentListSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            if (_writer.Configuration.Spaces.BeforeParentheses.ArrayAccessBrackets)
                _writer.WriteSpace();

            _writer.WriteSyntax(Syntax.OpenBracket);

            if (_writer.Configuration.Spaces.WithinParentheses.ArrayAccessBrackets)
                _writer.WriteSpace();

            bool hadOne = false;

            foreach (var parameter in node.Arguments)
            {
                if (hadOne)
                    _writer.WriteListSeparator();
                else
                    hadOne = true;

                parameter.Accept(this);
            }

            if (
                _writer.Configuration.Spaces.WithinParentheses.ArrayAccessBrackets &&
                node.Arguments.Count > 0
            )
                _writer.WriteSpace();

            _writer.WriteSyntax(Syntax.CloseBracket);
        }

        public void VisitBracketedParameterList(BracketedParameterListSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            _writer.WriteSyntax(Syntax.OpenBracket);

            bool hadOne = false;

            foreach (var parameter in node.Parameters)
            {
                if (hadOne)
                    _writer.WriteListSeparator();
                else
                    hadOne = true;

                parameter.Accept(this);
            }

            _writer.WriteSyntax(Syntax.CloseBracket);
        }

        public void VisitBreakStatement(BreakStatementSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            _writer.WriteIndent();
            _writer.WriteKeyword(PrinterKeyword.Break);
            _writer.EndStatement();

            WriteTrailingTrivia(node);
        }

        public void VisitCastExpression(CastExpressionSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            ExpressionStart(node);

            _writer.WriteSyntax(Syntax.OpenParen);

            if (_writer.Configuration.Spaces.WithinParentheses.TypeCastParentheses)
                _writer.WriteSpace();

            node.Type.Accept(this);

            if (_writer.Configuration.Spaces.WithinParentheses.TypeCastParentheses)
                _writer.WriteSpace();
            
            _writer.WriteSyntax(Syntax.CloseParen);

            if (_writer.Configuration.Spaces.Other.AfterTypeCastParentheses)
                _writer.WriteSpace();

            node.Expression.Accept(this);

            ExpressionEnd(node);
        }

        public void VisitCatchClause(CatchClauseSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            if (_writer.Configuration.LineBreaksAndWrapping.PlaceOnNewLine.PlaceCatchOnNewLine)
                _writer.WriteIndent();
            else
                _writer.WriteSpace();

            _writer.WriteKeyword(PrinterKeyword.Catch);

            if (node.Declaration != null)
            {
                if (_writer.Configuration.Spaces.BeforeParentheses.CatchParentheses)
                    _writer.WriteSpace();

                node.Declaration.Accept(this);
            }

            node.Block.Accept(this);
        }

        public void VisitCatchDeclaration(CatchDeclarationSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            _writer.WriteSyntax(Syntax.OpenParen);

            if (_writer.Configuration.Spaces.WithinParentheses.CatchParentheses)
                _writer.WriteSpace();

            node.Type.Accept(this);

            if (node.Identifier != null)
            {
                _writer.WriteSpace();
                _writer.WriteIdentifier(node.Identifier);
            }

            if (_writer.Configuration.Spaces.WithinParentheses.CatchParentheses)
                _writer.WriteSpace();

            _writer.WriteSyntax(Syntax.CloseParen);
        }

        public void VisitCheckedExpression(CheckedExpressionSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            ExpressionStart(node);

            _writer.WriteKeyword(
                node.Kind == CheckedOrUnchecked.Checked
                ? PrinterKeyword.Checked
                : PrinterKeyword.Unchecked
            );

            _writer.WriteSpace();
            _writer.WriteSyntax(Syntax.OpenParen);
            node.Expression.Accept(this);
            _writer.WriteSyntax(Syntax.CloseParen);

            ExpressionEnd(node);
        }

        public void VisitCheckedStatement(CheckedStatementSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            _writer.WriteIndent();

            _writer.WriteKeyword(
                node.Kind == CheckedOrUnchecked.Checked
                ? PrinterKeyword.Checked
                : PrinterKeyword.Unchecked
            );

            node.Block.Accept(this);

            WriteTrailingTrivia(node);
        }

        public void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            VisitTypeDeclaration(node);
        }

        private void VisitTypeDeclaration(TypeDeclarationSyntax node)
        {
            VisitBaseTypeDeclaration(node);

            _writer.PushBraceFormatting(_writer.Configuration.BracesLayout.TypeAndNamespaceDeclaration);

            if (node.Members.Count == 0)
            {
                _writer.EmptyBlock(_writer.Configuration.BlankLines.InsideType);
            }
            else
            {
                _writer.BeginBlock();

                _writer.WriteLine(_writer.Configuration.BlankLines.InsideType);

                int newline = 0;

                for (int i = 0; i < node.Members.Count; i++)
                {
                    var member = node.Members[i];

                    if (member.SyntaxKind == SyntaxKind.FieldDeclaration)
                        newline = Math.Max(newline, _writer.Configuration.BlankLines.AroundField);
                    else if (member is BaseTypeDeclarationSyntax)
                        newline = Math.Max(newline, _writer.Configuration.BlankLines.AroundType);
                    else
                        newline = Math.Max(newline, _writer.Configuration.BlankLines.AroundMethod);

                    if (i > 0)
                        _writer.WriteLine(newline);

                    member.Accept(this);

                    if (member.SyntaxKind == SyntaxKind.FieldDeclaration)
                        newline = _writer.Configuration.BlankLines.AroundField;
                    else if (member is BaseTypeDeclarationSyntax)
                        newline = _writer.Configuration.BlankLines.AroundType;
                    else
                        newline = _writer.Configuration.BlankLines.AroundMethod;
                }

                _writer.WriteLine(_writer.Configuration.BlankLines.InsideType);

                _writer.EndBlock();
            }

            _writer.PopBraceFormatting();

            WriteTrailingTrivia(node);
        }

        private void VisitBaseTypeDeclaration(BaseTypeDeclarationSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            var typeDeclaration = node as TypeDeclarationSyntax;

            _writer.WriteIndent();

            WriteAttributes(
                node,
                _writer.Configuration.LineBreaksAndWrapping.Other.PlaceTypeAttributeOnSingleLine
            );

            WriteTypeModifiers(GetGlobalMemberModifiers(node, node.Modifiers));

            switch (node.SyntaxKind)
            {
                case SyntaxKind.ClassDeclaration: _writer.WriteKeyword(PrinterKeyword.Class); break;
                case SyntaxKind.StructDeclaration: _writer.WriteKeyword(PrinterKeyword.Struct); break;
                case SyntaxKind.InterfaceDeclaration: _writer.WriteKeyword(PrinterKeyword.Interface); break;
                case SyntaxKind.EnumDeclaration: _writer.WriteKeyword(PrinterKeyword.Enum); break;
            }

            _writer.WriteSpace();
            _writer.WriteIdentifier(node.Identifier);

            if (typeDeclaration != null && typeDeclaration.TypeParameterList != null)
                typeDeclaration.TypeParameterList.Accept(this);

            if (node.BaseList != null)
                node.BaseList.Accept(this);

            if (typeDeclaration != null && typeDeclaration.ConstraintClauses.Count > 0)
                WriteConstraintClauses(typeDeclaration.ConstraintClauses);
        }

        private Modifiers GetGlobalMemberModifiers(SyntaxNode node, Modifiers modifiers)
        {
            if ((modifiers & AllAccessModifiers) == 0)
            {
                if (node.Parent is BaseTypeDeclarationSyntax)
                {
                    if (_writer.Configuration.Other.Modifiers.UseExplicitPrivateModifier)
                        modifiers |= Modifiers.Private;
                }
                else
                {
                    if (_writer.Configuration.Other.Modifiers.UseExplicitInternalModifier)
                        modifiers |= Modifiers.Internal;
                }
            }

            return modifiers;
        }

        private void WriteTypeModifiers(Modifiers modifiers)
        {
            bool partial = (modifiers & Modifiers.Partial) != 0;
            modifiers = modifiers & ~Modifiers.Partial;

            if (modifiers != Modifiers.None)
            {
                _writer.WriteModifiers(modifiers);
                _writer.WriteSpace();
            }

            if (partial)
            {
                _writer.WriteKeyword(PrinterKeyword.Partial);
                _writer.WriteSpace();
            }
        }

        public void VisitClassOrStructConstraint(ClassOrStructConstraintSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            switch (node.Kind)
            {
                case ClassOrStruct.Class: _writer.WriteKeyword(PrinterKeyword.Class); break;
                case ClassOrStruct.Struct: _writer.WriteKeyword(PrinterKeyword.Struct); break;
            }
        }

        public void VisitCompilationUnit(CompilationUnitSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            if (node.LeadingTrivia.Count > 0)
                _writer.WriteLine(_writer.Configuration.BlankLines.AfterFileHeaderComment);

            WriteGlobalNodes(
                node.Usings,
                node.Externs,
                node.Members,
                node.AttributeLists
            );

            WriteTrailingTrivia(node);
        }

        private void WriteGlobalNodes(SyntaxList<UsingDirectiveSyntax> usings, SyntaxList<ExternAliasDirectiveSyntax> externs, SyntaxList<MemberDeclarationSyntax> members, SyntaxList<AttributeListSyntax> attributeLists)
        {
            string lastUsingGroup = null;

            foreach (var @using in usings)
            {
                string usingGroup;

                var name = @using.Name;

                while (name.SyntaxKind == SyntaxKind.QualifiedName)
                {
                    name = ((QualifiedNameSyntax)name).Left;
                }

                if (name.SyntaxKind == SyntaxKind.IdentifierName)
                    usingGroup = ((IdentifierNameSyntax)name).Identifier;
                else
                    usingGroup = null;

                if (lastUsingGroup != null && usingGroup != lastUsingGroup)
                    _writer.WriteLine(_writer.Configuration.BlankLines.BetweenDifferentUsingGroups);

                @using.Accept(this);

                lastUsingGroup = usingGroup;
            }

            foreach (var @extern in externs)
            {
                @extern.Accept(this);
            }

            if (attributeLists != null && attributeLists.Count > 0)
            {
                if (usings.Count > 0 || externs.Count > 0)
                    _writer.WriteLine();

                foreach (var attribute in attributeLists)
                {
                    _writer.WriteIndent();
                    attribute.Accept(this);
                    _writer.WriteLine();
                }
            }

            int newlines;

            if (
                usings.Count > 0 ||
                externs.Count > 0 ||
                (attributeLists != null && attributeLists.Count > 0)
            )
                newlines = _writer.Configuration.BlankLines.AfterUsingList;
            else
                newlines = 0;

            if (members.Count > 0)
            {
                for (int i = 0; i < members.Count; i++)
                {
                    var member = members[i];

                    if (i > 0 || newlines > 0)
                    {
                        if (member.SyntaxKind == SyntaxKind.NamespaceDeclaration)
                            newlines = Math.Max(newlines, _writer.Configuration.BlankLines.AroundNamespace);
                        else if (member is BaseTypeDeclarationSyntax)
                            newlines = Math.Max(newlines, _writer.Configuration.BlankLines.AroundType);
                        else
                            newlines = Math.Max(newlines, 1);

                        _writer.WriteLine(newlines);
                    }

                    member.Accept(this);

                    if (member.SyntaxKind == SyntaxKind.NamespaceDeclaration)
                        newlines = _writer.Configuration.BlankLines.AroundNamespace;
                    else if (member is BaseTypeDeclarationSyntax)
                        newlines = _writer.Configuration.BlankLines.AroundType;
                    else
                        newlines = 1;
                }
            }
        }

        public void VisitConditionalExpression(ConditionalExpressionSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            ExpressionStart(node);

            node.Condition.Accept(this);

            if (_writer.Configuration.Spaces.TernaryOperator.BeforeQuestionMark)
                _writer.WriteSpace();

            var wrap = _writer.Configuration.LineBreaksAndWrapping.LineWrapping.WrapTernaryExpression;

            if (wrap != Configuration.WrapStyle.SimpleWrap)
                _writer.Break(wrap == Configuration.WrapStyle.ChopAlways);

            _writer.WriteSyntax(Syntax.Question);

            if (_writer.Configuration.Spaces.TernaryOperator.AfterQuestionMark)
                _writer.WriteSpace();

            node.WhenTrue.Accept(this);

            if (_writer.Configuration.Spaces.TernaryOperator.BeforeColon)
                _writer.WriteSpace();

            if (wrap != Configuration.WrapStyle.SimpleWrap)
                _writer.Break(wrap == Configuration.WrapStyle.ChopAlways);

            _writer.WriteSyntax(Syntax.Colon);

            if (_writer.Configuration.Spaces.TernaryOperator.AfterColon)
                _writer.WriteSpace();

            node.WhenFalse.Accept(this);

            ExpressionEnd(node);
        }

        public void VisitConstructorConstraint(ConstructorConstraintSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            _writer.WriteKeyword(PrinterKeyword.New);
            _writer.WriteSyntax(Syntax.OpenParen);
            _writer.WriteSyntax(Syntax.CloseParen);
        }

        public void VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            _writer.WriteIndent();

            WriteAttributes(
                node,
                _writer.Configuration.LineBreaksAndWrapping.Other.PlaceMethodAttributeOnSameLine
            );

            WriteMemberModifiers(node.Modifiers);

            _writer.WriteIdentifier(node.Identifier);

            node.ParameterList.Accept(this);

            if (node.Initializer != null)
                node.Initializer.Accept(this);

            node.Body.Accept(this);

            WriteTrailingTrivia(node);
        }

        private void WriteMemberModifiers(Modifiers modifiers)
        {
            if (
                (modifiers & AllAccessModifiers) == 0 &&
                _writer.Configuration.Other.Modifiers.UseExplicitPrivateModifier
            )
                modifiers |= Modifiers.Private;

            if (modifiers != Modifiers.None)
            {
                _writer.WriteModifiers(modifiers);
                _writer.WriteSpace();
            }
        }

        public void VisitConstructorInitializer(ConstructorInitializerSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            if (_writer.Configuration.LineBreaksAndWrapping.Other.PlaceConstructorInitializerOnSameLine)
            {
                _writer.WriteSpace();
            }
            else
            {

                _writer.WriteLine();

                _writer.PushIndent();
                _writer.WriteIndent();
            }

            _writer.WriteSyntax(Syntax.Colon);
            _writer.WriteSpace();
            _writer.WriteKeyword(node.Kind == ThisOrBase.This ? PrinterKeyword.This : PrinterKeyword.Base);
            node.ArgumentList.Accept(this);

            if (!_writer.Configuration.LineBreaksAndWrapping.Other.PlaceConstructorInitializerOnSameLine)
                _writer.PopIndent();
        }

        public void VisitContinueStatement(ContinueStatementSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            _writer.WriteIndent();
            _writer.WriteKeyword(PrinterKeyword.Continue);
            _writer.EndStatement();

            WriteTrailingTrivia(node);
        }

        public void VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            _writer.WriteIndent();

            WriteAttributes(
                node,
                _writer.Configuration.LineBreaksAndWrapping.Other.PlaceMethodAttributeOnSameLine
            );

            WriteMemberModifiers(node.Modifiers);

            _writer.WriteKeyword(node.Kind == ImplicitOrExplicit.Explicit ? PrinterKeyword.Explicit : PrinterKeyword.Implicit);
            _writer.WriteSpace();
            _writer.WriteKeyword(PrinterKeyword.Operator);
            _writer.WriteSpace();
            node.Type.Accept(this);
            node.ParameterList.Accept(this);

            node.Body.Accept(this);

            WriteTrailingTrivia(node);
        }

        public void VisitDefaultExpression(DefaultExpressionSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            ExpressionStart(node);

            _writer.WriteKeyword(PrinterKeyword.Default);
            _writer.WriteSyntax(Syntax.OpenParen);
            node.Type.Accept(this);
            _writer.WriteSyntax(Syntax.CloseParen);

            ExpressionEnd(node);
        }

        public void VisitDelegateDeclaration(DelegateDeclarationSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            _writer.WriteIndent();

            WriteAttributes(
                node,
                _writer.Configuration.LineBreaksAndWrapping.Other.PlaceMethodAttributeOnSameLine
            );

            var modifiers = GetGlobalMemberModifiers(node, node.Modifiers);
            
            if (modifiers != Modifiers.None)
            {
                _writer.WriteModifiers(modifiers);
                _writer.WriteSpace();
            }

            _writer.WriteKeyword(PrinterKeyword.Delegate);
            _writer.WriteSpace();
            node.ReturnType.Accept(this);
            _writer.WriteSpace();
            _writer.WriteIdentifier(node.Identifier);

            if (node.TypeParameterList != null)
                node.TypeParameterList.Accept(this);

            node.ParameterList.Accept(this);

            if (node.ConstraintClauses.Count > 0)
                WriteConstraintClauses(node.ConstraintClauses);

            _writer.EndStatement();

            WriteTrailingTrivia(node);
        }

        private void WriteConstraintClauses(IEnumerable<TypeParameterConstraintClauseSyntax> constraintClauses)
        {
            if (_writer.Configuration.LineBreaksAndWrapping.Other.PlaceTypeConstraintsOnSameLine)
                _writer.WriteSpace();

            if (_writer.Configuration.Other.AlignMultiLineConstructs.TypeParameterConstraints)
                _writer.SetAlignmentBreak(true);

            if (!_writer.Configuration.LineBreaksAndWrapping.Other.PlaceTypeConstraintsOnSameLine)
                _writer.Break(true);

            bool hadOne = false;

            foreach (var constraintClause in constraintClauses)
            {
                var wrapStyle = _writer.Configuration.LineBreaksAndWrapping.LineWrapping.WrapMultipleTypeParameterConstraints;

                if (hadOne)
                {
                    if (wrapStyle != Configuration.WrapStyle.SimpleWrap)
                        _writer.Break(wrapStyle == Configuration.WrapStyle.ChopAlways);

                    _writer.WriteSpace();
                }
                else
                {
                    hadOne = true;
                }

                constraintClause.Accept(this);
            }

            if (_writer.Configuration.Other.AlignMultiLineConstructs.TypeParameterConstraints)
                _writer.SetAlignmentBreak(false);
        }

        public void VisitDestructorDeclaration(DestructorDeclarationSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            _writer.WriteIndent();

            WriteAttributes(
                node,
                _writer.Configuration.LineBreaksAndWrapping.Other.PlaceMethodAttributeOnSameLine
            );

            _writer.WriteSyntax(Syntax.Tilde);
            _writer.WriteIdentifier(node.Identifier);
            node.ParameterList.Accept(this);

            node.Body.Accept(this);

            WriteTrailingTrivia(node);
        }

        public void VisitDoStatement(DoStatementSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            _writer.WriteIndent();
            _writer.WriteKeyword(PrinterKeyword.Do);

            if (!_writer.Configuration.LineBreaksAndWrapping.PlaceOnNewLine.PlaceWhileOnNewLine)
                _writer.PushBraceFormatting(_writer.Configuration.BracesLayout.Other, false);

            VisitBlockStatement(node.Statement);

            if (!_writer.Configuration.LineBreaksAndWrapping.PlaceOnNewLine.PlaceWhileOnNewLine)
            {
                _writer.PopBraceFormatting();
                _writer.WriteSpace();
            }
            else
            {
                _writer.WriteIndent();
            }

            _writer.WriteKeyword(PrinterKeyword.While);

            if (_writer.Configuration.Spaces.BeforeParentheses.WhileParentheses)
                _writer.WriteSpace();

            _writer.WriteSyntax(Syntax.OpenParen);

            if (_writer.Configuration.Spaces.WithinParentheses.WhileParentheses)
                _writer.WriteSpace();

            _wrapCompound = _writer.Configuration.LineBreaksAndWrapping.LineWrapping.ForceChopCompoundConditionInDoStatement;

            node.Condition.Accept(this);

            _wrapCompound = false;

            if (_writer.Configuration.Spaces.WithinParentheses.WhileParentheses)
                _writer.WriteSpace();

            _writer.WriteSyntax(Syntax.CloseParen);
            _writer.EndStatement();

            WriteTrailingTrivia(node);
        }

        private void VisitBlockStatement(StatementSyntax statement)
        {
            if (statement is BlockSyntax)
            {
                statement.Accept(this);
            }
            else
            {
                _writer.WriteLine();
                _writer.PushIndent();

                statement.Accept(this);

                _writer.PopIndent();
            }
        }

        public void VisitElementAccessExpression(ElementAccessExpressionSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            ExpressionStart(node);

            node.Expression.Accept(this);
            node.ArgumentList.Accept(this);

            ExpressionEnd(node);
        }

        public void VisitElseClause(ElseClauseSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            _writer.WriteKeyword(PrinterKeyword.Else);

            if (
                _writer.Configuration.Other.Other.SpecialElseIfTreatment &&
                node.Statement is IfStatementSyntax
            ) {
                _writer.WriteSpace();
                node.Statement.Accept(this);
            }
            else
            {
                VisitBlockStatement(node.Statement);
            }
        }

        public void VisitEmptyStatement(EmptyStatementSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            _writer.WriteIndent();
            _writer.EndStatement();

            WriteTrailingTrivia(node);
        }

        public void VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            VisitBaseTypeDeclaration(node);

            _writer.PushBraceFormatting(_writer.Configuration.BracesLayout.TypeAndNamespaceDeclaration);

            if (node.Members.Count == 0)
            {
                _writer.EmptyBlock(_writer.Configuration.BlankLines.InsideType);
            }
            else
            {
                _writer.BeginBlock();

                _writer.WriteLine(_writer.Configuration.BlankLines.InsideType);

                for (int i = 0; i < node.Members.Count; i++)
                {
                    if (i > 0)
                        _writer.WriteLine(_writer.Configuration.BlankLines.AroundField);

                    node.Members[i].Accept(this);
                }

                _writer.WriteLine(_writer.Configuration.BlankLines.InsideType);

                _writer.EndBlock();
            }

            _writer.PopBraceFormatting();

            WriteTrailingTrivia(node);
        }

        public void VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            _writer.WriteIndent();

            WriteAttributes(
                node,
                _writer.Configuration.LineBreaksAndWrapping.Other.PlaceFieldAttributeOnSameLine
            );

            _writer.WriteIdentifier(node.Identifier);

            if (node.EqualsValue != null)
                node.EqualsValue.Accept(this);

            var parent = node.Parent as EnumDeclarationSyntax;

            if (
                parent == null ||
                parent.Members.IndexOf(node) < parent.Members.Count - 1
            )
                _writer.WriteSyntax(Syntax.Comma);

            _writer.WriteLine();

            WriteTrailingTrivia(node);
        }

        public void VisitEqualsValueClause(EqualsValueClauseSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            _writer.WriteSpace();
            _writer.WriteOperator(PrinterOperator.Equals);
            _writer.WriteSpace();

            node.Value.Accept(this);
        }

        public void VisitEventDeclaration(EventDeclarationSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            _writer.WriteIndent();

            WriteAttributes(
                node,
                _writer.Configuration.LineBreaksAndWrapping.Other.PlacePropertyIndexerEventAttributeOnSameLine
            );

            WriteMemberModifiers(node.Modifiers);

            _writer.WriteKeyword(PrinterKeyword.Event);
            _writer.WriteSpace();

            node.Type.Accept(this);

            _writer.WriteSpace();

            if (node.ExplicitInterfaceSpecifier != null)
                node.ExplicitInterfaceSpecifier.Accept(this);

            _writer.WriteIdentifier(node.Identifier);

            node.AccessorList.Accept(this);

            WriteTrailingTrivia(node);
        }

        public void VisitEventFieldDeclaration(EventFieldDeclarationSyntax node)
        {
            VisitBaseFieldDeclaration(node);
        }

        private void VisitBaseFieldDeclaration(BaseFieldDeclarationSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            WriteLeadingTrivia(node);

            node.Validate();

            _writer.WriteIndent();

            WriteAttributes(
                node,
                _writer.Configuration.LineBreaksAndWrapping.Other.PlaceFieldAttributeOnSameLine
            );

            WriteMemberModifiers(node.Modifiers);

            if (node.SyntaxKind == SyntaxKind.EventFieldDeclaration)
            {
                _writer.WriteKeyword(PrinterKeyword.Event);
                _writer.WriteSpace();
            }

            if ((node.Modifiers & Modifiers.Const) != 0)
            {
                _writer.WriteKeyword(PrinterKeyword.Const);
                _writer.WriteSpace();
            }
            
            node.Declaration.Accept(this);

            _writer.EndStatement();

            WriteTrailingTrivia(node);
        }

        public void VisitExplicitInterfaceSpecifier(ExplicitInterfaceSpecifierSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            node.Name.Accept(this);
            _writer.WriteSyntax(Syntax.Dot);
        }

        public void VisitExpressionStatement(ExpressionStatementSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            _writer.WriteIndent();
            node.Expression.Accept(this);
            _writer.EndStatement();

            WriteTrailingTrivia(node);
        }

        public void VisitExternAliasDirective(ExternAliasDirectiveSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            _writer.WriteIndent();
            _writer.WriteKeyword(PrinterKeyword.Extern);
            _writer.WriteSpace();
            _writer.WriteKeyword(PrinterKeyword.Alias);
            _writer.WriteSpace();
            _writer.WriteIdentifier(node.Identifier);
            _writer.EndStatement();

            WriteTrailingTrivia(node);
        }

        public void VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            VisitBaseFieldDeclaration(node);
        }

        public void VisitFinallyClause(FinallyClauseSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            if (_writer.Configuration.LineBreaksAndWrapping.PlaceOnNewLine.PlaceFinallyOnNewLine)
                _writer.WriteIndent();
            else
                _writer.WriteSpace();

            _writer.WriteKeyword(PrinterKeyword.Finally);

            node.Block.Accept(this);
        }

        public void VisitForEachStatement(ForEachStatementSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            _writer.WriteIndent();
            _writer.WriteKeyword(PrinterKeyword.ForEach);

            if (_writer.Configuration.Spaces.BeforeParentheses.ForEachParentheses)
                _writer.WriteSpace();
            
            _writer.WriteSyntax(Syntax.OpenParen);

            if (_writer.Configuration.Spaces.WithinParentheses.ForEachParentheses)
                _writer.WriteSpace();

            node.Type.Accept(this);
            _writer.WriteSpace();
            _writer.WriteIdentifier(node.Identifier);
            _writer.WriteSpace();
            _writer.WriteKeyword(PrinterKeyword.In);
            _writer.WriteSpace();
            node.Expression.Accept(this);

            if (_writer.Configuration.Spaces.WithinParentheses.ForEachParentheses)
                _writer.WriteSpace();

            _writer.WriteSyntax(Syntax.CloseParen);

            VisitBlockStatement(node.Statement);

            WriteTrailingTrivia(node);
        }

        public void VisitForStatement(ForStatementSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            _writer.WriteIndent();
            _writer.WriteKeyword(PrinterKeyword.For);

            if (_writer.Configuration.Spaces.BeforeParentheses.ForParentheses)
                _writer.WriteSpace();

            _writer.WriteSyntax(Syntax.OpenParen);

            if (_writer.Configuration.Other.AlignMultiLineConstructs.ForStatementHeader)
                _writer.SetAlignmentBreak(true);

            if (_writer.Configuration.Spaces.WithinParentheses.ForParentheses)
                _writer.WriteSpace();

            bool hadOne = false;

            if (node.Declaration != null)
                node.Declaration.Accept(this);

            foreach (var initializer in node.Initializers)
            {
                if (hadOne)
                    _writer.WriteListSeparator();
                else
                    hadOne = true;

                initializer.Accept(this);
            }

            _writer.WriteStatementSeparator(_writer.Configuration.LineBreaksAndWrapping.LineWrapping.WrapForStatementHeader);

            if (node.Condition != null)
                node.Condition.Accept(this);

            _writer.WriteStatementSeparator(_writer.Configuration.LineBreaksAndWrapping.LineWrapping.WrapForStatementHeader);

            hadOne = false;

            foreach (var incrementor in node.Incrementors)
            {
                if (hadOne)
                    _writer.WriteListSeparator();
                else
                    hadOne = true;

                incrementor.Accept(this);
            }

            if (_writer.Configuration.Spaces.WithinParentheses.ForParentheses)
                _writer.WriteSpace();

            _writer.WriteSyntax(Syntax.CloseParen);

            if (_writer.Configuration.Other.AlignMultiLineConstructs.ForStatementHeader)
                _writer.SetAlignmentBreak(false);

            VisitBlockStatement(node.Statement);

            WriteTrailingTrivia(node);
        }

        public void VisitGenericName(GenericNameSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            ExpressionStart(node);

            _writer.WriteIdentifier(node.Identifier);
            node.TypeArgumentList.Accept(this);

            ExpressionEnd(node);
        }

        public void VisitGotoStatement(GotoStatementSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            _writer.WriteIndent();
            _writer.WriteKeyword(PrinterKeyword.Goto);

            switch (node.Kind)
            {
                case CaseOrDefault.Case:
                    _writer.WriteSpace();
                    _writer.WriteKeyword(PrinterKeyword.Case);
                    break;

                case CaseOrDefault.Default:
                    _writer.WriteSpace();
                    _writer.WriteKeyword(PrinterKeyword.Default);
                    break;
            }

            if (node.Expression != null)
            {
                _writer.WriteSpace();
                node.Expression.Accept(this);
            }

            _writer.EndStatement();

            WriteTrailingTrivia(node);
        }

        public void VisitIdentifierName(IdentifierNameSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            ExpressionStart(node);

            if (node.IsVar)
                _writer.WriteKeyword(PrinterKeyword.Var);
            else if (node.Identifier == "global")
                _writer.WriteKeyword(PrinterKeyword.Global);
            else
                _writer.WriteIdentifier(node.Identifier);

            ExpressionEnd(node);
        }

        public void VisitIfStatement(IfStatementSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            bool parentIsElse = node.Parent is ElseClauseSyntax;

            if (
                parentIsElse &&
                (node.LeadingTrivia.Count > 0 || node.TrailingTrivia.Count > 0)
            ) {
                throw new CSharpSyntaxException(String.Format(
                    "Trivia on else if node '{0}' is not supported",
                    node
                ));
            }

            WriteLeadingTrivia(node);

            if (
                !_writer.Configuration.Other.Other.SpecialElseIfTreatment ||
                !parentIsElse
            )
                _writer.WriteIndent();

            _writer.WriteKeyword(PrinterKeyword.If);

            if (_writer.Configuration.Spaces.BeforeParentheses.IfParentheses)
                _writer.WriteSpace();
            
            _writer.WriteSyntax(Syntax.OpenParen);

            if (_writer.Configuration.Spaces.WithinParentheses.IfParentheses)
                _writer.WriteSpace();

            _wrapCompound = _writer.Configuration.LineBreaksAndWrapping.LineWrapping.ForceChopCompoundConditionInIfStatement;

            node.Condition.Accept(this);

            _wrapCompound = false;

            if (_writer.Configuration.Spaces.WithinParentheses.IfParentheses)
                _writer.WriteSpace();

            _writer.WriteSyntax(Syntax.CloseParen);

            bool noNewline =
                node.Else != null &&
                !_writer.Configuration.LineBreaksAndWrapping.PlaceOnNewLine.PlaceElseOnNewLine;

            if (noNewline)
                _writer.PushBraceFormatting(_writer.Configuration.BracesLayout.Other, false);

            VisitBlockStatement(node.Statement);

            if (noNewline)
            {
                _writer.PopBraceFormatting();
                _writer.WriteSpace();
            }
            else if (node.Else != null)
            {
                _writer.WriteIndent();
            }

            if (node.Else != null)
                node.Else.Accept(this);

            WriteTrailingTrivia(node);
        }

        public void VisitImplicitArrayCreationExpression(ImplicitArrayCreationExpressionSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            ExpressionStart(node);

            if (_writer.Configuration.Other.AlignMultiLineConstructs.ArrayObjectCollectionInitializer)
                _writer.SetAlignmentBreak(true);

            _writer.WriteKeyword(PrinterKeyword.New);
            _writer.WriteSyntax(Syntax.OpenBracket);

            for (int i = 0; i < node.Commas.GetValueOrDefault(0); i++)
            {
                _writer.WriteListSeparator();
            }

            _writer.WriteSyntax(Syntax.CloseBracket);

            _writer.PushBraceFormatting(_writer.Configuration.BracesLayout.ArrayAndObjectInitializer, false);

            node.Initializer.Accept(this);

            _writer.PopBraceFormatting();

            if (_writer.Configuration.Other.AlignMultiLineConstructs.ArrayObjectCollectionInitializer)
                _writer.SetAlignmentBreak(false);

            ExpressionEnd(node);
        }

        public void VisitIndexerDeclaration(IndexerDeclarationSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            BasePropertyDeclarationProlog(node);

            _writer.WriteKeyword(PrinterKeyword.This);
            node.ParameterList.Accept(this);

            node.AccessorList.Accept(this);

            WriteTrailingTrivia(node);
        }

        public void VisitInitializerExpression(InitializerExpressionSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            ExpressionStart(node);

            if (node.Expressions.Count == 0)
            {
                _writer.EmptyBlock();
            }
            else
            {
                _writer.BeginBlock();

                var wrapStyle = _writer.Configuration.LineBreaksAndWrapping.LineWrapping.WrapObjectAndCollectionInitializers;

                for (int i = 0; i < node.Expressions.Count; i++)
                {
                    _writer.WriteIndent();

                    node.Expressions[i].Accept(this);

                    if (i != node.Expressions.Count - 1)
                    {
                        _writer.WriteSyntax(Syntax.Comma);

                        if (wrapStyle != Configuration.WrapStyle.SimpleWrap)
                            _writer.Break(wrapStyle == Configuration.WrapStyle.ChopAlways);

                        _writer.WriteSpace();
                    }
                    else
                    {
                        _writer.WriteLine(true);
                    }
                }

                _writer.EndBlock();
            }

            ExpressionEnd(node);
        }

        public void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            VisitTypeDeclaration(node);
        }

        public void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            ExpressionStart(node);

            node.Expression.Accept(this);

            _writer.PushWrapStyle(_writer.Configuration.LineBreaksAndWrapping.LineWrapping.WrapInvocationArguments);

            node.ArgumentList.Accept(this);

            _writer.PopWrapStyle();

            ExpressionEnd(node);
        }

        public void VisitLabeledStatement(LabeledStatementSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            _writer.PushIndent(-_writer.Configuration.Indentation);

            _writer.WriteIndent();
            _writer.WriteIdentifier(node.Identifier);
            _writer.WriteSyntax(Syntax.Colon);
            _writer.WriteLine();

            _writer.PopIndent();

            WriteTrailingTrivia(node);

            node.Statement.Accept(this);
        }

        public void VisitLiteralExpression(LiteralExpressionSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            ExpressionStart(node);

            _writer.WriteLiteral(node.Value);

            ExpressionEnd(node);
        }

        public void VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            _writer.WriteIndent();

            if ((node.Modifiers & Modifiers.Const) != 0)
            {
                _writer.WriteKeyword(PrinterKeyword.Const);
                _writer.WriteSpace();
            }

            node.Declaration.Accept(this);

            _writer.EndStatement();

            WriteTrailingTrivia(node);
        }

        public void VisitLockStatement(LockStatementSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            _writer.WriteIndent();
            _writer.WriteKeyword(PrinterKeyword.Lock);

            if (_writer.Configuration.Spaces.BeforeParentheses.LockParentheses)
                _writer.WriteSpace();
            
            _writer.WriteSyntax(Syntax.OpenParen);

            if (_writer.Configuration.Spaces.WithinParentheses.LockParentheses)
                _writer.WriteSpace();

            node.Expression.Accept(this);

            if (_writer.Configuration.Spaces.WithinParentheses.LockParentheses)
                _writer.WriteSpace(); 
            
            _writer.WriteSyntax(Syntax.CloseParen);

            VisitBlockStatement(node.Statement);

            WriteTrailingTrivia(node);
        }

        public void VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            ExpressionStart(node);

            node.Expression.Accept(this);

            _writer.WriteSyntax(Syntax.Dot, _writer.Configuration.LineBreaksAndWrapping.LineWrapping.WrapChainedMethodCalls);

            node.Name.Accept(this);

            ExpressionEnd(node);
        }

        public void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            _writer.WriteIndent();

            WriteAttributes(
                node,
                _writer.Configuration.LineBreaksAndWrapping.Other.PlaceMethodAttributeOnSameLine
            );

            WriteMemberModifiers(node.Modifiers);

            node.ReturnType.Accept(this);
            _writer.WriteSpace();

            _writer.WriteIdentifier(node.Identifier);

            if (node.TypeParameterList != null)
                node.TypeParameterList.Accept(this);

            _writer.PushWrapStyle(_writer.Configuration.LineBreaksAndWrapping.LineWrapping.WrapFormalParameters);

            node.ParameterList.Accept(this);

            _writer.PopWrapStyle();

            if (node.ConstraintClauses.Count > 0)
                WriteConstraintClauses(node.ConstraintClauses);

            _writer.PushBraceFormatting(_writer.Configuration.BracesLayout.MethodDeclaration);

            if (node.Body != null)
            {
                bool isSimple =
                    _writer.Configuration.LineBreaksAndWrapping.Other.PlaceSimpleMethodOnSingleLine &&
                    IsSimpleBody(node.Body);

                if (isSimple)
                    _writer.PushSingleLineBody(true);

                node.Body.Accept(this);

                if (isSimple)
                {
                    _writer.PopSingleLineBody();
                    _writer.WriteLine();
                }
            }
            else
            {
                _writer.EndStatement();
            }

            _writer.PopBraceFormatting();

            WriteTrailingTrivia(node);
        }

        public void VisitNameColon(NameColonSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            node.Name.Accept(this);
            _writer.WriteSyntax(Syntax.Colon);
            _writer.WriteSpace();
        }

        public void VisitNameEquals(NameEqualsSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            node.Name.Accept(this);

            bool writeSpace;

            if (node.Parent is UsingDirectiveSyntax)
                writeSpace = _writer.Configuration.Spaces.Other.AroundEqualsInNamespaceAliasDirective;
            else
                writeSpace = true;

            if (writeSpace)
                _writer.WriteSpace();

            _writer.WriteOperator(PrinterOperator.Equals);

            if (writeSpace)
                _writer.WriteSpace();
        }

        public void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            _writer.WriteIndent();
            _writer.WriteKeyword(PrinterKeyword.Namespace);
            _writer.WriteSpace();
            node.Name.Accept(this);

            _writer.PushBraceFormatting(_writer.Configuration.BracesLayout.TypeAndNamespaceDeclaration);

            if (!node.ChildNodes().Any())
            {
                _writer.EmptyBlock(_writer.Configuration.BlankLines.InsideNamespace);
            }
            else
            {
                _writer.BeginBlock();

                _writer.WriteLine(_writer.Configuration.BlankLines.InsideNamespace);

                WriteGlobalNodes(
                    node.Usings,
                    node.Externs,
                    node.Members,
                    null
                );

                _writer.WriteLine(_writer.Configuration.BlankLines.InsideNamespace);

                _writer.EndBlock();
            }

            _writer.PopBraceFormatting();

            WriteTrailingTrivia(node);
        }

        public void VisitNullableType(NullableTypeSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            ExpressionStart(node);

            node.ElementType.Accept(this);

            if (_writer.Configuration.Spaces.Other.BeforeNullableMark)
                _writer.WriteSpace();

            _writer.WriteSyntax(Syntax.Question);

            ExpressionEnd(node);
        }

        public void VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            ExpressionStart(node);

            if (_writer.Configuration.Other.AlignMultiLineConstructs.ArrayObjectCollectionInitializer)
                _writer.SetAlignmentBreak(true);

            _writer.WriteKeyword(PrinterKeyword.New);
            _writer.WriteSpace();
            node.Type.Accept(this);

            if (node.ArgumentList != null)
                node.ArgumentList.Accept(this);

            if (node.Initializer != null)
            {
                _writer.PushBraceFormatting(_writer.Configuration.BracesLayout.ArrayAndObjectInitializer, false);

                node.Initializer.Accept(this);

                _writer.PopBraceFormatting();

                if (_writer.Configuration.Other.AlignMultiLineConstructs.ArrayObjectCollectionInitializer)
                    _writer.SetAlignmentBreak(false);
            }

            ExpressionEnd(node);
        }

        public void VisitOmittedArraySizeExpression(OmittedArraySizeExpressionSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            ExpressionStart(node);

            // We don't write anything for omitted.

            ExpressionEnd(node);
        }

        public void VisitOmittedTypeArgument(OmittedTypeArgumentSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            ExpressionStart(node);

            // We don't write anything for omitted.

            ExpressionEnd(node);
        }

        public void VisitOperatorDeclaration(OperatorDeclarationSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            _writer.WriteIndent();

            WriteAttributes(
                node,
                _writer.Configuration.LineBreaksAndWrapping.Other.PlaceMethodAttributeOnSameLine
            );

            WriteMemberModifiers(node.Modifiers);

            _writer.WriteKeyword(PrinterKeyword.Operator);
            _writer.WriteSpace();

            node.ReturnType.Accept(this);
            _writer.WriteSpace();

            switch (node.Operator)
            {
                case Operator.Ampersand: _writer.WriteOperator(PrinterOperator.Ampersand); break;
                case Operator.Asterisk: _writer.WriteOperator(PrinterOperator.Asterisk); break;
                case Operator.Bar: _writer.WriteOperator(PrinterOperator.Bar); break;
                case Operator.Caret: _writer.WriteOperator(PrinterOperator.Caret); break;
                case Operator.EqualsEquals: _writer.WriteOperator(PrinterOperator.EqualsEquals); break;
                case Operator.Exclamation: _writer.WriteOperator(PrinterOperator.Exclamation); break;
                case Operator.ExclamationEquals: _writer.WriteOperator(PrinterOperator.ExclamationEquals); break;
                case Operator.False: _writer.WriteKeyword(PrinterKeyword.False); break;
                case Operator.GreaterThan: _writer.WriteOperator(PrinterOperator.GreaterThan); break;
                case Operator.GreaterThanEquals: _writer.WriteOperator(PrinterOperator.GreaterThanEquals); break;
                case Operator.GreaterThanGreaterThan: _writer.WriteOperator(PrinterOperator.GreaterThanGreaterThan); break;
                case Operator.LessThan: _writer.WriteOperator(PrinterOperator.LessThan); break;
                case Operator.LessThanEquals: _writer.WriteOperator(PrinterOperator.LessThanEquals); break;
                case Operator.LessThanLessThan: _writer.WriteOperator(PrinterOperator.LessThanLessThan); break;
                case Operator.Minus: _writer.WriteOperator(PrinterOperator.Minus); break;
                case Operator.MinusMinus: _writer.WriteOperator(PrinterOperator.MinusMinus); break;
                case Operator.Percent: _writer.WriteOperator(PrinterOperator.Percent); break;
                case Operator.Plus: _writer.WriteOperator(PrinterOperator.Plus); break;
                case Operator.PlusPlus: _writer.WriteOperator(PrinterOperator.PlusPlus); break;
                case Operator.Slash: _writer.WriteOperator(PrinterOperator.Slash); break;
                case Operator.Tilde: _writer.WriteOperator(PrinterOperator.Tilde); break;
                case Operator.True: _writer.WriteKeyword(PrinterKeyword.True); break;
                default: throw ThrowHelper.InvalidEnumValue(node.Operator);
            }

            node.ParameterList.Accept(this);

            node.Body.Accept(this);

            WriteTrailingTrivia(node);
        }

        public void VisitParameterList(ParameterListSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            if (node.Parameters.Count == 0)
            {
                if (_writer.Configuration.Spaces.BeforeParentheses.MethodDeclarationEmptyParentheses)
                    _writer.WriteSpace();
            }
            else
            {
                if (_writer.Configuration.Spaces.BeforeParentheses.MethodDeclarationParentheses)
                    _writer.WriteSpace();
            }

            _writer.WriteSyntax(Syntax.OpenParen);

            if (_writer.Configuration.Other.AlignMultiLineConstructs.MethodParameters)
                _writer.SetAlignmentBreak(true);

            bool spacesWithin = false;

            if (node.Parent != null)
            {
                switch (node.Parent.SyntaxKind)
                {
                    case SyntaxKind.MethodDeclaration:
                        spacesWithin =
                            node.Parameters.Count > 0
                            ? _writer.Configuration.Spaces.WithinParentheses.MethodDeclarationParentheses
                            : _writer.Configuration.Spaces.WithinParentheses.MethodDeclarationEmptyParentheses;
                        break;
                }
            }

            if (spacesWithin)
                _writer.WriteSpace();

            bool hadOne = false;

            foreach (var parameter in node.Parameters)
            {
                if (hadOne)
                    _writer.WriteListSeparator();
                else
                    hadOne = true;

                parameter.Accept(this);
            }

            if (spacesWithin && node.Parameters.Count > 0)
                _writer.WriteSpace();

            if (_writer.Configuration.Other.AlignMultiLineConstructs.MethodParameters)
                _writer.SetAlignmentBreak(false);

            _writer.WriteSyntax(Syntax.CloseParen);
        }

        public void VisitParameter(ParameterSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            switch (node.Modifier)
            {
                case ParameterModifier.Ref:
                    _writer.WriteKeyword(PrinterKeyword.Ref);
                    _writer.WriteSpace();
                    break;

                case ParameterModifier.Out:
                    _writer.WriteKeyword(PrinterKeyword.Out);
                    _writer.WriteSpace();
                    break;
            }

            if (node.Type != null)
            {
                node.Type.Accept(this);
                _writer.WriteSpace();
            }

            _writer.WriteIdentifier(node.Identifier);

            if (node.Default != null)
                node.Default.Accept(this);
        }

        public void VisitParenthesizedExpression(ParenthesizedExpressionSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            ExpressionStart(node);

            _writer.WriteSyntax(Syntax.OpenParen);

            if (_writer.Configuration.Spaces.WithinParentheses.Parentheses)
                _writer.WriteSpace();

            node.Expression.Accept(this);

            if (_writer.Configuration.Spaces.WithinParentheses.Parentheses)
                _writer.WriteSpace();

            _writer.WriteSyntax(Syntax.CloseParen);

            ExpressionEnd(node);
        }

        public void VisitParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            ExpressionStart(node);

            if (node.Modifiers != Modifiers.None)
            {
                _writer.WriteModifiers(node.Modifiers);
                _writer.WriteSpace();
            }

            node.ParameterList.Accept(this);

            if (_writer.Configuration.Spaces.Other.AroundLambdaArrow)
                _writer.WriteSpace();

            _writer.WriteSyntax(Syntax.Arrow);

            if (
                _writer.Configuration.Spaces.Other.AroundLambdaArrow &&
                !(node.Body is BlockSyntax)
            )
                _writer.WriteSpace();

            _writer.PushBraceFormatting(_writer.Configuration.BracesLayout.Other, false);

            node.Body.Accept(this);

            _writer.PopBraceFormatting();

            ExpressionEnd(node);
        }

        public void VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            ExpressionStart(node);

            node.Operand.Accept(this);

            switch (node.Operator)
            {
                case PostfixUnaryOperator.MinusMinus: _writer.WriteOperator(PrinterOperator.MinusMinus); break;
                case PostfixUnaryOperator.PlusPlus: _writer.WriteOperator(PrinterOperator.PlusPlus); break;
                default: throw ThrowHelper.InvalidEnumValue(node.Operator);
            }

            ExpressionEnd(node);
        }

        public void VisitPredefinedType(PredefinedTypeSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            ExpressionStart(node);

            switch (node.Type)
            {
                case PredefinedType.Bool: _writer.WriteKeyword(PrinterKeyword.Bool); break;
                case PredefinedType.Byte: _writer.WriteKeyword(PrinterKeyword.Byte); break;
                case PredefinedType.Char: _writer.WriteKeyword(PrinterKeyword.Char); break;
                case PredefinedType.Decimal: _writer.WriteKeyword(PrinterKeyword.Decimal); break;
                case PredefinedType.Double: _writer.WriteKeyword(PrinterKeyword.Double); break;
                case PredefinedType.Float: _writer.WriteKeyword(PrinterKeyword.Float); break;
                case PredefinedType.Int: _writer.WriteKeyword(PrinterKeyword.Int); break;
                case PredefinedType.Long: _writer.WriteKeyword(PrinterKeyword.Long); break;
                case PredefinedType.Object: _writer.WriteKeyword(PrinterKeyword.Object); break;
                case PredefinedType.SByte: _writer.WriteKeyword(PrinterKeyword.SByte); break;
                case PredefinedType.Short: _writer.WriteKeyword(PrinterKeyword.Short); break;
                case PredefinedType.String: _writer.WriteKeyword(PrinterKeyword.String); break;
                case PredefinedType.UInt: _writer.WriteKeyword(PrinterKeyword.UInt); break;
                case PredefinedType.ULong: _writer.WriteKeyword(PrinterKeyword.ULong); break;
                case PredefinedType.UShort: _writer.WriteKeyword(PrinterKeyword.UShort); break;
                case PredefinedType.Void: _writer.WriteKeyword(PrinterKeyword.Void); break;
                default: throw ThrowHelper.InvalidEnumValue(node.Type);
            }

            ExpressionEnd(node);
        }

        public void VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            ExpressionStart(node);

            switch (node.Operator)
            {
                case PrefixUnaryOperator.Ampersand: _writer.WriteOperator(PrinterOperator.Ampersand); break;
                case PrefixUnaryOperator.Asterisk: _writer.WriteOperator(PrinterOperator.Asterisk); break;
                case PrefixUnaryOperator.Exclamation: _writer.WriteOperator(PrinterOperator.Exclamation); break;
                case PrefixUnaryOperator.Minus: _writer.WriteOperator(PrinterOperator.Minus); break;
                case PrefixUnaryOperator.MinusMinus: _writer.WriteOperator(PrinterOperator.MinusMinus); break;
                case PrefixUnaryOperator.Plus: _writer.WriteOperator(PrinterOperator.Plus); break;
                case PrefixUnaryOperator.PlusPlus: _writer.WriteOperator(PrinterOperator.PlusPlus); break;
                case PrefixUnaryOperator.Tilde: _writer.WriteOperator(PrinterOperator.Tilde); break;
                default: throw ThrowHelper.InvalidEnumValue(node.Operator);
            }

            node.Operand.Accept(this);

            ExpressionEnd(node);
        }

        public void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            BasePropertyDeclarationProlog(node);

            _writer.WriteIdentifier(node.Identifier);

            node.AccessorList.Accept(this);

            WriteTrailingTrivia(node);
        }

        private void BasePropertyDeclarationProlog(BasePropertyDeclarationSyntax node)
        {
            _writer.WriteIndent();

            WriteAttributes(
                node,
                _writer.Configuration.LineBreaksAndWrapping.Other.PlacePropertyIndexerEventAttributeOnSameLine
            );

            WriteMemberModifiers(node.Modifiers);

            node.Type.Accept(this);
            _writer.WriteSpace();

            if (node.ExplicitInterfaceSpecifier != null)
                node.ExplicitInterfaceSpecifier.Accept(this);
        }

        public void VisitQualifiedName(QualifiedNameSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            ExpressionStart(node);

            node.Left.Accept(this);
            _writer.WriteSyntax(Syntax.Dot);
            node.Right.Accept(this);

            ExpressionEnd(node);
        }

        public void VisitReturnStatement(ReturnStatementSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            _writer.WriteIndent();
            _writer.WriteKeyword(PrinterKeyword.Return);

            if (node.Expression != null)
            {
                _writer.WriteSpace();
                node.Expression.Accept(this);
            }

            _writer.EndStatement();

            WriteTrailingTrivia(node);
        }

        public void VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            ExpressionStart(node);

            if (node.Modifiers != Modifiers.None)
            {
                _writer.WriteModifiers(node.Modifiers);
                _writer.WriteSpace();
            }

            node.Parameter.Accept(this);

            if (_writer.Configuration.Spaces.Other.AroundLambdaArrow)
                _writer.WriteSpace();

            _writer.WriteSyntax(Syntax.Arrow);

            if (
                _writer.Configuration.Spaces.Other.AroundLambdaArrow &&
                !(node.Body is BlockSyntax)
            )
                _writer.WriteSpace();

            _writer.PushBraceFormatting(_writer.Configuration.BracesLayout.Other, false);

            node.Body.Accept(this);

            _writer.PopBraceFormatting();

            ExpressionEnd(node);
        }

        public void VisitSizeOfExpression(SizeOfExpressionSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            ExpressionStart(node);

            _writer.WriteKeyword(PrinterKeyword.SizeOf);

            if (_writer.Configuration.Spaces.BeforeParentheses.SizeOfParentheses)
                _writer.WriteSpace();

            _writer.WriteSyntax(Syntax.OpenParen);

            if (_writer.Configuration.Spaces.WithinParentheses.SizeOfParentheses)
                _writer.WriteSpace();

            node.Type.Accept(this);

            if (_writer.Configuration.Spaces.WithinParentheses.SizeOfParentheses)
                _writer.WriteSpace();

            _writer.WriteSyntax(Syntax.CloseParen);

            ExpressionEnd(node);
        }

        public void VisitStructDeclaration(StructDeclarationSyntax node)
        {
            VisitTypeDeclaration(node);
        }

        public void VisitSwitchLabel(SwitchLabelSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            _writer.WriteIndent();

            switch (node.Kind)
            {
                case CaseOrDefault.Default:
                    _writer.WriteKeyword(PrinterKeyword.Default);
                    break;

                case CaseOrDefault.Case:
                    _writer.WriteKeyword(PrinterKeyword.Case);
                    _writer.WriteSpace();
                    node.Value.Accept(this);
                    break;
            }

            if (_writer.Configuration.Spaces.Other.BeforeColonInCaseStatement)
                _writer.WriteSpace();

            _writer.WriteSyntax(Syntax.Colon);
        }

        public void VisitSwitchSection(SwitchSectionSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            bool hadOne = false;

            foreach (var label in node.Labels)
            {
                if (hadOne)
                    _writer.WriteLine();
                else
                    hadOne = true;

                label.Accept(this);
            }

            _writer.PushBraceFormatting(_writer.Configuration.BracesLayout.BlockUnderCaseLabel, false);

            foreach (var statement in node.Statements)
            {
                VisitBlockStatement(statement);
            }

            _writer.PopBraceFormatting();

            WriteTrailingTrivia(node);
        }

        public void VisitSwitchStatement(SwitchStatementSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            _writer.WriteIndent();
            _writer.WriteKeyword(PrinterKeyword.Switch);

            if (_writer.Configuration.Spaces.BeforeParentheses.SwitchParentheses)
                _writer.WriteSpace();

            _writer.WriteSyntax(Syntax.OpenParen);

            if (_writer.Configuration.Spaces.WithinParentheses.SwitchParentheses)
                _writer.WriteSpace();
            
            node.Expression.Accept(this);

            if (_writer.Configuration.Spaces.WithinParentheses.SwitchParentheses)
                _writer.WriteSpace();
            
            _writer.WriteSyntax(Syntax.CloseParen);

            if (node.Sections.Count == 0)
            {
                _writer.EmptyBlock();
            }
            else
            {
                _writer.BeginBlock(
                    true,
                    true,
                    !_writer.Configuration.Other.Other.IndentCaseFromSwitch
                );

                bool hadOne = false;

                foreach (var section in node.Sections)
                {
                    if (hadOne)
                        _writer.WriteLine();
                    else
                        hadOne = true;

                    section.Accept(this);
                    _writer.WriteLine();
                }

                _writer.EndBlock(
                    true,
                    !_writer.Configuration.Other.Other.IndentCaseFromSwitch
                );
            }

            WriteTrailingTrivia(node);
        }

        public void VisitThisExpression(ThisExpressionSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            ExpressionStart(node);

            _writer.WriteKeyword(PrinterKeyword.This);

            ExpressionEnd(node);
        }

        public void VisitThrowStatement(ThrowStatementSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            _writer.WriteIndent();
            _writer.WriteKeyword(PrinterKeyword.Throw);

            if (node.Expression != null)
            {
                _writer.WriteSpace();
                node.Expression.Accept(this);
            }

            _writer.EndStatement();

            WriteTrailingTrivia(node);
        }

        public void VisitTryStatement(TryStatementSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            _writer.WriteIndent();
            _writer.WriteKeyword(PrinterKeyword.Try);

            if (node.Catches.Count > 0)
            {
                if (!_writer.Configuration.LineBreaksAndWrapping.PlaceOnNewLine.PlaceCatchOnNewLine)
                    _writer.PushBraceFormatting(_writer.Configuration.BracesLayout.Other, false);
            }
            else if (node.Finally != null)
            {
                if (!_writer.Configuration.LineBreaksAndWrapping.PlaceOnNewLine.PlaceFinallyOnNewLine)
                    _writer.PushBraceFormatting(_writer.Configuration.BracesLayout.Other, false);
            }

            node.Block.Accept(this);

            for (int i = 0; i < node.Catches.Count; i++)
            {
                if (i == node.Catches.Count - 1)
                {
                    if (!_writer.Configuration.LineBreaksAndWrapping.PlaceOnNewLine.PlaceCatchOnNewLine)
                        _writer.PopBraceFormatting();
                    if (
                        node.Finally != null &&
                        !_writer.Configuration.LineBreaksAndWrapping.PlaceOnNewLine.PlaceFinallyOnNewLine
                    )
                        _writer.PushBraceFormatting(_writer.Configuration.BracesLayout.Other, false);
                }

                node.Catches[i].Accept(this);
            }

            if (node.Finally != null)
            {
                if (!_writer.Configuration.LineBreaksAndWrapping.PlaceOnNewLine.PlaceFinallyOnNewLine)
                    _writer.PopBraceFormatting();

                node.Finally.Accept(this);
            }

            WriteTrailingTrivia(node);
        }

        public void VisitTypeArgumentList(TypeArgumentListSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            if (_writer.Configuration.Spaces.BeforeParentheses.BeforeTypeArgumentListAngle)
                _writer.WriteSpace();

            _writer.WriteSyntax(Syntax.LessThan);

            if (_writer.Configuration.Spaces.WithinParentheses.TypeArgumentAngles)
                _writer.WriteSpace();

            bool hadOne = false;

            foreach (var argument in node.Arguments)
            {
                if (hadOne)
                    _writer.WriteListSeparator();
                else
                    hadOne = true;

                argument.Accept(this);
            }

            if (_writer.Configuration.Spaces.WithinParentheses.TypeArgumentAngles)
                _writer.WriteSpace();

            _writer.WriteSyntax(Syntax.GreaterThan);
        }

        public void VisitTypeConstraint(TypeConstraintSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            node.Type.Accept(this);
        }

        public void VisitTypeOfExpression(TypeOfExpressionSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            ExpressionStart(node);

            _writer.WriteKeyword(PrinterKeyword.TypeOf);

            if (_writer.Configuration.Spaces.BeforeParentheses.TypeOfParentheses)
                _writer.WriteSpace();

            _writer.WriteSyntax(Syntax.OpenParen);

            if (_writer.Configuration.Spaces.WithinParentheses.TypeOfParentheses)
                _writer.WriteSpace();

            node.Type.Accept(this);

            if (_writer.Configuration.Spaces.WithinParentheses.TypeOfParentheses)
                _writer.WriteSpace();

            _writer.WriteSyntax(Syntax.CloseParen);

            ExpressionEnd(node);
        }

        public void VisitTypeParameterConstraintClause(TypeParameterConstraintClauseSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            _writer.WriteKeyword(PrinterKeyword.Where);
            _writer.WriteSpace();

            node.Name.Accept(this);

            if (_writer.Configuration.Spaces.Other.BeforeTypeParameterConstraintColon)
                _writer.WriteSpace();

            _writer.WriteSyntax(Syntax.Colon);

            if (_writer.Configuration.Spaces.Other.AfterTypeParameterConstraintColon)
                _writer.WriteSpace();

            bool hadOne = false;

            foreach (var constraint in node.Constraints)
            {
                if (hadOne)
                    _writer.WriteListSeparator();
                else
                    hadOne = true;

                constraint.Accept(this);
            }
        }

        public void VisitTypeParameterList(TypeParameterListSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            if (_writer.Configuration.Spaces.BeforeParentheses.BeforeTypeParameterListAngle)
                _writer.WriteSpace();

            _writer.WriteSyntax(Syntax.LessThan);

            if (_writer.Configuration.Spaces.WithinParentheses.TypeParameterAngles)
                _writer.WriteSpace();

            bool hadOne = false;

            foreach (var parameter in node.Parameters)
            {
                if (hadOne)
                    _writer.WriteListSeparator();
                else
                    hadOne = true;

                parameter.Accept(this);
            }

            if (_writer.Configuration.Spaces.WithinParentheses.TypeParameterAngles)
                _writer.WriteSpace();

            _writer.WriteSyntax(Syntax.GreaterThan);
        }

        public void VisitTypeParameter(TypeParameterSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteAttributes(node, true);

            switch (node.Variance)
            {
                case Variance.In:
                    _writer.WriteKeyword(PrinterKeyword.In);
                    _writer.WriteSpace();
                    break;

                case Variance.Out:
                    _writer.WriteKeyword(PrinterKeyword.Out);
                    _writer.WriteSpace();
                    break;
            }

            _writer.WriteIdentifier(node.Identifier);
        }

        public void VisitUsingDirective(UsingDirectiveSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            _writer.WriteIndent();
            _writer.WriteKeyword(PrinterKeyword.Using);
            _writer.WriteSpace();

            if (node.Alias != null)
                node.Alias.Accept(this);

            node.Name.Accept(this);
            _writer.EndStatement();

            WriteTrailingTrivia(node);
        }

        public void VisitUsingStatement(UsingStatementSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            _writer.WriteIndent();
            _writer.WriteKeyword(PrinterKeyword.Using);

            if (_writer.Configuration.Spaces.BeforeParentheses.UsingParentheses)
                _writer.WriteSpace();

            _writer.WriteSyntax(Syntax.OpenParen);

            if (_writer.Configuration.Spaces.WithinParentheses.UsingParentheses)
                _writer.WriteSpace();

            if (node.Expression != null)
                node.Expression.Accept(this);
            if (node.Declaration != null)
                node.Declaration.Accept(this);

            if (_writer.Configuration.Spaces.WithinParentheses.UsingParentheses)
                _writer.WriteSpace();

            _writer.WriteSyntax(Syntax.CloseParen);

            if (
                _writer.Configuration.Other.Other.IndentNestedUsingStatements &&
                node.Statement is UsingStatementSyntax
            ) {
                _writer.WriteLine();
                node.Statement.Accept(this);
            }
            else
            {
                VisitBlockStatement(node.Statement);
            }

            WriteTrailingTrivia(node);
        }

        public void VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            node.Type.Accept(this);

            _writer.WriteSpace();

            if (_writer.Configuration.Other.AlignMultiLineConstructs.MultipleDeclarations)
                _writer.SetAlignmentBreak(true);

            _writer.PushWrapStyle(_writer.Configuration.LineBreaksAndWrapping.LineWrapping.WrapMultipleDeclarations);

            bool hadOne = false;

            foreach (var variable in node.Variables)
            {
                if (hadOne)
                    _writer.WriteListSeparator();
                else
                    hadOne = true;

                variable.Accept(this);
            }

            _writer.PopWrapStyle();

            if (_writer.Configuration.Other.AlignMultiLineConstructs.MultipleDeclarations)
                _writer.SetAlignmentBreak(false);
        }

        public void VisitVariableDeclarator(VariableDeclaratorSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            _writer.WriteIdentifier(node.Identifier);

            if (node.Initializer != null)
                node.Initializer.Accept(this);
        }

        public void VisitWhileStatement(WhileStatementSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            _writer.WriteIndent();
            _writer.WriteKeyword(PrinterKeyword.While);

            if (_writer.Configuration.Spaces.BeforeParentheses.WhileParentheses)
                _writer.WriteSpace();

            _writer.WriteSyntax(Syntax.OpenParen);

            if (_writer.Configuration.Spaces.WithinParentheses.WhileParentheses)
                _writer.WriteSpace();

            _wrapCompound = _writer.Configuration.LineBreaksAndWrapping.LineWrapping.ForceChopCompoundConditionInWhileStatement;

            node.Condition.Accept(this);

            _wrapCompound = false;

            if (_writer.Configuration.Spaces.WithinParentheses.WhileParentheses)
                _writer.WriteSpace();

            _writer.WriteSyntax(Syntax.CloseParen);

            VisitBlockStatement(node.Statement);

            WriteTrailingTrivia(node);
        }

        public void VisitYieldStatement(YieldStatementSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            WriteLeadingTrivia(node);

            _writer.WriteIndent();
            _writer.WriteKeyword(PrinterKeyword.Yield);
            _writer.WriteSpace();

            switch (node.Kind)
            {
                case ReturnOrBreak.Break: _writer.WriteKeyword(PrinterKeyword.Break); break;
                case ReturnOrBreak.Return: _writer.WriteKeyword(PrinterKeyword.Return); break;
                default: throw ThrowHelper.InvalidEnumValue(node.Kind);
            }

            if (node.Expression != null)
            {
                _writer.WriteSpace();
                node.Expression.Accept(this);
            }

            _writer.EndStatement();

            WriteTrailingTrivia(node);
        }

        public void VisitAwaitExpression(AwaitExpressionSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            ExpressionStart(node);

            _writer.WriteKeyword(PrinterKeyword.Await);
            _writer.WriteSpace();
            node.Expression.Accept(this);

            ExpressionEnd(node);
        }

        private void WriteLeadingTrivia(SyntaxTriviaNode node)
        {
            foreach (var trivia in node.LeadingTrivia)
            {
                WriteTrivia(trivia);
            }
        }

        private void WriteAttributes(IHasAttributeLists node, bool attributesOnSingleLine)
        {
            foreach (var attribute in node.AttributeLists)
            {
                attribute.Accept(this);

                if (attributesOnSingleLine)
                {
                    _writer.WriteSpace();
                }
                else
                {
                    _writer.WriteLine();
                    _writer.WriteIndent();
                }
            }
        }

        private void WriteTrailingTrivia(SyntaxTriviaNode node)
        {
            foreach (var trivia in node.TrailingTrivia)
            {
                WriteTrivia(trivia);
            }
        }

        private void WriteTrivia(SyntaxTrivia trivia)
        {
            if (trivia.Kind == SyntaxTriviaKind.NewLine)
            {
                _writer.WriteLine();
                return;
            }

            switch (trivia.Kind)
            {
                case SyntaxTriviaKind.BlockComment:
                    var lines = (trivia.Text ?? String.Empty).Split('\n');

                    for (int i = 0; i < lines.Length; i++)
                    {
                        string line = lines[i].TrimEnd();

                        if (i == 0)
                        {
                            if (line.Length > 0)
                                line = "/* " + line;
                            else
                                line = "/*";
                        }
                        if (i == lines.Length - 1)
                        {
                            if (line.Length > 0)
                                line += " */";
                            else
                                line = "*/";
                        }

                        _writer.WriteIndent();
                        _writer.WriteComment(line.TrimEnd());
                        _writer.WriteLine();
                    }
                    break;

                default:
                    var syntax =
                        trivia.Kind == SyntaxTriviaKind.Comment
                        ? "// "
                        : "/// ";

                    foreach (string line in (trivia.Text ?? String.Empty).Split('\n'))
                    {
                        _writer.WriteIndent();
                        _writer.WriteComment((syntax + line).TrimEnd());
                        _writer.WriteLine();
                    }
                    break;
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_writer != null)
                {
                    _writer.Flush();
                    _writer = null;
                }

                _disposed = true;
            }
        }
    }
}
