using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSyntax.Printer
{
    partial class SyntaxPrinter
    {
        public void VisitFromClause(FromClauseSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            if (
                !_writer.Configuration.LineBreaksAndWrapping.Other.PlaceLinqExpressionOnSingleLine &&
                node.Parent is QueryBodySyntax
            )
                _writer.Break(true);

            _writer.WriteKeyword(PrinterKeyword.From);
            _writer.WriteSpace();

            if (node.Type != null)
            {
                node.Type.Accept(this);
                _writer.WriteSpace();
            }

            _writer.WriteIdentifier(node.Identifier);
            _writer.WriteSpace();
            _writer.WriteKeyword(PrinterKeyword.In);
            _writer.WriteSpace();
            node.Expression.Accept(this);
        }

        public void VisitGroupClause(GroupClauseSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            if (!_writer.Configuration.LineBreaksAndWrapping.Other.PlaceLinqExpressionOnSingleLine)
                _writer.Break(true);

            _writer.WriteKeyword(PrinterKeyword.Group);
            _writer.WriteSpace();
            node.GroupExpression.Accept(this);
            _writer.WriteSpace();
            _writer.WriteKeyword(PrinterKeyword.By);
            _writer.WriteSpace();
            node.ByExpression.Accept(this);
        }

        public void VisitJoinClause(JoinClauseSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            if (!_writer.Configuration.LineBreaksAndWrapping.Other.PlaceLinqExpressionOnSingleLine)
                _writer.Break(true);

            _writer.WriteKeyword(PrinterKeyword.Join);
            _writer.WriteSpace();

            if (node.Type != null)
            {
                node.Type.Accept(this);
                _writer.WriteSpace();
            }

            _writer.WriteIdentifier(node.Identifier);
            _writer.WriteSpace();
            _writer.WriteKeyword(PrinterKeyword.In);
            _writer.WriteSpace();
            node.InExpression.Accept(this);
            _writer.WriteSpace();
            _writer.WriteKeyword(PrinterKeyword.On);
            _writer.WriteSpace();
            node.LeftExpression.Accept(this);
            _writer.WriteSpace();
            _writer.WriteKeyword(PrinterKeyword.Equals);
            _writer.WriteSpace();
            node.RightExpression.Accept(this);

            if (node.Into != null)
            {
                _writer.WriteSpace();
                node.Into.Accept(this);
            }
        }

        public void VisitJoinIntoClause(JoinIntoClauseSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            _writer.WriteKeyword(PrinterKeyword.Into);
            _writer.WriteSpace();
            _writer.WriteIdentifier(node.Identifier);
        }

        public void VisitLetClause(LetClauseSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            if (!_writer.Configuration.LineBreaksAndWrapping.Other.PlaceLinqExpressionOnSingleLine)
                _writer.Break(true);

            _writer.WriteKeyword(PrinterKeyword.Let);
            _writer.WriteSpace();
            _writer.WriteIdentifier(node.Identifier);
            _writer.WriteSpace();
            _writer.WriteOperator(PrinterOperator.Equals);
            _writer.WriteSpace();
            node.Expression.Accept(this);
        }

        public void VisitOrderByClause(OrderByClauseSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            if (!_writer.Configuration.LineBreaksAndWrapping.Other.PlaceLinqExpressionOnSingleLine)
                _writer.Break(true);

            _writer.WriteKeyword(PrinterKeyword.OrderBy);
            _writer.WriteSpace();

            bool hadOne = false;

            foreach (var ordering in node.Orderings)
            {
                if (hadOne)
                    _writer.WriteListSeparator();
                else
                    hadOne = true;

                ordering.Accept(this);
            }
        }

        public void VisitOrdering(OrderingSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            node.Expression.Accept(this);

            if (node.Kind != AscendingOrDescending.Ascending)
            {
                _writer.WriteSpace();
                _writer.WriteKeyword(PrinterKeyword.Descending);
            }
        }

        public void VisitQueryBody(QueryBodySyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            foreach (var clause in node.Clauses)
            {
                clause.Accept(this);
                _writer.WriteSpace();
            }

            node.SelectOrGroup.Accept(this);

            if (node.Continuation != null)
            {
                _writer.WriteSpace();
                node.Continuation.Accept(this);
            }
        }

        public void VisitQueryContinuation(QueryContinuationSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            if (node.Identifier != null)
            {
                _writer.WriteKeyword(PrinterKeyword.Into);
                _writer.WriteSpace();
                _writer.WriteIdentifier(node.Identifier);
                _writer.WriteSpace();
            }

            node.Body.Accept(this);
        }

        public void VisitQueryExpression(QueryExpressionSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            ExpressionStart(node);

            if (_writer.Configuration.Other.AlignMultiLineConstructs.LinqQuery)
                _writer.SetAlignmentBreak(true);

            node.FromClause.Accept(this);
            _writer.WriteSpace();
            node.Body.Accept(this);

            if (_writer.Configuration.Other.AlignMultiLineConstructs.LinqQuery)
                _writer.SetAlignmentBreak(false);

            ExpressionEnd(node);
        }

        public void VisitSelectClause(SelectClauseSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            if (!_writer.Configuration.LineBreaksAndWrapping.Other.PlaceLinqExpressionOnSingleLine)
                _writer.Break(true);

            _writer.WriteKeyword(PrinterKeyword.Select);
            _writer.WriteSpace();
            node.Expression.Accept(this);
        }

        public void VisitWhereClause(WhereClauseSyntax node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            node.Validate();

            if (!_writer.Configuration.LineBreaksAndWrapping.Other.PlaceLinqExpressionOnSingleLine)
                _writer.Break(true);

            _writer.WriteKeyword(PrinterKeyword.Where);
            _writer.WriteSpace();
            node.Condition.Accept(this);
        }
    }
}
