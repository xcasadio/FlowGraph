using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public class SyntaxVisitorBase : ISyntaxVisitor
    {
        public bool Done { get; protected set; }
        
        [DebuggerStepThrough]
        public virtual void VisitAccessorDeclaration(AccessorDeclarationSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitAccessorList(AccessorListSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitAliasQualifiedName(AliasQualifiedNameSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitAnonymousMethodExpression(AnonymousMethodExpressionSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitAnonymousObjectCreationExpression(AnonymousObjectCreationExpressionSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitAnonymousObjectMemberDeclarator(AnonymousObjectMemberDeclaratorSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitArgumentList(ArgumentListSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitArgument(ArgumentSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitArrayCreationExpression(ArrayCreationExpressionSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitArrayRankSpecifier(ArrayRankSpecifierSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitArrayType(ArrayTypeSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitAttributeArgumentList(AttributeArgumentListSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitAttributeArgument(AttributeArgumentSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitAttributeList(AttributeListSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitAttribute(AttributeSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitAwaitExpression(AwaitExpressionSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitBaseExpression(BaseExpressionSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitBaseList(BaseListSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitBinaryExpression(BinaryExpressionSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitBlock(BlockSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitBracketedArgumentList(BracketedArgumentListSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitBracketedParameterList(BracketedParameterListSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitBreakStatement(BreakStatementSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitCastExpression(CastExpressionSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitCatchClause(CatchClauseSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitCatchDeclaration(CatchDeclarationSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitCheckedExpression(CheckedExpressionSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitCheckedStatement(CheckedStatementSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitClassOrStructConstraint(ClassOrStructConstraintSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitCompilationUnit(CompilationUnitSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitConditionalExpression(ConditionalExpressionSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitConstructorConstraint(ConstructorConstraintSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitConstructorInitializer(ConstructorInitializerSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitContinueStatement(ContinueStatementSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitDefaultExpression(DefaultExpressionSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitDelegateDeclaration(DelegateDeclarationSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitDestructorDeclaration(DestructorDeclarationSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitDoStatement(DoStatementSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitElementAccessExpression(ElementAccessExpressionSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitElseClause(ElseClauseSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitEmptyStatement(EmptyStatementSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitEqualsValueClause(EqualsValueClauseSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitEventDeclaration(EventDeclarationSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitEventFieldDeclaration(EventFieldDeclarationSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitExplicitInterfaceSpecifier(ExplicitInterfaceSpecifierSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitExpressionStatement(ExpressionStatementSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitExternAliasDirective(ExternAliasDirectiveSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitFinallyClause(FinallyClauseSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitForEachStatement(ForEachStatementSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitForStatement(ForStatementSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitFromClause(FromClauseSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitGenericName(GenericNameSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitGotoStatement(GotoStatementSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitGroupClause(GroupClauseSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitIdentifierName(IdentifierNameSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitIfStatement(IfStatementSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitImplicitArrayCreationExpression(ImplicitArrayCreationExpressionSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitIndexerDeclaration(IndexerDeclarationSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitInitializerExpression(InitializerExpressionSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitJoinClause(JoinClauseSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitJoinIntoClause(JoinIntoClauseSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitLabeledStatement(LabeledStatementSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitLetClause(LetClauseSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitLiteralExpression(LiteralExpressionSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitLockStatement(LockStatementSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitNameColon(NameColonSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitNameEquals(NameEqualsSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitNullableType(NullableTypeSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitOmittedArraySizeExpression(OmittedArraySizeExpressionSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitOmittedTypeArgument(OmittedTypeArgumentSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitOperatorDeclaration(OperatorDeclarationSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitOrderByClause(OrderByClauseSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitOrdering(OrderingSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitParameterList(ParameterListSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitParameter(ParameterSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitParenthesizedExpression(ParenthesizedExpressionSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitPredefinedType(PredefinedTypeSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitQualifiedName(QualifiedNameSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitQueryBody(QueryBodySyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitQueryContinuation(QueryContinuationSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitQueryExpression(QueryExpressionSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitReturnStatement(ReturnStatementSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitSelectClause(SelectClauseSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitSizeOfExpression(SizeOfExpressionSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitStructDeclaration(StructDeclarationSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitSwitchLabel(SwitchLabelSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitSwitchSection(SwitchSectionSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitSwitchStatement(SwitchStatementSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitThisExpression(ThisExpressionSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitThrowStatement(ThrowStatementSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitTryStatement(TryStatementSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitTypeArgumentList(TypeArgumentListSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitTypeConstraint(TypeConstraintSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitTypeOfExpression(TypeOfExpressionSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitTypeParameterConstraintClause(TypeParameterConstraintClauseSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitTypeParameterList(TypeParameterListSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitTypeParameter(TypeParameterSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitUsingDirective(UsingDirectiveSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitUsingStatement(UsingStatementSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitVariableDeclarator(VariableDeclaratorSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitWhereClause(WhereClauseSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitWhileStatement(WhileStatementSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void VisitYieldStatement(YieldStatementSyntax node)
        {
            DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual void Visit(SyntaxNode node)
        {
            if (node != null)
                node.Accept(this);
        }
        
        [DebuggerStepThrough]
        public virtual void DefaultVisit(SyntaxNode node)
        {
        }
    }
    
    public class SyntaxVisitorBase<T> : ISyntaxVisitor<T>
    {
        [DebuggerStepThrough]
        public virtual T VisitAccessorDeclaration(AccessorDeclarationSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitAccessorList(AccessorListSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitAliasQualifiedName(AliasQualifiedNameSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitAnonymousMethodExpression(AnonymousMethodExpressionSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitAnonymousObjectCreationExpression(AnonymousObjectCreationExpressionSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitAnonymousObjectMemberDeclarator(AnonymousObjectMemberDeclaratorSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitArgumentList(ArgumentListSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitArgument(ArgumentSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitArrayCreationExpression(ArrayCreationExpressionSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitArrayRankSpecifier(ArrayRankSpecifierSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitArrayType(ArrayTypeSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitAttributeArgumentList(AttributeArgumentListSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitAttributeArgument(AttributeArgumentSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitAttributeList(AttributeListSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitAttribute(AttributeSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitAwaitExpression(AwaitExpressionSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitBaseExpression(BaseExpressionSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitBaseList(BaseListSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitBinaryExpression(BinaryExpressionSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitBlock(BlockSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitBracketedArgumentList(BracketedArgumentListSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitBracketedParameterList(BracketedParameterListSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitBreakStatement(BreakStatementSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitCastExpression(CastExpressionSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitCatchClause(CatchClauseSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitCatchDeclaration(CatchDeclarationSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitCheckedExpression(CheckedExpressionSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitCheckedStatement(CheckedStatementSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitClassOrStructConstraint(ClassOrStructConstraintSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitCompilationUnit(CompilationUnitSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitConditionalExpression(ConditionalExpressionSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitConstructorConstraint(ConstructorConstraintSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitConstructorInitializer(ConstructorInitializerSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitContinueStatement(ContinueStatementSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitDefaultExpression(DefaultExpressionSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitDelegateDeclaration(DelegateDeclarationSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitDestructorDeclaration(DestructorDeclarationSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitDoStatement(DoStatementSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitElementAccessExpression(ElementAccessExpressionSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitElseClause(ElseClauseSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitEmptyStatement(EmptyStatementSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitEqualsValueClause(EqualsValueClauseSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitEventDeclaration(EventDeclarationSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitEventFieldDeclaration(EventFieldDeclarationSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitExplicitInterfaceSpecifier(ExplicitInterfaceSpecifierSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitExpressionStatement(ExpressionStatementSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitExternAliasDirective(ExternAliasDirectiveSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitFinallyClause(FinallyClauseSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitForEachStatement(ForEachStatementSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitForStatement(ForStatementSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitFromClause(FromClauseSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitGenericName(GenericNameSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitGotoStatement(GotoStatementSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitGroupClause(GroupClauseSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitIdentifierName(IdentifierNameSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitIfStatement(IfStatementSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitImplicitArrayCreationExpression(ImplicitArrayCreationExpressionSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitIndexerDeclaration(IndexerDeclarationSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitInitializerExpression(InitializerExpressionSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitJoinClause(JoinClauseSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitJoinIntoClause(JoinIntoClauseSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitLabeledStatement(LabeledStatementSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitLetClause(LetClauseSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitLiteralExpression(LiteralExpressionSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitLockStatement(LockStatementSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitNameColon(NameColonSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitNameEquals(NameEqualsSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitNullableType(NullableTypeSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitOmittedArraySizeExpression(OmittedArraySizeExpressionSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitOmittedTypeArgument(OmittedTypeArgumentSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitOperatorDeclaration(OperatorDeclarationSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitOrderByClause(OrderByClauseSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitOrdering(OrderingSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitParameterList(ParameterListSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitParameter(ParameterSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitParenthesizedExpression(ParenthesizedExpressionSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitPredefinedType(PredefinedTypeSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitQualifiedName(QualifiedNameSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitQueryBody(QueryBodySyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitQueryContinuation(QueryContinuationSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitQueryExpression(QueryExpressionSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitReturnStatement(ReturnStatementSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitSelectClause(SelectClauseSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitSizeOfExpression(SizeOfExpressionSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitStructDeclaration(StructDeclarationSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitSwitchLabel(SwitchLabelSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitSwitchSection(SwitchSectionSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitSwitchStatement(SwitchStatementSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitThisExpression(ThisExpressionSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitThrowStatement(ThrowStatementSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitTryStatement(TryStatementSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitTypeArgumentList(TypeArgumentListSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitTypeConstraint(TypeConstraintSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitTypeOfExpression(TypeOfExpressionSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitTypeParameterConstraintClause(TypeParameterConstraintClauseSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitTypeParameterList(TypeParameterListSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitTypeParameter(TypeParameterSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitUsingDirective(UsingDirectiveSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitUsingStatement(UsingStatementSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitVariableDeclarator(VariableDeclaratorSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitWhereClause(WhereClauseSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitWhileStatement(WhileStatementSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T VisitYieldStatement(YieldStatementSyntax node)
        {
            return DefaultVisit(node);
        }
        
        [DebuggerStepThrough]
        public virtual T Visit(SyntaxNode node)
        {
            if (node == null)
                return default(T);
            
            return node.Accept(this);
        }
        
        [DebuggerStepThrough]
        public virtual T DefaultVisit(SyntaxNode node)
        {
            return default(T);
        }
    }
}
