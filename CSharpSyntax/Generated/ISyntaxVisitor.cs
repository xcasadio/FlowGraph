using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSyntax
{
    public interface ISyntaxVisitor
    {
        bool Done { get; }
        
        void VisitAccessorDeclaration(AccessorDeclarationSyntax node);
        
        void VisitAccessorList(AccessorListSyntax node);
        
        void VisitAliasQualifiedName(AliasQualifiedNameSyntax node);
        
        void VisitAnonymousMethodExpression(AnonymousMethodExpressionSyntax node);
        
        void VisitAnonymousObjectCreationExpression(AnonymousObjectCreationExpressionSyntax node);
        
        void VisitAnonymousObjectMemberDeclarator(AnonymousObjectMemberDeclaratorSyntax node);
        
        void VisitArgumentList(ArgumentListSyntax node);
        
        void VisitArgument(ArgumentSyntax node);
        
        void VisitArrayCreationExpression(ArrayCreationExpressionSyntax node);
        
        void VisitArrayRankSpecifier(ArrayRankSpecifierSyntax node);
        
        void VisitArrayType(ArrayTypeSyntax node);
        
        void VisitAttributeArgumentList(AttributeArgumentListSyntax node);
        
        void VisitAttributeArgument(AttributeArgumentSyntax node);
        
        void VisitAttributeList(AttributeListSyntax node);
        
        void VisitAttribute(AttributeSyntax node);
        
        void VisitAwaitExpression(AwaitExpressionSyntax node);
        
        void VisitBaseExpression(BaseExpressionSyntax node);
        
        void VisitBaseList(BaseListSyntax node);
        
        void VisitBinaryExpression(BinaryExpressionSyntax node);
        
        void VisitBlock(BlockSyntax node);
        
        void VisitBracketedArgumentList(BracketedArgumentListSyntax node);
        
        void VisitBracketedParameterList(BracketedParameterListSyntax node);
        
        void VisitBreakStatement(BreakStatementSyntax node);
        
        void VisitCastExpression(CastExpressionSyntax node);
        
        void VisitCatchClause(CatchClauseSyntax node);
        
        void VisitCatchDeclaration(CatchDeclarationSyntax node);
        
        void VisitCheckedExpression(CheckedExpressionSyntax node);
        
        void VisitCheckedStatement(CheckedStatementSyntax node);
        
        void VisitClassDeclaration(ClassDeclarationSyntax node);
        
        void VisitClassOrStructConstraint(ClassOrStructConstraintSyntax node);
        
        void VisitCompilationUnit(CompilationUnitSyntax node);
        
        void VisitConditionalExpression(ConditionalExpressionSyntax node);
        
        void VisitConstructorConstraint(ConstructorConstraintSyntax node);
        
        void VisitConstructorDeclaration(ConstructorDeclarationSyntax node);
        
        void VisitConstructorInitializer(ConstructorInitializerSyntax node);
        
        void VisitContinueStatement(ContinueStatementSyntax node);
        
        void VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node);
        
        void VisitDefaultExpression(DefaultExpressionSyntax node);
        
        void VisitDelegateDeclaration(DelegateDeclarationSyntax node);
        
        void VisitDestructorDeclaration(DestructorDeclarationSyntax node);
        
        void VisitDoStatement(DoStatementSyntax node);
        
        void VisitElementAccessExpression(ElementAccessExpressionSyntax node);
        
        void VisitElseClause(ElseClauseSyntax node);
        
        void VisitEmptyStatement(EmptyStatementSyntax node);
        
        void VisitEnumDeclaration(EnumDeclarationSyntax node);
        
        void VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node);
        
        void VisitEqualsValueClause(EqualsValueClauseSyntax node);
        
        void VisitEventDeclaration(EventDeclarationSyntax node);
        
        void VisitEventFieldDeclaration(EventFieldDeclarationSyntax node);
        
        void VisitExplicitInterfaceSpecifier(ExplicitInterfaceSpecifierSyntax node);
        
        void VisitExpressionStatement(ExpressionStatementSyntax node);
        
        void VisitExternAliasDirective(ExternAliasDirectiveSyntax node);
        
        void VisitFieldDeclaration(FieldDeclarationSyntax node);
        
        void VisitFinallyClause(FinallyClauseSyntax node);
        
        void VisitForEachStatement(ForEachStatementSyntax node);
        
        void VisitForStatement(ForStatementSyntax node);
        
        void VisitFromClause(FromClauseSyntax node);
        
        void VisitGenericName(GenericNameSyntax node);
        
        void VisitGotoStatement(GotoStatementSyntax node);
        
        void VisitGroupClause(GroupClauseSyntax node);
        
        void VisitIdentifierName(IdentifierNameSyntax node);
        
        void VisitIfStatement(IfStatementSyntax node);
        
        void VisitImplicitArrayCreationExpression(ImplicitArrayCreationExpressionSyntax node);
        
        void VisitIndexerDeclaration(IndexerDeclarationSyntax node);
        
        void VisitInitializerExpression(InitializerExpressionSyntax node);
        
        void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node);
        
        void VisitInvocationExpression(InvocationExpressionSyntax node);
        
        void VisitJoinClause(JoinClauseSyntax node);
        
        void VisitJoinIntoClause(JoinIntoClauseSyntax node);
        
        void VisitLabeledStatement(LabeledStatementSyntax node);
        
        void VisitLetClause(LetClauseSyntax node);
        
        void VisitLiteralExpression(LiteralExpressionSyntax node);
        
        void VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node);
        
        void VisitLockStatement(LockStatementSyntax node);
        
        void VisitMemberAccessExpression(MemberAccessExpressionSyntax node);
        
        void VisitMethodDeclaration(MethodDeclarationSyntax node);
        
        void VisitNameColon(NameColonSyntax node);
        
        void VisitNameEquals(NameEqualsSyntax node);
        
        void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node);
        
        void VisitNullableType(NullableTypeSyntax node);
        
        void VisitObjectCreationExpression(ObjectCreationExpressionSyntax node);
        
        void VisitOmittedArraySizeExpression(OmittedArraySizeExpressionSyntax node);
        
        void VisitOmittedTypeArgument(OmittedTypeArgumentSyntax node);
        
        void VisitOperatorDeclaration(OperatorDeclarationSyntax node);
        
        void VisitOrderByClause(OrderByClauseSyntax node);
        
        void VisitOrdering(OrderingSyntax node);
        
        void VisitParameterList(ParameterListSyntax node);
        
        void VisitParameter(ParameterSyntax node);
        
        void VisitParenthesizedExpression(ParenthesizedExpressionSyntax node);
        
        void VisitParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax node);
        
        void VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax node);
        
        void VisitPredefinedType(PredefinedTypeSyntax node);
        
        void VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node);
        
        void VisitPropertyDeclaration(PropertyDeclarationSyntax node);
        
        void VisitQualifiedName(QualifiedNameSyntax node);
        
        void VisitQueryBody(QueryBodySyntax node);
        
        void VisitQueryContinuation(QueryContinuationSyntax node);
        
        void VisitQueryExpression(QueryExpressionSyntax node);
        
        void VisitReturnStatement(ReturnStatementSyntax node);
        
        void VisitSelectClause(SelectClauseSyntax node);
        
        void VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node);
        
        void VisitSizeOfExpression(SizeOfExpressionSyntax node);
        
        void VisitStructDeclaration(StructDeclarationSyntax node);
        
        void VisitSwitchLabel(SwitchLabelSyntax node);
        
        void VisitSwitchSection(SwitchSectionSyntax node);
        
        void VisitSwitchStatement(SwitchStatementSyntax node);
        
        void VisitThisExpression(ThisExpressionSyntax node);
        
        void VisitThrowStatement(ThrowStatementSyntax node);
        
        void VisitTryStatement(TryStatementSyntax node);
        
        void VisitTypeArgumentList(TypeArgumentListSyntax node);
        
        void VisitTypeConstraint(TypeConstraintSyntax node);
        
        void VisitTypeOfExpression(TypeOfExpressionSyntax node);
        
        void VisitTypeParameterConstraintClause(TypeParameterConstraintClauseSyntax node);
        
        void VisitTypeParameterList(TypeParameterListSyntax node);
        
        void VisitTypeParameter(TypeParameterSyntax node);
        
        void VisitUsingDirective(UsingDirectiveSyntax node);
        
        void VisitUsingStatement(UsingStatementSyntax node);
        
        void VisitVariableDeclaration(VariableDeclarationSyntax node);
        
        void VisitVariableDeclarator(VariableDeclaratorSyntax node);
        
        void VisitWhereClause(WhereClauseSyntax node);
        
        void VisitWhileStatement(WhileStatementSyntax node);
        
        void VisitYieldStatement(YieldStatementSyntax node);
    }
    
    public interface ISyntaxVisitor<T>
    {
        T VisitAccessorDeclaration(AccessorDeclarationSyntax node);
        
        T VisitAccessorList(AccessorListSyntax node);
        
        T VisitAliasQualifiedName(AliasQualifiedNameSyntax node);
        
        T VisitAnonymousMethodExpression(AnonymousMethodExpressionSyntax node);
        
        T VisitAnonymousObjectCreationExpression(AnonymousObjectCreationExpressionSyntax node);
        
        T VisitAnonymousObjectMemberDeclarator(AnonymousObjectMemberDeclaratorSyntax node);
        
        T VisitArgumentList(ArgumentListSyntax node);
        
        T VisitArgument(ArgumentSyntax node);
        
        T VisitArrayCreationExpression(ArrayCreationExpressionSyntax node);
        
        T VisitArrayRankSpecifier(ArrayRankSpecifierSyntax node);
        
        T VisitArrayType(ArrayTypeSyntax node);
        
        T VisitAttributeArgumentList(AttributeArgumentListSyntax node);
        
        T VisitAttributeArgument(AttributeArgumentSyntax node);
        
        T VisitAttributeList(AttributeListSyntax node);
        
        T VisitAttribute(AttributeSyntax node);
        
        T VisitAwaitExpression(AwaitExpressionSyntax node);
        
        T VisitBaseExpression(BaseExpressionSyntax node);
        
        T VisitBaseList(BaseListSyntax node);
        
        T VisitBinaryExpression(BinaryExpressionSyntax node);
        
        T VisitBlock(BlockSyntax node);
        
        T VisitBracketedArgumentList(BracketedArgumentListSyntax node);
        
        T VisitBracketedParameterList(BracketedParameterListSyntax node);
        
        T VisitBreakStatement(BreakStatementSyntax node);
        
        T VisitCastExpression(CastExpressionSyntax node);
        
        T VisitCatchClause(CatchClauseSyntax node);
        
        T VisitCatchDeclaration(CatchDeclarationSyntax node);
        
        T VisitCheckedExpression(CheckedExpressionSyntax node);
        
        T VisitCheckedStatement(CheckedStatementSyntax node);
        
        T VisitClassDeclaration(ClassDeclarationSyntax node);
        
        T VisitClassOrStructConstraint(ClassOrStructConstraintSyntax node);
        
        T VisitCompilationUnit(CompilationUnitSyntax node);
        
        T VisitConditionalExpression(ConditionalExpressionSyntax node);
        
        T VisitConstructorConstraint(ConstructorConstraintSyntax node);
        
        T VisitConstructorDeclaration(ConstructorDeclarationSyntax node);
        
        T VisitConstructorInitializer(ConstructorInitializerSyntax node);
        
        T VisitContinueStatement(ContinueStatementSyntax node);
        
        T VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node);
        
        T VisitDefaultExpression(DefaultExpressionSyntax node);
        
        T VisitDelegateDeclaration(DelegateDeclarationSyntax node);
        
        T VisitDestructorDeclaration(DestructorDeclarationSyntax node);
        
        T VisitDoStatement(DoStatementSyntax node);
        
        T VisitElementAccessExpression(ElementAccessExpressionSyntax node);
        
        T VisitElseClause(ElseClauseSyntax node);
        
        T VisitEmptyStatement(EmptyStatementSyntax node);
        
        T VisitEnumDeclaration(EnumDeclarationSyntax node);
        
        T VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node);
        
        T VisitEqualsValueClause(EqualsValueClauseSyntax node);
        
        T VisitEventDeclaration(EventDeclarationSyntax node);
        
        T VisitEventFieldDeclaration(EventFieldDeclarationSyntax node);
        
        T VisitExplicitInterfaceSpecifier(ExplicitInterfaceSpecifierSyntax node);
        
        T VisitExpressionStatement(ExpressionStatementSyntax node);
        
        T VisitExternAliasDirective(ExternAliasDirectiveSyntax node);
        
        T VisitFieldDeclaration(FieldDeclarationSyntax node);
        
        T VisitFinallyClause(FinallyClauseSyntax node);
        
        T VisitForEachStatement(ForEachStatementSyntax node);
        
        T VisitForStatement(ForStatementSyntax node);
        
        T VisitFromClause(FromClauseSyntax node);
        
        T VisitGenericName(GenericNameSyntax node);
        
        T VisitGotoStatement(GotoStatementSyntax node);
        
        T VisitGroupClause(GroupClauseSyntax node);
        
        T VisitIdentifierName(IdentifierNameSyntax node);
        
        T VisitIfStatement(IfStatementSyntax node);
        
        T VisitImplicitArrayCreationExpression(ImplicitArrayCreationExpressionSyntax node);
        
        T VisitIndexerDeclaration(IndexerDeclarationSyntax node);
        
        T VisitInitializerExpression(InitializerExpressionSyntax node);
        
        T VisitInterfaceDeclaration(InterfaceDeclarationSyntax node);
        
        T VisitInvocationExpression(InvocationExpressionSyntax node);
        
        T VisitJoinClause(JoinClauseSyntax node);
        
        T VisitJoinIntoClause(JoinIntoClauseSyntax node);
        
        T VisitLabeledStatement(LabeledStatementSyntax node);
        
        T VisitLetClause(LetClauseSyntax node);
        
        T VisitLiteralExpression(LiteralExpressionSyntax node);
        
        T VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node);
        
        T VisitLockStatement(LockStatementSyntax node);
        
        T VisitMemberAccessExpression(MemberAccessExpressionSyntax node);
        
        T VisitMethodDeclaration(MethodDeclarationSyntax node);
        
        T VisitNameColon(NameColonSyntax node);
        
        T VisitNameEquals(NameEqualsSyntax node);
        
        T VisitNamespaceDeclaration(NamespaceDeclarationSyntax node);
        
        T VisitNullableType(NullableTypeSyntax node);
        
        T VisitObjectCreationExpression(ObjectCreationExpressionSyntax node);
        
        T VisitOmittedArraySizeExpression(OmittedArraySizeExpressionSyntax node);
        
        T VisitOmittedTypeArgument(OmittedTypeArgumentSyntax node);
        
        T VisitOperatorDeclaration(OperatorDeclarationSyntax node);
        
        T VisitOrderByClause(OrderByClauseSyntax node);
        
        T VisitOrdering(OrderingSyntax node);
        
        T VisitParameterList(ParameterListSyntax node);
        
        T VisitParameter(ParameterSyntax node);
        
        T VisitParenthesizedExpression(ParenthesizedExpressionSyntax node);
        
        T VisitParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax node);
        
        T VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax node);
        
        T VisitPredefinedType(PredefinedTypeSyntax node);
        
        T VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node);
        
        T VisitPropertyDeclaration(PropertyDeclarationSyntax node);
        
        T VisitQualifiedName(QualifiedNameSyntax node);
        
        T VisitQueryBody(QueryBodySyntax node);
        
        T VisitQueryContinuation(QueryContinuationSyntax node);
        
        T VisitQueryExpression(QueryExpressionSyntax node);
        
        T VisitReturnStatement(ReturnStatementSyntax node);
        
        T VisitSelectClause(SelectClauseSyntax node);
        
        T VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node);
        
        T VisitSizeOfExpression(SizeOfExpressionSyntax node);
        
        T VisitStructDeclaration(StructDeclarationSyntax node);
        
        T VisitSwitchLabel(SwitchLabelSyntax node);
        
        T VisitSwitchSection(SwitchSectionSyntax node);
        
        T VisitSwitchStatement(SwitchStatementSyntax node);
        
        T VisitThisExpression(ThisExpressionSyntax node);
        
        T VisitThrowStatement(ThrowStatementSyntax node);
        
        T VisitTryStatement(TryStatementSyntax node);
        
        T VisitTypeArgumentList(TypeArgumentListSyntax node);
        
        T VisitTypeConstraint(TypeConstraintSyntax node);
        
        T VisitTypeOfExpression(TypeOfExpressionSyntax node);
        
        T VisitTypeParameterConstraintClause(TypeParameterConstraintClauseSyntax node);
        
        T VisitTypeParameterList(TypeParameterListSyntax node);
        
        T VisitTypeParameter(TypeParameterSyntax node);
        
        T VisitUsingDirective(UsingDirectiveSyntax node);
        
        T VisitUsingStatement(UsingStatementSyntax node);
        
        T VisitVariableDeclaration(VariableDeclarationSyntax node);
        
        T VisitVariableDeclarator(VariableDeclaratorSyntax node);
        
        T VisitWhereClause(WhereClauseSyntax node);
        
        T VisitWhileStatement(WhileStatementSyntax node);
        
        T VisitYieldStatement(YieldStatementSyntax node);
    }
}
