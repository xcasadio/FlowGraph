using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSyntax
{
    partial class VariableDeclaratorSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Identifier, "Identifier");
        }
    }

    partial class VariableDeclarationSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Type, "Type");
            ValidateNotEmpty(Variables, "Variables");
        }
    }

    partial class EqualsValueClauseSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Value, "Value");
        }
    }

    partial class BinaryExpressionSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Left, "Left");
            ValidateNotNull(Right, "Right");
        }
    }

    partial class PrefixUnaryExpressionSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Operand, "Operand");
        }
    }

    partial class PostfixUnaryExpressionSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Operand, "Operand");
        }
    }

    partial class PropertyDeclarationSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Identifier, "Identifier");
        }
    }

    partial class BasePropertyDeclarationSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(AccessorList, "AccessorList");
            ValidateNotNull(Type, "Type");

            if (ExplicitInterfaceSpecifier != null && Modifiers != Modifiers.None)
            {
                throw new CSharpSyntaxException(String.Format(
                    "Node '{0}' has both modifiers and an explicit interface specifier",
                    this
                ));
            }
        }
    }

    partial class AccessorListSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotEmpty(Accessors, "Accessors");

            bool valid = true;

            if (Accessors.Count == 2)
            {
                switch (Accessors[0].Kind)
                {
                    case AccessorDeclarationKind.Add: valid = Accessors[1].Kind == AccessorDeclarationKind.Remove; break;
                    case AccessorDeclarationKind.Remove: valid = Accessors[1].Kind == AccessorDeclarationKind.Add; break;
                    case AccessorDeclarationKind.Get: valid = Accessors[1].Kind == AccessorDeclarationKind.Set; break;
                    case AccessorDeclarationKind.Set: valid = Accessors[1].Kind == AccessorDeclarationKind.Get; break;
                    default: valid = false; break;
                }
            }
            else if (Accessors.Count > 2)
            {
                valid = false;
            }

            if (!valid)
            {
                throw new CSharpSyntaxException(String.Format(
                    "Node '{0}' has an invalid accessor combination",
                    this
                ));
            }
        }
    }

    partial class IdentifierNameSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Identifier, "Identifier");
        }
    }

    partial class GenericNameSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Identifier, "Identifier");
            ValidateNotNull(TypeArgumentList, "TypeArgumentList");

            if (IsUnboundGenericName)
            {
                if (!TypeArgumentList.Arguments.All(p => p is OmittedTypeArgumentSyntax))
                {
                    throw new CSharpSyntaxException(String.Format(
                        "Unbound generic name syntax '{0}' may only have omitted type arguments",
                        this
                    ));
                }
            }
            else
            {
                if (TypeArgumentList.Arguments.Any(p => p is OmittedTypeArgumentSyntax))
                {
                    throw new CSharpSyntaxException(String.Format(
                        "Bound generic name syntax '{0}' may not have omitted type arguments",
                        this
                    ));
                }
            }
        }
    }

    partial class QualifiedNameSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Left, "Left");
            ValidateNotNull(Right, "Right");
        }
    }

    partial class AliasQualifiedNameSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Alias, "Alias");
            ValidateNotNull(Name, "Name");
        }
    }

    partial class TypeArgumentListSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotEmpty(Arguments, "Arguments");
        }
    }

    partial class ArrayTypeSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(ElementType, "ElementType");
            ValidateNotEmpty(RankSpecifiers, "RankSpecifiers");
        }
    }

    partial class NullableTypeSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(ElementType, "ElementType");
        }
    }

    partial class ExplicitInterfaceSpecifierSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Name, "Name");
        }
    }

    partial class AnonymousMethodExpressionSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(ParameterList, "ParameterList");
            ValidateNotNull(Block, "Block");
            ValidateModifiers(Modifiers, Modifiers.Async);
        }
    }

    partial class ParameterSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Identifier, "Identifier");

            bool lambdaExpression =
                Parent is SimpleLambdaExpressionSyntax ||
                Parent != null && Parent.Parent is ParenthesizedLambdaExpressionSyntax;

            if (!lambdaExpression)
            {
                ValidateNotNull(Type, "Type");
            }
            else if (Modifier != ParameterModifier.None)
            {
                throw new CSharpSyntaxException(String.Format(
                    "Parameter '{0}' of an anonymous method cannot have a modifier",
                    this
                ));
            }
        }
    }

    partial class AnonymousObjectCreationExpressionSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotEmpty(Initializers, "Initializers");
        }
    }

    partial class AnonymousObjectMemberDeclaratorSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Expression, "Expression");
        }
    }

    partial class NameEqualsSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Name, "Name");
        }
    }

    partial class InvocationExpressionSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(ArgumentList, "ArgumentList");
            ValidateNotNull(Expression, "Expression");
        }
    }

    partial class MemberAccessExpressionSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Expression, "Expression");
            ValidateNotNull(Name, "Name");
        }
    }

    partial class ArgumentSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Expression, "Expression");
        }
    }

    partial class NameColonSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Name, "Name");
        }
    }

    partial class ArrayCreationExpressionSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Type, "Type");
        }
    }

    partial class BaseTypeDeclarationSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Identifier, "Identifier");
        }
    }

    partial class BaseListSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotEmpty(Types, "Types");
        }
    }

    partial class TypeParameterListSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotEmpty(Parameters, "Parameters");
        }
    }

    partial class TypeParameterSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Identifier, "Identifier");
        }
    }

    partial class TypeParameterConstraintClauseSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Name, "Name");
            ValidateNotEmpty(Constraints, "Constraints");
        }
    }

    partial class TypeConstraintSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Type, "Type");
        }
    }

    partial class AttributeListSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotEmpty(Attributes, "Attributes");
        }
    }

    partial class AttributeSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Name, "Name");
        }
    }

    partial class AttributeArgumentListSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotEmpty(Arguments, "Arguments");
        }
    }

    partial class AttributeArgumentSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Expression, "Expression");

            if (NameEquals != null && NameColon != null)
            {
                throw new CSharpSyntaxException(String.Format(
                    "Name colon and name equals cannot be specified both on attribute argument '{0}'",
                    this
                ));
            }
        }
    }

    partial class IndexerDeclarationSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(ParameterList, "ParameterList");
        }
    }

    partial class BracketedParameterListSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotEmpty(Parameters, "Parameters");
        }
    }

    partial class CastExpressionSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Expression, "Expression");
            ValidateNotNull(Type, "Type");
        }
    }

    partial class TryStatementSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Block, "Block");

            if (Finally == null)
                ValidateNotEmpty(Catches, "Catches");
        }
    }

    partial class CatchClauseSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Block, "Block");
        }
    }

    partial class CatchDeclarationSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Type, "Type");
        }
    }

    partial class FinallyClauseSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Block, "Block");
        }
    }

    partial class CheckedExpressionSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Expression, "Expression");
        }
    }

    partial class CheckedStatementSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Block, "Block");
        }
    }

    partial class UsingDirectiveSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Name, "Name");
        }
    }

    partial class ExternAliasDirectiveSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Identifier, "Identifier");
        }
    }

    partial class ConditionalExpressionSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Condition, "Condition");
            ValidateNotNull(WhenTrue, "WhenTrue");
            ValidateNotNull(WhenFalse, "WhenFalse");
        }
    }

    partial class BaseMethodDeclarationSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(ParameterList, "ParameterList");
        }
    }
    
    partial class ConstructorDeclarationSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Identifier, "Identifier");
            ValidateNotNull(Body, "Body");
        }
    }

    partial class DestructorDeclarationSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Identifier, "Identifier");
            ValidateNotNull(Body, "Body");

            if (Modifiers != Modifiers.None)
            {
                throw new CSharpSyntaxException(String.Format(
                    "Destructor declaration '{0}' cannot have any modifiers",
                    this
                ));
            }
            if (ParameterList.Parameters.Count > 0)
            {
                throw new CSharpSyntaxException(String.Format(
                    "Destructor declaration '{0}' cannot have a parameter list",
                    this
                ));
            }
        }
    }

    partial class ConversionOperatorDeclarationSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Type, "Type");
            ValidateNotNull(Body, "Body");
        }
    }

    partial class ConstructorInitializerSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(ArgumentList, "ArgumentList");
        }
    }

    partial class DefaultExpressionSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Type, "Type");
        }
    }

    partial class DelegateDeclarationSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Identifier, "Identifier");
            ValidateNotNull(ParameterList, "ParameterList");
            ValidateNotNull(ReturnType, "ReturnType");
        }
    }

    partial class DoStatementSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Condition, "Condition");
            ValidateNotNull(Statement, "Statement");
        }
    }

    partial class IfStatementSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Condition, "Condition");
            ValidateNotNull(Statement, "Statement");
        }
    }

    partial class ElseClauseSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Statement, "Statement");
        }
    }

    partial class ElementAccessExpressionSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Expression, "Expression");
            ValidateNotNull(ArgumentList, "ArgumentList");
        }
    }

    partial class BracketedArgumentListSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotEmpty(Arguments, "Arguments");
        }
    }

    partial class EnumMemberDeclarationSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Identifier, "Identifier");
        }
    }

    partial class EventDeclarationSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Identifier, "Identifier");
        }
    }

    partial class BaseFieldDeclarationSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Declaration, "Declaration");
        }
    }

    partial class ExpressionStatementSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Expression, "Expression");
        }
    }

    partial class ForEachStatementSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Identifier, "Identifier");
            ValidateNotNull(Expression, "Expression");
            ValidateNotNull(Statement, "Statement");
            ValidateNotNull(Type, "Type");
        }
    }

    partial class ForStatementSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Statement, "Statement");

            if (
                Declaration != null &&
                Initializers.Count > 0
            ) {
                throw new CSharpSyntaxException(String.Format(
                    "Cannot specify both a declaration and initializers for for node '{0}'",
                    this
                ));
            }
        }
    }

    partial class GotoStatementSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            if (Kind != CaseOrDefault.Default)
                ValidateNotNull(Expression, "Expression");
        }
    }

    partial class ImplicitArrayCreationExpressionSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Initializer, "Initializer");
        }
    }

    partial class LabeledStatementSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Identifier, "Identifier");
            ValidateNotNull(Statement, "Statement");
        }
    }

    partial class LocalDeclarationStatementSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Declaration, "Declaration");
        }
    }

    partial class LockStatementSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Expression, "Expression");
            ValidateNotNull(Statement, "Statement");
        }
    }

    partial class MethodDeclarationSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Identifier, "Identifier");
            ValidateNotNull(ReturnType, "ReturnType");
        }
    }

    partial class NamespaceDeclarationSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Name, "Name");
        }
    }

    partial class ObjectCreationExpressionSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Type, "Type");
            if (ArgumentList == null)
                ValidateNotNull(Initializer, "Initializer");
        }
    }

    partial class OperatorDeclarationSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(ReturnType, "ReturnType");
            ValidateNotNull(Body, "Body");
        }
    }

    partial class ParenthesizedExpressionSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Expression, "Expression");
        }
    }

    partial class SimpleLambdaExpressionSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Body, "Body");
            ValidateNotNull(Parameter, "Parameter");
            ValidateModifiers(Modifiers, Modifiers.Async);

            if (Parameter.Type != null)
            {
                throw new CSharpSyntaxException(String.Format(
                    "Simple lambda expression '{0}' cannot have a type on the parameter; use ParenthesizedLambdaExpression instead",
                    this
                ));
            }
        }
    }

    partial class ParenthesizedLambdaExpressionSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Body, "Body");
            ValidateNotNull(ParameterList, "ParameterList");
            ValidateModifiers(Modifiers, Modifiers.Async);
        }
    }

    partial class SizeOfExpressionSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Type, "Type");
        }
    }

    partial class SwitchStatementSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Expression, "Expression");
        }
    }

    partial class SwitchSectionSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotEmpty(Labels, "Labels");
            ValidateNotEmpty(Statements, "Statements");
        }
    }

    partial class SwitchLabelSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            if (Kind == CaseOrDefault.Case)
                ValidateNotNull(Value, "Value");
        }
    }

    partial class TypeOfExpressionSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Type, "Type");
        }
    }

    partial class UsingStatementSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            if ((Expression != null) == (Declaration != null))
            {
                throw new CSharpSyntaxException(String.Format(
                    "Specify either an expression or a declaration on using node '{0}'",
                    this
                ));
            }

            ValidateNotNull(Statement, "Statement");
        }
    }

    partial class WhileStatementSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Condition, "Condition");
            ValidateNotNull(Statement, "Statement");
        }
    }

    partial class YieldStatementSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            if (Kind == ReturnOrBreak.Return)
                ValidateNotNull(Expression, "Expression");
        }
    }

    partial class QueryExpressionSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(FromClause, "FromClause");
            ValidateNotNull(Body, "Body");
        }
    }

    partial class FromClauseSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Identifier, "Identifier");
            ValidateNotNull(Expression, "Expression");
        }
    }

    partial class QueryBodySyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(SelectOrGroup, "SelectOrGroup");
        }
    }

    partial class SelectClauseSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Expression, "Expression");
        }
    }

    partial class JoinClauseSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Identifier, "Identifier");
            ValidateNotNull(InExpression, "InExpression");
            ValidateNotNull(LeftExpression, "LeftExpression");
            ValidateNotNull(RightExpression, "RightExpression");
        }
    }

    partial class JoinIntoClauseSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Identifier, "Identifier");
        }
    }

    partial class LetClauseSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Expression, "Expression");
            ValidateNotNull(Identifier, "Identifier");
        }
    }

    partial class OrderByClauseSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotEmpty(Orderings, "Orderings");
        }
    }

    partial class OrderingSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Expression, "Expression");
        }
    }

    partial class WhereClauseSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Condition, "Condition");
        }
    }

    partial class GroupClauseSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(GroupExpression, "GroupExpression");
            ValidateNotNull(ByExpression, "ByExpression");
        }
    }

    partial class QueryContinuationSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Body, "Body");
        }
    }
    
    partial class AwaitExpressionSyntax
    {
        internal override void Validate()
        {
            base.Validate();

            ValidateNotNull(Expression, "Expression");
        }
    }
}
