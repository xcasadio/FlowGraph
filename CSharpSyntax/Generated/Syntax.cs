using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSyntax
{
    public static partial class Syntax
    {
        public static AccessorDeclarationSyntax AccessorDeclaration(AccessorDeclarationKind kind = default(AccessorDeclarationKind), BlockSyntax body = null)
        {
            var result = new AccessorDeclarationSyntax();
            
            result.Kind = kind;
            result.Body = body;
            
            return result;
        }
        
        public static AccessorDeclarationSyntax AccessorDeclaration(AccessorDeclarationKind kind = default(AccessorDeclarationKind), IEnumerable<AttributeListSyntax> attributeLists = null, Modifiers modifiers = default(Modifiers), BlockSyntax body = null)
        {
            var result = new AccessorDeclarationSyntax();
            
            result.Kind = kind;
            if (attributeLists != null)
                result.AttributeLists.AddRange(attributeLists);
            result.Modifiers = modifiers;
            result.Body = body;
            
            return result;
        }
        
        public static AccessorListSyntax AccessorList(IEnumerable<AccessorDeclarationSyntax> accessors = null)
        {
            var result = new AccessorListSyntax();
            
            if (accessors != null)
                result.Accessors.AddRange(accessors);
            
            return result;
        }
        
        public static AccessorListSyntax AccessorList(params AccessorDeclarationSyntax[] accessors)
        {
            var result = new AccessorListSyntax();
            
            if (accessors != null)
                result.Accessors.AddRange(accessors);
            
            return result;
        }
        
        public static AliasQualifiedNameSyntax AliasQualifiedName(IdentifierNameSyntax alias = null, SimpleNameSyntax name = null)
        {
            var result = new AliasQualifiedNameSyntax();
            
            result.Alias = alias;
            result.Name = name;
            
            return result;
        }
        
        public static AliasQualifiedNameSyntax AliasQualifiedName(string alias = null, SimpleNameSyntax name = null)
        {
            var result = new AliasQualifiedNameSyntax();
            
            if (alias != null)
                result.Alias = (IdentifierNameSyntax)ParseName(alias);
            result.Name = name;
            
            return result;
        }
        
        public static AliasQualifiedNameSyntax AliasQualifiedName(string alias = null, string name = null)
        {
            var result = new AliasQualifiedNameSyntax();
            
            if (alias != null)
                result.Alias = (IdentifierNameSyntax)ParseName(alias);
            if (name != null)
                result.Name = (SimpleNameSyntax)ParseName(name);
            
            return result;
        }
        
        public static AnonymousMethodExpressionSyntax AnonymousMethodExpression(ParameterListSyntax parameterList = null, BlockSyntax block = null)
        {
            var result = new AnonymousMethodExpressionSyntax();
            
            result.ParameterList = parameterList;
            result.Block = block;
            
            return result;
        }
        
        public static AnonymousMethodExpressionSyntax AnonymousMethodExpression(Modifiers modifiers = default(Modifiers), ParameterListSyntax parameterList = null, BlockSyntax block = null)
        {
            var result = new AnonymousMethodExpressionSyntax();
            
            result.Modifiers = modifiers;
            result.ParameterList = parameterList;
            result.Block = block;
            
            return result;
        }
        
        public static AnonymousObjectCreationExpressionSyntax AnonymousObjectCreationExpression(IEnumerable<AnonymousObjectMemberDeclaratorSyntax> initializers = null)
        {
            var result = new AnonymousObjectCreationExpressionSyntax();
            
            if (initializers != null)
                result.Initializers.AddRange(initializers);
            
            return result;
        }
        
        public static AnonymousObjectCreationExpressionSyntax AnonymousObjectCreationExpression(params AnonymousObjectMemberDeclaratorSyntax[] initializers)
        {
            var result = new AnonymousObjectCreationExpressionSyntax();
            
            if (initializers != null)
                result.Initializers.AddRange(initializers);
            
            return result;
        }
        
        public static AnonymousObjectMemberDeclaratorSyntax AnonymousObjectMemberDeclarator(NameEqualsSyntax nameEquals = null, ExpressionSyntax expression = null)
        {
            var result = new AnonymousObjectMemberDeclaratorSyntax();
            
            result.NameEquals = nameEquals;
            result.Expression = expression;
            
            return result;
        }
        
        public static AnonymousObjectMemberDeclaratorSyntax AnonymousObjectMemberDeclarator(ExpressionSyntax expression = null)
        {
            var result = new AnonymousObjectMemberDeclaratorSyntax();
            
            result.Expression = expression;
            
            return result;
        }
        
        public static ArgumentListSyntax ArgumentList(IEnumerable<ArgumentSyntax> arguments = null)
        {
            var result = new ArgumentListSyntax();
            
            if (arguments != null)
                result.Arguments.AddRange(arguments);
            
            return result;
        }
        
        public static ArgumentListSyntax ArgumentList(params ArgumentSyntax[] arguments)
        {
            var result = new ArgumentListSyntax();
            
            if (arguments != null)
                result.Arguments.AddRange(arguments);
            
            return result;
        }
        
        public static ArgumentSyntax Argument(NameColonSyntax nameColon = null, ExpressionSyntax expression = null)
        {
            var result = new ArgumentSyntax();
            
            result.NameColon = nameColon;
            result.Expression = expression;
            
            return result;
        }
        
        public static ArgumentSyntax Argument(ExpressionSyntax expression = null)
        {
            var result = new ArgumentSyntax();
            
            result.Expression = expression;
            
            return result;
        }
        
        public static ArrayCreationExpressionSyntax ArrayCreationExpression(ArrayTypeSyntax type = null, InitializerExpressionSyntax initializer = null)
        {
            var result = new ArrayCreationExpressionSyntax();
            
            result.Type = type;
            result.Initializer = initializer;
            
            return result;
        }
        
        public static ArrayCreationExpressionSyntax ArrayCreationExpression(string type = null, InitializerExpressionSyntax initializer = null)
        {
            var result = new ArrayCreationExpressionSyntax();
            
            if (type != null)
                result.Type = (ArrayTypeSyntax)ParseName(type);
            result.Initializer = initializer;
            
            return result;
        }
        
        public static ArrayRankSpecifierSyntax ArrayRankSpecifier(IEnumerable<ExpressionSyntax> sizes = null)
        {
            var result = new ArrayRankSpecifierSyntax();
            
            if (sizes != null)
                result.Sizes.AddRange(sizes);
            
            return result;
        }
        
        public static ArrayRankSpecifierSyntax ArrayRankSpecifier(params ExpressionSyntax[] sizes)
        {
            var result = new ArrayRankSpecifierSyntax();
            
            if (sizes != null)
                result.Sizes.AddRange(sizes);
            
            return result;
        }
        
        public static ArrayTypeSyntax ArrayType(TypeSyntax elementType = null, IEnumerable<ArrayRankSpecifierSyntax> rankSpecifiers = null)
        {
            var result = new ArrayTypeSyntax();
            
            result.ElementType = elementType;
            if (rankSpecifiers != null)
                result.RankSpecifiers.AddRange(rankSpecifiers);
            
            return result;
        }
        
        public static ArrayTypeSyntax ArrayType(string elementType = null, IEnumerable<ArrayRankSpecifierSyntax> rankSpecifiers = null)
        {
            var result = new ArrayTypeSyntax();
            
            if (elementType != null)
                result.ElementType = ParseName(elementType);
            if (rankSpecifiers != null)
                result.RankSpecifiers.AddRange(rankSpecifiers);
            
            return result;
        }
        
        public static AttributeArgumentListSyntax AttributeArgumentList(IEnumerable<AttributeArgumentSyntax> arguments = null)
        {
            var result = new AttributeArgumentListSyntax();
            
            if (arguments != null)
                result.Arguments.AddRange(arguments);
            
            return result;
        }
        
        public static AttributeArgumentListSyntax AttributeArgumentList(params AttributeArgumentSyntax[] arguments)
        {
            var result = new AttributeArgumentListSyntax();
            
            if (arguments != null)
                result.Arguments.AddRange(arguments);
            
            return result;
        }
        
        public static AttributeArgumentSyntax AttributeArgument(NameEqualsSyntax nameEquals = null, NameColonSyntax nameColon = null, ExpressionSyntax expression = null)
        {
            var result = new AttributeArgumentSyntax();
            
            result.NameEquals = nameEquals;
            result.NameColon = nameColon;
            result.Expression = expression;
            
            return result;
        }
        
        public static AttributeArgumentSyntax AttributeArgument(ExpressionSyntax expression = null)
        {
            var result = new AttributeArgumentSyntax();
            
            result.Expression = expression;
            
            return result;
        }
        
        public static AttributeListSyntax AttributeList(AttributeTarget target = default(AttributeTarget), IEnumerable<AttributeSyntax> attributes = null)
        {
            var result = new AttributeListSyntax();
            
            result.Target = target;
            if (attributes != null)
                result.Attributes.AddRange(attributes);
            
            return result;
        }
        
        public static AttributeListSyntax AttributeList(IEnumerable<AttributeSyntax> attributes = null)
        {
            var result = new AttributeListSyntax();
            
            if (attributes != null)
                result.Attributes.AddRange(attributes);
            
            return result;
        }
        
        public static AttributeListSyntax AttributeList(params AttributeSyntax[] attributes)
        {
            var result = new AttributeListSyntax();
            
            if (attributes != null)
                result.Attributes.AddRange(attributes);
            
            return result;
        }
        
        public static AttributeSyntax Attribute(NameSyntax name = null, AttributeArgumentListSyntax argumentList = null)
        {
            var result = new AttributeSyntax();
            
            result.Name = name;
            result.ArgumentList = argumentList;
            
            return result;
        }
        
        public static AttributeSyntax Attribute(string name = null, AttributeArgumentListSyntax argumentList = null)
        {
            var result = new AttributeSyntax();
            
            if (name != null)
                result.Name = (NameSyntax)ParseName(name);
            result.ArgumentList = argumentList;
            
            return result;
        }
        
        public static AwaitExpressionSyntax AwaitExpression(ExpressionSyntax expression = null)
        {
            var result = new AwaitExpressionSyntax();
            
            result.Expression = expression;
            
            return result;
        }
        
        public static BaseExpressionSyntax BaseExpression()
        {
            var result = new BaseExpressionSyntax();
            
            
            return result;
        }
        
        public static BaseListSyntax BaseList(IEnumerable<TypeSyntax> types = null)
        {
            var result = new BaseListSyntax();
            
            if (types != null)
                result.Types.AddRange(types);
            
            return result;
        }
        
        public static BaseListSyntax BaseList(params TypeSyntax[] types)
        {
            var result = new BaseListSyntax();
            
            if (types != null)
                result.Types.AddRange(types);
            
            return result;
        }
        
        public static BaseListSyntax BaseList(IEnumerable<string> types = null)
        {
            var result = new BaseListSyntax();
            
            if (types != null)
                result.Types.AddRange(ParseNames<TypeSyntax>(types));
            
            return result;
        }
        
        public static BaseListSyntax BaseList(params string[] types)
        {
            var result = new BaseListSyntax();
            
            if (types != null)
                result.Types.AddRange(ParseNames<TypeSyntax>(types));
            
            return result;
        }
        
        public static BinaryExpressionSyntax BinaryExpression(BinaryOperator @operator = default(BinaryOperator), ExpressionSyntax left = null, ExpressionSyntax right = null)
        {
            var result = new BinaryExpressionSyntax();
            
            result.Operator = @operator;
            result.Left = left;
            result.Right = right;
            
            return result;
        }
        
        public static BlockSyntax Block(IEnumerable<StatementSyntax> statements = null)
        {
            var result = new BlockSyntax();
            
            if (statements != null)
                result.Statements.AddRange(statements);
            
            return result;
        }
        
        public static BlockSyntax Block(params StatementSyntax[] statements)
        {
            var result = new BlockSyntax();
            
            if (statements != null)
                result.Statements.AddRange(statements);
            
            return result;
        }
        
        public static BracketedArgumentListSyntax BracketedArgumentList(IEnumerable<ArgumentSyntax> arguments = null)
        {
            var result = new BracketedArgumentListSyntax();
            
            if (arguments != null)
                result.Arguments.AddRange(arguments);
            
            return result;
        }
        
        public static BracketedArgumentListSyntax BracketedArgumentList(params ArgumentSyntax[] arguments)
        {
            var result = new BracketedArgumentListSyntax();
            
            if (arguments != null)
                result.Arguments.AddRange(arguments);
            
            return result;
        }
        
        public static BracketedParameterListSyntax BracketedParameterList(IEnumerable<ParameterSyntax> parameters = null)
        {
            var result = new BracketedParameterListSyntax();
            
            if (parameters != null)
                result.Parameters.AddRange(parameters);
            
            return result;
        }
        
        public static BracketedParameterListSyntax BracketedParameterList(params ParameterSyntax[] parameters)
        {
            var result = new BracketedParameterListSyntax();
            
            if (parameters != null)
                result.Parameters.AddRange(parameters);
            
            return result;
        }
        
        public static BreakStatementSyntax BreakStatement()
        {
            var result = new BreakStatementSyntax();
            
            
            return result;
        }
        
        public static CastExpressionSyntax CastExpression(TypeSyntax type = null, ExpressionSyntax expression = null)
        {
            var result = new CastExpressionSyntax();
            
            result.Type = type;
            result.Expression = expression;
            
            return result;
        }
        
        public static CastExpressionSyntax CastExpression(string type = null, ExpressionSyntax expression = null)
        {
            var result = new CastExpressionSyntax();
            
            if (type != null)
                result.Type = ParseName(type);
            result.Expression = expression;
            
            return result;
        }
        
        public static CatchClauseSyntax CatchClause(CatchDeclarationSyntax declaration = null, BlockSyntax block = null)
        {
            var result = new CatchClauseSyntax();
            
            result.Declaration = declaration;
            result.Block = block;
            
            return result;
        }
        
        public static CatchDeclarationSyntax CatchDeclaration(TypeSyntax type = null, string identifier = null)
        {
            var result = new CatchDeclarationSyntax();
            
            result.Type = type;
            result.Identifier = identifier;
            
            return result;
        }
        
        public static CatchDeclarationSyntax CatchDeclaration(string type = null, string identifier = null)
        {
            var result = new CatchDeclarationSyntax();
            
            if (type != null)
                result.Type = ParseName(type);
            result.Identifier = identifier;
            
            return result;
        }
        
        public static CheckedExpressionSyntax CheckedExpression(CheckedOrUnchecked kind = default(CheckedOrUnchecked), ExpressionSyntax expression = null)
        {
            var result = new CheckedExpressionSyntax();
            
            result.Kind = kind;
            result.Expression = expression;
            
            return result;
        }
        
        public static CheckedStatementSyntax CheckedStatement(CheckedOrUnchecked kind = default(CheckedOrUnchecked), BlockSyntax block = null)
        {
            var result = new CheckedStatementSyntax();
            
            result.Kind = kind;
            result.Block = block;
            
            return result;
        }
        
        public static ClassDeclarationSyntax ClassDeclaration(IEnumerable<AttributeListSyntax> attributeLists = null, Modifiers modifiers = default(Modifiers), string identifier = null, TypeParameterListSyntax typeParameterList = null, BaseListSyntax baseList = null, IEnumerable<TypeParameterConstraintClauseSyntax> constraintClauses = null, IEnumerable<MemberDeclarationSyntax> members = null)
        {
            var result = new ClassDeclarationSyntax();
            
            if (attributeLists != null)
                result.AttributeLists.AddRange(attributeLists);
            result.Modifiers = modifiers;
            result.Identifier = identifier;
            result.TypeParameterList = typeParameterList;
            result.BaseList = baseList;
            if (constraintClauses != null)
                result.ConstraintClauses.AddRange(constraintClauses);
            if (members != null)
                result.Members.AddRange(members);
            
            return result;
        }
        
        public static ClassDeclarationSyntax ClassDeclaration(string identifier = null)
        {
            var result = new ClassDeclarationSyntax();
            
            result.Identifier = identifier;
            
            return result;
        }
        
        public static ClassOrStructConstraintSyntax ClassOrStructConstraint(ClassOrStruct kind = default(ClassOrStruct))
        {
            var result = new ClassOrStructConstraintSyntax();
            
            result.Kind = kind;
            
            return result;
        }
        
        public static CompilationUnitSyntax CompilationUnit(IEnumerable<ExternAliasDirectiveSyntax> externs = null, IEnumerable<UsingDirectiveSyntax> usings = null, IEnumerable<AttributeListSyntax> attributeLists = null, IEnumerable<MemberDeclarationSyntax> members = null)
        {
            var result = new CompilationUnitSyntax();
            
            if (externs != null)
                result.Externs.AddRange(externs);
            if (usings != null)
                result.Usings.AddRange(usings);
            if (attributeLists != null)
                result.AttributeLists.AddRange(attributeLists);
            if (members != null)
                result.Members.AddRange(members);
            
            return result;
        }
        
        public static ConditionalExpressionSyntax ConditionalExpression(ExpressionSyntax condition = null, ExpressionSyntax whenTrue = null, ExpressionSyntax whenFalse = null)
        {
            var result = new ConditionalExpressionSyntax();
            
            result.Condition = condition;
            result.WhenTrue = whenTrue;
            result.WhenFalse = whenFalse;
            
            return result;
        }
        
        public static ConstructorConstraintSyntax ConstructorConstraint()
        {
            var result = new ConstructorConstraintSyntax();
            
            
            return result;
        }
        
        public static ConstructorDeclarationSyntax ConstructorDeclaration(IEnumerable<AttributeListSyntax> attributeLists = null, Modifiers modifiers = default(Modifiers), string identifier = null, ParameterListSyntax parameterList = null, ConstructorInitializerSyntax initializer = null, BlockSyntax body = null)
        {
            var result = new ConstructorDeclarationSyntax();
            
            if (attributeLists != null)
                result.AttributeLists.AddRange(attributeLists);
            result.Modifiers = modifiers;
            result.Identifier = identifier;
            result.ParameterList = parameterList;
            result.Initializer = initializer;
            result.Body = body;
            
            return result;
        }
        
        public static ConstructorDeclarationSyntax ConstructorDeclaration(string identifier = null, ParameterListSyntax parameterList = null, BlockSyntax body = null)
        {
            var result = new ConstructorDeclarationSyntax();
            
            result.Identifier = identifier;
            result.ParameterList = parameterList;
            result.Body = body;
            
            return result;
        }
        
        public static ConstructorInitializerSyntax ConstructorInitializer(ThisOrBase kind = default(ThisOrBase), ArgumentListSyntax argumentList = null)
        {
            var result = new ConstructorInitializerSyntax();
            
            result.Kind = kind;
            result.ArgumentList = argumentList;
            
            return result;
        }
        
        public static ContinueStatementSyntax ContinueStatement()
        {
            var result = new ContinueStatementSyntax();
            
            
            return result;
        }
        
        public static ConversionOperatorDeclarationSyntax ConversionOperatorDeclaration(IEnumerable<AttributeListSyntax> attributeLists = null, Modifiers modifiers = default(Modifiers), ImplicitOrExplicit kind = default(ImplicitOrExplicit), TypeSyntax type = null, ParameterListSyntax parameterList = null, BlockSyntax body = null)
        {
            var result = new ConversionOperatorDeclarationSyntax();
            
            if (attributeLists != null)
                result.AttributeLists.AddRange(attributeLists);
            result.Modifiers = modifiers;
            result.Kind = kind;
            result.Type = type;
            result.ParameterList = parameterList;
            result.Body = body;
            
            return result;
        }
        
        public static ConversionOperatorDeclarationSyntax ConversionOperatorDeclaration(ImplicitOrExplicit kind = default(ImplicitOrExplicit), TypeSyntax type = null)
        {
            var result = new ConversionOperatorDeclarationSyntax();
            
            result.Kind = kind;
            result.Type = type;
            
            return result;
        }
        
        public static ConversionOperatorDeclarationSyntax ConversionOperatorDeclaration(IEnumerable<AttributeListSyntax> attributeLists = null, Modifiers modifiers = default(Modifiers), ImplicitOrExplicit kind = default(ImplicitOrExplicit), string type = null, ParameterListSyntax parameterList = null, BlockSyntax body = null)
        {
            var result = new ConversionOperatorDeclarationSyntax();
            
            if (attributeLists != null)
                result.AttributeLists.AddRange(attributeLists);
            result.Modifiers = modifiers;
            result.Kind = kind;
            if (type != null)
                result.Type = ParseName(type);
            result.ParameterList = parameterList;
            result.Body = body;
            
            return result;
        }
        
        public static ConversionOperatorDeclarationSyntax ConversionOperatorDeclaration(ImplicitOrExplicit kind = default(ImplicitOrExplicit), string type = null)
        {
            var result = new ConversionOperatorDeclarationSyntax();
            
            result.Kind = kind;
            if (type != null)
                result.Type = ParseName(type);
            
            return result;
        }
        
        public static DefaultExpressionSyntax DefaultExpression(TypeSyntax type = null)
        {
            var result = new DefaultExpressionSyntax();
            
            result.Type = type;
            
            return result;
        }
        
        public static DefaultExpressionSyntax DefaultExpression(string type = null)
        {
            var result = new DefaultExpressionSyntax();
            
            if (type != null)
                result.Type = ParseName(type);
            
            return result;
        }
        
        public static DelegateDeclarationSyntax DelegateDeclaration(IEnumerable<AttributeListSyntax> attributeLists = null, Modifiers modifiers = default(Modifiers), TypeSyntax returnType = null, string identifier = null, TypeParameterListSyntax typeParameterList = null, ParameterListSyntax parameterList = null, IEnumerable<TypeParameterConstraintClauseSyntax> constraintClauses = null)
        {
            var result = new DelegateDeclarationSyntax();
            
            if (attributeLists != null)
                result.AttributeLists.AddRange(attributeLists);
            result.Modifiers = modifiers;
            result.ReturnType = returnType;
            result.Identifier = identifier;
            result.TypeParameterList = typeParameterList;
            result.ParameterList = parameterList;
            if (constraintClauses != null)
                result.ConstraintClauses.AddRange(constraintClauses);
            
            return result;
        }
        
        public static DelegateDeclarationSyntax DelegateDeclaration(TypeSyntax returnType = null, string identifier = null, ParameterListSyntax parameterList = null)
        {
            var result = new DelegateDeclarationSyntax();
            
            result.ReturnType = returnType;
            result.Identifier = identifier;
            result.ParameterList = parameterList;
            
            return result;
        }
        
        public static DelegateDeclarationSyntax DelegateDeclaration(IEnumerable<AttributeListSyntax> attributeLists = null, Modifiers modifiers = default(Modifiers), string returnType = null, string identifier = null, TypeParameterListSyntax typeParameterList = null, ParameterListSyntax parameterList = null, IEnumerable<TypeParameterConstraintClauseSyntax> constraintClauses = null)
        {
            var result = new DelegateDeclarationSyntax();
            
            if (attributeLists != null)
                result.AttributeLists.AddRange(attributeLists);
            result.Modifiers = modifiers;
            if (returnType != null)
                result.ReturnType = ParseName(returnType);
            result.Identifier = identifier;
            result.TypeParameterList = typeParameterList;
            result.ParameterList = parameterList;
            if (constraintClauses != null)
                result.ConstraintClauses.AddRange(constraintClauses);
            
            return result;
        }
        
        public static DelegateDeclarationSyntax DelegateDeclaration(string returnType = null, string identifier = null, ParameterListSyntax parameterList = null)
        {
            var result = new DelegateDeclarationSyntax();
            
            if (returnType != null)
                result.ReturnType = ParseName(returnType);
            result.Identifier = identifier;
            result.ParameterList = parameterList;
            
            return result;
        }
        
        public static DestructorDeclarationSyntax DestructorDeclaration(IEnumerable<AttributeListSyntax> attributeLists = null, Modifiers modifiers = default(Modifiers), string identifier = null, ParameterListSyntax parameterList = null, BlockSyntax body = null)
        {
            var result = new DestructorDeclarationSyntax();
            
            if (attributeLists != null)
                result.AttributeLists.AddRange(attributeLists);
            result.Modifiers = modifiers;
            result.Identifier = identifier;
            result.ParameterList = parameterList;
            result.Body = body;
            
            return result;
        }
        
        public static DestructorDeclarationSyntax DestructorDeclaration(string identifier = null, ParameterListSyntax parameterList = null, BlockSyntax body = null)
        {
            var result = new DestructorDeclarationSyntax();
            
            result.Identifier = identifier;
            result.ParameterList = parameterList;
            result.Body = body;
            
            return result;
        }
        
        public static DoStatementSyntax DoStatement(StatementSyntax statement = null, ExpressionSyntax condition = null)
        {
            var result = new DoStatementSyntax();
            
            result.Statement = statement;
            result.Condition = condition;
            
            return result;
        }
        
        public static ElementAccessExpressionSyntax ElementAccessExpression(ExpressionSyntax expression = null, BracketedArgumentListSyntax argumentList = null)
        {
            var result = new ElementAccessExpressionSyntax();
            
            result.Expression = expression;
            result.ArgumentList = argumentList;
            
            return result;
        }
        
        public static ElseClauseSyntax ElseClause(StatementSyntax statement = null)
        {
            var result = new ElseClauseSyntax();
            
            result.Statement = statement;
            
            return result;
        }
        
        public static EmptyStatementSyntax EmptyStatement()
        {
            var result = new EmptyStatementSyntax();
            
            
            return result;
        }
        
        public static EnumDeclarationSyntax EnumDeclaration(IEnumerable<AttributeListSyntax> attributeLists = null, Modifiers modifiers = default(Modifiers), string identifier = null, BaseListSyntax baseList = null, IEnumerable<EnumMemberDeclarationSyntax> members = null)
        {
            var result = new EnumDeclarationSyntax();
            
            if (attributeLists != null)
                result.AttributeLists.AddRange(attributeLists);
            result.Modifiers = modifiers;
            result.Identifier = identifier;
            result.BaseList = baseList;
            if (members != null)
                result.Members.AddRange(members);
            
            return result;
        }
        
        public static EnumDeclarationSyntax EnumDeclaration(string identifier = null)
        {
            var result = new EnumDeclarationSyntax();
            
            result.Identifier = identifier;
            
            return result;
        }
        
        public static EnumMemberDeclarationSyntax EnumMemberDeclaration(IEnumerable<AttributeListSyntax> attributeLists = null, string identifier = null, EqualsValueClauseSyntax equalsValue = null)
        {
            var result = new EnumMemberDeclarationSyntax();
            
            if (attributeLists != null)
                result.AttributeLists.AddRange(attributeLists);
            result.Identifier = identifier;
            result.EqualsValue = equalsValue;
            
            return result;
        }
        
        public static EnumMemberDeclarationSyntax EnumMemberDeclaration(string identifier = null)
        {
            var result = new EnumMemberDeclarationSyntax();
            
            result.Identifier = identifier;
            
            return result;
        }
        
        public static EqualsValueClauseSyntax EqualsValueClause(ExpressionSyntax value = null)
        {
            var result = new EqualsValueClauseSyntax();
            
            result.Value = value;
            
            return result;
        }
        
        public static EventDeclarationSyntax EventDeclaration(IEnumerable<AttributeListSyntax> attributeLists = null, Modifiers modifiers = default(Modifiers), TypeSyntax type = null, ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier = null, string identifier = null, AccessorListSyntax accessorList = null)
        {
            var result = new EventDeclarationSyntax();
            
            if (attributeLists != null)
                result.AttributeLists.AddRange(attributeLists);
            result.Modifiers = modifiers;
            result.Type = type;
            result.ExplicitInterfaceSpecifier = explicitInterfaceSpecifier;
            result.Identifier = identifier;
            result.AccessorList = accessorList;
            
            return result;
        }
        
        public static EventDeclarationSyntax EventDeclaration(TypeSyntax type = null, string identifier = null)
        {
            var result = new EventDeclarationSyntax();
            
            result.Type = type;
            result.Identifier = identifier;
            
            return result;
        }
        
        public static EventDeclarationSyntax EventDeclaration(IEnumerable<AttributeListSyntax> attributeLists = null, Modifiers modifiers = default(Modifiers), string type = null, ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier = null, string identifier = null, AccessorListSyntax accessorList = null)
        {
            var result = new EventDeclarationSyntax();
            
            if (attributeLists != null)
                result.AttributeLists.AddRange(attributeLists);
            result.Modifiers = modifiers;
            if (type != null)
                result.Type = ParseName(type);
            result.ExplicitInterfaceSpecifier = explicitInterfaceSpecifier;
            result.Identifier = identifier;
            result.AccessorList = accessorList;
            
            return result;
        }
        
        public static EventDeclarationSyntax EventDeclaration(string type = null, string identifier = null)
        {
            var result = new EventDeclarationSyntax();
            
            if (type != null)
                result.Type = ParseName(type);
            result.Identifier = identifier;
            
            return result;
        }
        
        public static EventFieldDeclarationSyntax EventFieldDeclaration(IEnumerable<AttributeListSyntax> attributeLists = null, Modifiers modifiers = default(Modifiers), VariableDeclarationSyntax declaration = null)
        {
            var result = new EventFieldDeclarationSyntax();
            
            if (attributeLists != null)
                result.AttributeLists.AddRange(attributeLists);
            result.Modifiers = modifiers;
            result.Declaration = declaration;
            
            return result;
        }
        
        public static EventFieldDeclarationSyntax EventFieldDeclaration(VariableDeclarationSyntax declaration = null)
        {
            var result = new EventFieldDeclarationSyntax();
            
            result.Declaration = declaration;
            
            return result;
        }
        
        public static ExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier(NameSyntax name = null)
        {
            var result = new ExplicitInterfaceSpecifierSyntax();
            
            result.Name = name;
            
            return result;
        }
        
        public static ExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier(string name = null)
        {
            var result = new ExplicitInterfaceSpecifierSyntax();
            
            if (name != null)
                result.Name = (NameSyntax)ParseName(name);
            
            return result;
        }
        
        public static ExpressionStatementSyntax ExpressionStatement(ExpressionSyntax expression = null)
        {
            var result = new ExpressionStatementSyntax();
            
            result.Expression = expression;
            
            return result;
        }
        
        public static ExternAliasDirectiveSyntax ExternAliasDirective(string identifier = null)
        {
            var result = new ExternAliasDirectiveSyntax();
            
            result.Identifier = identifier;
            
            return result;
        }
        
        public static FieldDeclarationSyntax FieldDeclaration(IEnumerable<AttributeListSyntax> attributeLists = null, Modifiers modifiers = default(Modifiers), VariableDeclarationSyntax declaration = null)
        {
            var result = new FieldDeclarationSyntax();
            
            if (attributeLists != null)
                result.AttributeLists.AddRange(attributeLists);
            result.Modifiers = modifiers;
            result.Declaration = declaration;
            
            return result;
        }
        
        public static FieldDeclarationSyntax FieldDeclaration(VariableDeclarationSyntax declaration = null)
        {
            var result = new FieldDeclarationSyntax();
            
            result.Declaration = declaration;
            
            return result;
        }
        
        public static FinallyClauseSyntax FinallyClause(BlockSyntax block = null)
        {
            var result = new FinallyClauseSyntax();
            
            result.Block = block;
            
            return result;
        }
        
        public static ForEachStatementSyntax ForEachStatement(TypeSyntax type = null, string identifier = null, ExpressionSyntax expression = null, StatementSyntax statement = null)
        {
            var result = new ForEachStatementSyntax();
            
            result.Type = type;
            result.Identifier = identifier;
            result.Expression = expression;
            result.Statement = statement;
            
            return result;
        }
        
        public static ForEachStatementSyntax ForEachStatement(string type = null, string identifier = null, ExpressionSyntax expression = null, StatementSyntax statement = null)
        {
            var result = new ForEachStatementSyntax();
            
            if (type != null)
                result.Type = ParseName(type);
            result.Identifier = identifier;
            result.Expression = expression;
            result.Statement = statement;
            
            return result;
        }
        
        public static ForStatementSyntax ForStatement(VariableDeclarationSyntax declaration = null, IEnumerable<ExpressionSyntax> initializers = null, ExpressionSyntax condition = null, IEnumerable<ExpressionSyntax> incrementors = null, StatementSyntax statement = null)
        {
            var result = new ForStatementSyntax();
            
            result.Declaration = declaration;
            if (initializers != null)
                result.Initializers.AddRange(initializers);
            result.Condition = condition;
            if (incrementors != null)
                result.Incrementors.AddRange(incrementors);
            result.Statement = statement;
            
            return result;
        }
        
        public static ForStatementSyntax ForStatement(StatementSyntax statement = null)
        {
            var result = new ForStatementSyntax();
            
            result.Statement = statement;
            
            return result;
        }
        
        public static FromClauseSyntax FromClause(TypeSyntax type = null, string identifier = null, ExpressionSyntax expression = null)
        {
            var result = new FromClauseSyntax();
            
            result.Type = type;
            result.Identifier = identifier;
            result.Expression = expression;
            
            return result;
        }
        
        public static FromClauseSyntax FromClause(string identifier = null, ExpressionSyntax expression = null)
        {
            var result = new FromClauseSyntax();
            
            result.Identifier = identifier;
            result.Expression = expression;
            
            return result;
        }
        
        public static FromClauseSyntax FromClause(string type = null, string identifier = null, ExpressionSyntax expression = null)
        {
            var result = new FromClauseSyntax();
            
            if (type != null)
                result.Type = ParseName(type);
            result.Identifier = identifier;
            result.Expression = expression;
            
            return result;
        }
        
        public static GenericNameSyntax GenericName(string identifier = null, TypeArgumentListSyntax typeArgumentList = null)
        {
            var result = new GenericNameSyntax();
            
            result.Identifier = identifier;
            result.TypeArgumentList = typeArgumentList;
            
            return result;
        }
        
        public static GotoStatementSyntax GotoStatement(CaseOrDefault? kind = default(CaseOrDefault?), ExpressionSyntax expression = null)
        {
            var result = new GotoStatementSyntax();
            
            result.Kind = kind;
            result.Expression = expression;
            
            return result;
        }
        
        public static GroupClauseSyntax GroupClause(ExpressionSyntax groupExpression = null, ExpressionSyntax byExpression = null)
        {
            var result = new GroupClauseSyntax();
            
            result.GroupExpression = groupExpression;
            result.ByExpression = byExpression;
            
            return result;
        }
        
        public static IdentifierNameSyntax IdentifierName(string identifier = null)
        {
            var result = new IdentifierNameSyntax();
            
            result.Identifier = identifier;
            
            return result;
        }
        
        public static IfStatementSyntax IfStatement(ExpressionSyntax condition = null, StatementSyntax statement = null, ElseClauseSyntax @else = null)
        {
            var result = new IfStatementSyntax();
            
            result.Condition = condition;
            result.Statement = statement;
            result.Else = @else;
            
            return result;
        }
        
        public static ImplicitArrayCreationExpressionSyntax ImplicitArrayCreationExpression(int? commas = default(int?), InitializerExpressionSyntax initializer = null)
        {
            var result = new ImplicitArrayCreationExpressionSyntax();
            
            result.Commas = commas;
            result.Initializer = initializer;
            
            return result;
        }
        
        public static ImplicitArrayCreationExpressionSyntax ImplicitArrayCreationExpression(InitializerExpressionSyntax initializer = null)
        {
            var result = new ImplicitArrayCreationExpressionSyntax();
            
            result.Initializer = initializer;
            
            return result;
        }
        
        public static IndexerDeclarationSyntax IndexerDeclaration(IEnumerable<AttributeListSyntax> attributeLists = null, Modifiers modifiers = default(Modifiers), TypeSyntax type = null, ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier = null, BracketedParameterListSyntax parameterList = null, AccessorListSyntax accessorList = null)
        {
            var result = new IndexerDeclarationSyntax();
            
            if (attributeLists != null)
                result.AttributeLists.AddRange(attributeLists);
            result.Modifiers = modifiers;
            result.Type = type;
            result.ExplicitInterfaceSpecifier = explicitInterfaceSpecifier;
            result.ParameterList = parameterList;
            result.AccessorList = accessorList;
            
            return result;
        }
        
        public static IndexerDeclarationSyntax IndexerDeclaration(TypeSyntax type = null)
        {
            var result = new IndexerDeclarationSyntax();
            
            result.Type = type;
            
            return result;
        }
        
        public static IndexerDeclarationSyntax IndexerDeclaration(IEnumerable<AttributeListSyntax> attributeLists = null, Modifiers modifiers = default(Modifiers), string type = null, ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier = null, BracketedParameterListSyntax parameterList = null, AccessorListSyntax accessorList = null)
        {
            var result = new IndexerDeclarationSyntax();
            
            if (attributeLists != null)
                result.AttributeLists.AddRange(attributeLists);
            result.Modifiers = modifiers;
            if (type != null)
                result.Type = ParseName(type);
            result.ExplicitInterfaceSpecifier = explicitInterfaceSpecifier;
            result.ParameterList = parameterList;
            result.AccessorList = accessorList;
            
            return result;
        }
        
        public static IndexerDeclarationSyntax IndexerDeclaration(string type = null)
        {
            var result = new IndexerDeclarationSyntax();
            
            if (type != null)
                result.Type = ParseName(type);
            
            return result;
        }
        
        public static InitializerExpressionSyntax InitializerExpression(IEnumerable<ExpressionSyntax> expressions = null)
        {
            var result = new InitializerExpressionSyntax();
            
            if (expressions != null)
                result.Expressions.AddRange(expressions);
            
            return result;
        }
        
        public static InitializerExpressionSyntax InitializerExpression(params ExpressionSyntax[] expressions)
        {
            var result = new InitializerExpressionSyntax();
            
            if (expressions != null)
                result.Expressions.AddRange(expressions);
            
            return result;
        }
        
        public static InterfaceDeclarationSyntax InterfaceDeclaration(IEnumerable<AttributeListSyntax> attributeLists = null, Modifiers modifiers = default(Modifiers), string identifier = null, TypeParameterListSyntax typeParameterList = null, BaseListSyntax baseList = null, IEnumerable<TypeParameterConstraintClauseSyntax> constraintClauses = null, IEnumerable<MemberDeclarationSyntax> members = null)
        {
            var result = new InterfaceDeclarationSyntax();
            
            if (attributeLists != null)
                result.AttributeLists.AddRange(attributeLists);
            result.Modifiers = modifiers;
            result.Identifier = identifier;
            result.TypeParameterList = typeParameterList;
            result.BaseList = baseList;
            if (constraintClauses != null)
                result.ConstraintClauses.AddRange(constraintClauses);
            if (members != null)
                result.Members.AddRange(members);
            
            return result;
        }
        
        public static InterfaceDeclarationSyntax InterfaceDeclaration(string identifier = null)
        {
            var result = new InterfaceDeclarationSyntax();
            
            result.Identifier = identifier;
            
            return result;
        }
        
        public static InvocationExpressionSyntax InvocationExpression(ExpressionSyntax expression = null, ArgumentListSyntax argumentList = null)
        {
            var result = new InvocationExpressionSyntax();
            
            result.Expression = expression;
            result.ArgumentList = argumentList;
            
            return result;
        }
        
        public static JoinClauseSyntax JoinClause(TypeSyntax type = null, string identifier = null, ExpressionSyntax inExpression = null, ExpressionSyntax leftExpression = null, ExpressionSyntax rightExpression = null, JoinIntoClauseSyntax into = null)
        {
            var result = new JoinClauseSyntax();
            
            result.Type = type;
            result.Identifier = identifier;
            result.InExpression = inExpression;
            result.LeftExpression = leftExpression;
            result.RightExpression = rightExpression;
            result.Into = into;
            
            return result;
        }
        
        public static JoinClauseSyntax JoinClause(string identifier = null, ExpressionSyntax inExpression = null, ExpressionSyntax leftExpression = null, ExpressionSyntax rightExpression = null)
        {
            var result = new JoinClauseSyntax();
            
            result.Identifier = identifier;
            result.InExpression = inExpression;
            result.LeftExpression = leftExpression;
            result.RightExpression = rightExpression;
            
            return result;
        }
        
        public static JoinClauseSyntax JoinClause(string type = null, string identifier = null, ExpressionSyntax inExpression = null, ExpressionSyntax leftExpression = null, ExpressionSyntax rightExpression = null, JoinIntoClauseSyntax into = null)
        {
            var result = new JoinClauseSyntax();
            
            if (type != null)
                result.Type = ParseName(type);
            result.Identifier = identifier;
            result.InExpression = inExpression;
            result.LeftExpression = leftExpression;
            result.RightExpression = rightExpression;
            result.Into = into;
            
            return result;
        }
        
        public static JoinIntoClauseSyntax JoinIntoClause(string identifier = null)
        {
            var result = new JoinIntoClauseSyntax();
            
            result.Identifier = identifier;
            
            return result;
        }
        
        public static LabeledStatementSyntax LabeledStatement(string identifier = null, StatementSyntax statement = null)
        {
            var result = new LabeledStatementSyntax();
            
            result.Identifier = identifier;
            result.Statement = statement;
            
            return result;
        }
        
        public static LetClauseSyntax LetClause(string identifier = null, ExpressionSyntax expression = null)
        {
            var result = new LetClauseSyntax();
            
            result.Identifier = identifier;
            result.Expression = expression;
            
            return result;
        }
        
        public static LiteralExpressionSyntax LiteralExpression(object value = null)
        {
            var result = new LiteralExpressionSyntax();
            
            result.Value = value;
            
            return result;
        }
        
        public static LocalDeclarationStatementSyntax LocalDeclarationStatement(Modifiers modifiers = default(Modifiers), VariableDeclarationSyntax declaration = null)
        {
            var result = new LocalDeclarationStatementSyntax();
            
            result.Modifiers = modifiers;
            result.Declaration = declaration;
            
            return result;
        }
        
        public static LocalDeclarationStatementSyntax LocalDeclarationStatement(VariableDeclarationSyntax declaration = null)
        {
            var result = new LocalDeclarationStatementSyntax();
            
            result.Declaration = declaration;
            
            return result;
        }
        
        public static LockStatementSyntax LockStatement(ExpressionSyntax expression = null, StatementSyntax statement = null)
        {
            var result = new LockStatementSyntax();
            
            result.Expression = expression;
            result.Statement = statement;
            
            return result;
        }
        
        public static MemberAccessExpressionSyntax MemberAccessExpression(ExpressionSyntax expression = null, SimpleNameSyntax name = null)
        {
            var result = new MemberAccessExpressionSyntax();
            
            result.Expression = expression;
            result.Name = name;
            
            return result;
        }
        
        public static MemberAccessExpressionSyntax MemberAccessExpression(ExpressionSyntax expression = null, string name = null)
        {
            var result = new MemberAccessExpressionSyntax();
            
            result.Expression = expression;
            if (name != null)
                result.Name = (SimpleNameSyntax)ParseName(name);
            
            return result;
        }
        
        public static MethodDeclarationSyntax MethodDeclaration(IEnumerable<AttributeListSyntax> attributeLists = null, Modifiers modifiers = default(Modifiers), TypeSyntax returnType = null, ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier = null, string identifier = null, TypeParameterListSyntax typeParameterList = null, ParameterListSyntax parameterList = null, IEnumerable<TypeParameterConstraintClauseSyntax> constraintClauses = null, BlockSyntax body = null)
        {
            var result = new MethodDeclarationSyntax();
            
            if (attributeLists != null)
                result.AttributeLists.AddRange(attributeLists);
            result.Modifiers = modifiers;
            result.ReturnType = returnType;
            result.ExplicitInterfaceSpecifier = explicitInterfaceSpecifier;
            result.Identifier = identifier;
            result.TypeParameterList = typeParameterList;
            result.ParameterList = parameterList;
            if (constraintClauses != null)
                result.ConstraintClauses.AddRange(constraintClauses);
            result.Body = body;
            
            return result;
        }
        
        public static MethodDeclarationSyntax MethodDeclaration(TypeSyntax returnType = null, string identifier = null, ParameterListSyntax parameterList = null, BlockSyntax body = null)
        {
            var result = new MethodDeclarationSyntax();
            
            result.ReturnType = returnType;
            result.Identifier = identifier;
            result.ParameterList = parameterList;
            result.Body = body;
            
            return result;
        }
        
        public static MethodDeclarationSyntax MethodDeclaration(IEnumerable<AttributeListSyntax> attributeLists = null, Modifiers modifiers = default(Modifiers), string returnType = null, ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier = null, string identifier = null, TypeParameterListSyntax typeParameterList = null, ParameterListSyntax parameterList = null, IEnumerable<TypeParameterConstraintClauseSyntax> constraintClauses = null, BlockSyntax body = null)
        {
            var result = new MethodDeclarationSyntax();
            
            if (attributeLists != null)
                result.AttributeLists.AddRange(attributeLists);
            result.Modifiers = modifiers;
            if (returnType != null)
                result.ReturnType = ParseName(returnType);
            result.ExplicitInterfaceSpecifier = explicitInterfaceSpecifier;
            result.Identifier = identifier;
            result.TypeParameterList = typeParameterList;
            result.ParameterList = parameterList;
            if (constraintClauses != null)
                result.ConstraintClauses.AddRange(constraintClauses);
            result.Body = body;
            
            return result;
        }
        
        public static MethodDeclarationSyntax MethodDeclaration(string returnType = null, string identifier = null, ParameterListSyntax parameterList = null, BlockSyntax body = null)
        {
            var result = new MethodDeclarationSyntax();
            
            if (returnType != null)
                result.ReturnType = ParseName(returnType);
            result.Identifier = identifier;
            result.ParameterList = parameterList;
            result.Body = body;
            
            return result;
        }
        
        public static NameColonSyntax NameColon(IdentifierNameSyntax name = null)
        {
            var result = new NameColonSyntax();
            
            result.Name = name;
            
            return result;
        }
        
        public static NameColonSyntax NameColon(string name = null)
        {
            var result = new NameColonSyntax();
            
            if (name != null)
                result.Name = (IdentifierNameSyntax)ParseName(name);
            
            return result;
        }
        
        public static NameEqualsSyntax NameEquals(IdentifierNameSyntax name = null)
        {
            var result = new NameEqualsSyntax();
            
            result.Name = name;
            
            return result;
        }
        
        public static NameEqualsSyntax NameEquals(string name = null)
        {
            var result = new NameEqualsSyntax();
            
            if (name != null)
                result.Name = (IdentifierNameSyntax)ParseName(name);
            
            return result;
        }
        
        public static NamespaceDeclarationSyntax NamespaceDeclaration(NameSyntax name = null, IEnumerable<ExternAliasDirectiveSyntax> externs = null, IEnumerable<UsingDirectiveSyntax> usings = null, IEnumerable<MemberDeclarationSyntax> members = null)
        {
            var result = new NamespaceDeclarationSyntax();
            
            result.Name = name;
            if (externs != null)
                result.Externs.AddRange(externs);
            if (usings != null)
                result.Usings.AddRange(usings);
            if (members != null)
                result.Members.AddRange(members);
            
            return result;
        }
        
        public static NamespaceDeclarationSyntax NamespaceDeclaration(string name = null, IEnumerable<ExternAliasDirectiveSyntax> externs = null, IEnumerable<UsingDirectiveSyntax> usings = null, IEnumerable<MemberDeclarationSyntax> members = null)
        {
            var result = new NamespaceDeclarationSyntax();
            
            if (name != null)
                result.Name = (NameSyntax)ParseName(name);
            if (externs != null)
                result.Externs.AddRange(externs);
            if (usings != null)
                result.Usings.AddRange(usings);
            if (members != null)
                result.Members.AddRange(members);
            
            return result;
        }
        
        public static NullableTypeSyntax NullableType(TypeSyntax elementType = null)
        {
            var result = new NullableTypeSyntax();
            
            result.ElementType = elementType;
            
            return result;
        }
        
        public static NullableTypeSyntax NullableType(string elementType = null)
        {
            var result = new NullableTypeSyntax();
            
            if (elementType != null)
                result.ElementType = ParseName(elementType);
            
            return result;
        }
        
        public static ObjectCreationExpressionSyntax ObjectCreationExpression(TypeSyntax type = null, ArgumentListSyntax argumentList = null, InitializerExpressionSyntax initializer = null)
        {
            var result = new ObjectCreationExpressionSyntax();
            
            result.Type = type;
            result.ArgumentList = argumentList;
            result.Initializer = initializer;
            
            return result;
        }
        
        public static ObjectCreationExpressionSyntax ObjectCreationExpression(string type = null, ArgumentListSyntax argumentList = null, InitializerExpressionSyntax initializer = null)
        {
            var result = new ObjectCreationExpressionSyntax();
            
            if (type != null)
                result.Type = ParseName(type);
            result.ArgumentList = argumentList;
            result.Initializer = initializer;
            
            return result;
        }
        
        public static OmittedArraySizeExpressionSyntax OmittedArraySizeExpression()
        {
            var result = new OmittedArraySizeExpressionSyntax();
            
            
            return result;
        }
        
        public static OmittedTypeArgumentSyntax OmittedTypeArgument()
        {
            var result = new OmittedTypeArgumentSyntax();
            
            
            return result;
        }
        
        public static OperatorDeclarationSyntax OperatorDeclaration(IEnumerable<AttributeListSyntax> attributeLists = null, Modifiers modifiers = default(Modifiers), TypeSyntax returnType = null, Operator @operator = default(Operator), ParameterListSyntax parameterList = null, BlockSyntax body = null)
        {
            var result = new OperatorDeclarationSyntax();
            
            if (attributeLists != null)
                result.AttributeLists.AddRange(attributeLists);
            result.Modifiers = modifiers;
            result.ReturnType = returnType;
            result.Operator = @operator;
            result.ParameterList = parameterList;
            result.Body = body;
            
            return result;
        }
        
        public static OperatorDeclarationSyntax OperatorDeclaration(TypeSyntax returnType = null, Operator @operator = default(Operator))
        {
            var result = new OperatorDeclarationSyntax();
            
            result.ReturnType = returnType;
            result.Operator = @operator;
            
            return result;
        }
        
        public static OperatorDeclarationSyntax OperatorDeclaration(IEnumerable<AttributeListSyntax> attributeLists = null, Modifiers modifiers = default(Modifiers), string returnType = null, Operator @operator = default(Operator), ParameterListSyntax parameterList = null, BlockSyntax body = null)
        {
            var result = new OperatorDeclarationSyntax();
            
            if (attributeLists != null)
                result.AttributeLists.AddRange(attributeLists);
            result.Modifiers = modifiers;
            if (returnType != null)
                result.ReturnType = ParseName(returnType);
            result.Operator = @operator;
            result.ParameterList = parameterList;
            result.Body = body;
            
            return result;
        }
        
        public static OperatorDeclarationSyntax OperatorDeclaration(string returnType = null, Operator @operator = default(Operator))
        {
            var result = new OperatorDeclarationSyntax();
            
            if (returnType != null)
                result.ReturnType = ParseName(returnType);
            result.Operator = @operator;
            
            return result;
        }
        
        public static OrderByClauseSyntax OrderByClause(IEnumerable<OrderingSyntax> orderings = null)
        {
            var result = new OrderByClauseSyntax();
            
            if (orderings != null)
                result.Orderings.AddRange(orderings);
            
            return result;
        }
        
        public static OrderByClauseSyntax OrderByClause(params OrderingSyntax[] orderings)
        {
            var result = new OrderByClauseSyntax();
            
            if (orderings != null)
                result.Orderings.AddRange(orderings);
            
            return result;
        }
        
        public static OrderingSyntax Ordering(AscendingOrDescending kind = default(AscendingOrDescending), ExpressionSyntax expression = null)
        {
            var result = new OrderingSyntax();
            
            result.Kind = kind;
            result.Expression = expression;
            
            return result;
        }
        
        public static ParameterListSyntax ParameterList(IEnumerable<ParameterSyntax> parameters = null)
        {
            var result = new ParameterListSyntax();
            
            if (parameters != null)
                result.Parameters.AddRange(parameters);
            
            return result;
        }
        
        public static ParameterListSyntax ParameterList(params ParameterSyntax[] parameters)
        {
            var result = new ParameterListSyntax();
            
            if (parameters != null)
                result.Parameters.AddRange(parameters);
            
            return result;
        }
        
        public static ParameterSyntax Parameter(IEnumerable<AttributeListSyntax> attributeLists = null, ParameterModifier modifier = default(ParameterModifier), TypeSyntax type = null, string identifier = null, EqualsValueClauseSyntax @default = null)
        {
            var result = new ParameterSyntax();
            
            if (attributeLists != null)
                result.AttributeLists.AddRange(attributeLists);
            result.Modifier = modifier;
            result.Type = type;
            result.Identifier = identifier;
            result.Default = @default;
            
            return result;
        }
        
        public static ParameterSyntax Parameter(string identifier = null)
        {
            var result = new ParameterSyntax();
            
            result.Identifier = identifier;
            
            return result;
        }
        
        public static ParameterSyntax Parameter(IEnumerable<AttributeListSyntax> attributeLists = null, ParameterModifier modifier = default(ParameterModifier), string type = null, string identifier = null, EqualsValueClauseSyntax @default = null)
        {
            var result = new ParameterSyntax();
            
            if (attributeLists != null)
                result.AttributeLists.AddRange(attributeLists);
            result.Modifier = modifier;
            if (type != null)
                result.Type = ParseName(type);
            result.Identifier = identifier;
            result.Default = @default;
            
            return result;
        }
        
        public static ParenthesizedExpressionSyntax ParenthesizedExpression(ExpressionSyntax expression = null)
        {
            var result = new ParenthesizedExpressionSyntax();
            
            result.Expression = expression;
            
            return result;
        }
        
        public static ParenthesizedLambdaExpressionSyntax ParenthesizedLambdaExpression(ParameterListSyntax parameterList = null, SyntaxNode body = null)
        {
            var result = new ParenthesizedLambdaExpressionSyntax();
            
            result.ParameterList = parameterList;
            result.Body = body;
            
            return result;
        }
        
        public static ParenthesizedLambdaExpressionSyntax ParenthesizedLambdaExpression(Modifiers modifiers = default(Modifiers), ParameterListSyntax parameterList = null, SyntaxNode body = null)
        {
            var result = new ParenthesizedLambdaExpressionSyntax();
            
            result.Modifiers = modifiers;
            result.ParameterList = parameterList;
            result.Body = body;
            
            return result;
        }
        
        public static ParenthesizedLambdaExpressionSyntax ParenthesizedLambdaExpression(SyntaxNode body = null)
        {
            var result = new ParenthesizedLambdaExpressionSyntax();
            
            result.Body = body;
            
            return result;
        }
        
        public static PostfixUnaryExpressionSyntax PostfixUnaryExpression(PostfixUnaryOperator @operator = default(PostfixUnaryOperator), ExpressionSyntax operand = null)
        {
            var result = new PostfixUnaryExpressionSyntax();
            
            result.Operator = @operator;
            result.Operand = operand;
            
            return result;
        }
        
        public static PredefinedTypeSyntax PredefinedType()
        {
            var result = new PredefinedTypeSyntax();
            
            
            return result;
        }
        
        public static PrefixUnaryExpressionSyntax PrefixUnaryExpression(PrefixUnaryOperator @operator = default(PrefixUnaryOperator), ExpressionSyntax operand = null)
        {
            var result = new PrefixUnaryExpressionSyntax();
            
            result.Operator = @operator;
            result.Operand = operand;
            
            return result;
        }
        
        public static PropertyDeclarationSyntax PropertyDeclaration(IEnumerable<AttributeListSyntax> attributeLists = null, Modifiers modifiers = default(Modifiers), TypeSyntax type = null, ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier = null, string identifier = null, AccessorListSyntax accessorList = null)
        {
            var result = new PropertyDeclarationSyntax();
            
            if (attributeLists != null)
                result.AttributeLists.AddRange(attributeLists);
            result.Modifiers = modifiers;
            result.Type = type;
            result.ExplicitInterfaceSpecifier = explicitInterfaceSpecifier;
            result.Identifier = identifier;
            result.AccessorList = accessorList;
            
            return result;
        }
        
        public static PropertyDeclarationSyntax PropertyDeclaration(TypeSyntax type = null, string identifier = null)
        {
            var result = new PropertyDeclarationSyntax();
            
            result.Type = type;
            result.Identifier = identifier;
            
            return result;
        }
        
        public static PropertyDeclarationSyntax PropertyDeclaration(IEnumerable<AttributeListSyntax> attributeLists = null, Modifiers modifiers = default(Modifiers), string type = null, ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier = null, string identifier = null, AccessorListSyntax accessorList = null)
        {
            var result = new PropertyDeclarationSyntax();
            
            if (attributeLists != null)
                result.AttributeLists.AddRange(attributeLists);
            result.Modifiers = modifiers;
            if (type != null)
                result.Type = ParseName(type);
            result.ExplicitInterfaceSpecifier = explicitInterfaceSpecifier;
            result.Identifier = identifier;
            result.AccessorList = accessorList;
            
            return result;
        }
        
        public static PropertyDeclarationSyntax PropertyDeclaration(string type = null, string identifier = null)
        {
            var result = new PropertyDeclarationSyntax();
            
            if (type != null)
                result.Type = ParseName(type);
            result.Identifier = identifier;
            
            return result;
        }
        
        public static QualifiedNameSyntax QualifiedName(NameSyntax left = null, SimpleNameSyntax right = null)
        {
            var result = new QualifiedNameSyntax();
            
            result.Left = left;
            result.Right = right;
            
            return result;
        }
        
        public static QualifiedNameSyntax QualifiedName(string left = null, string right = null)
        {
            var result = new QualifiedNameSyntax();
            
            if (left != null)
                result.Left = (NameSyntax)ParseName(left);
            if (right != null)
                result.Right = (SimpleNameSyntax)ParseName(right);
            
            return result;
        }
        
        public static QueryBodySyntax QueryBody(IEnumerable<QueryClauseSyntax> clauses = null, SelectOrGroupClauseSyntax selectOrGroup = null, QueryContinuationSyntax continuation = null)
        {
            var result = new QueryBodySyntax();
            
            if (clauses != null)
                result.Clauses.AddRange(clauses);
            result.SelectOrGroup = selectOrGroup;
            result.Continuation = continuation;
            
            return result;
        }
        
        public static QueryBodySyntax QueryBody(SelectOrGroupClauseSyntax selectOrGroup = null)
        {
            var result = new QueryBodySyntax();
            
            result.SelectOrGroup = selectOrGroup;
            
            return result;
        }
        
        public static QueryContinuationSyntax QueryContinuation(string identifier = null, QueryBodySyntax body = null)
        {
            var result = new QueryContinuationSyntax();
            
            result.Identifier = identifier;
            result.Body = body;
            
            return result;
        }
        
        public static QueryExpressionSyntax QueryExpression(FromClauseSyntax fromClause = null, QueryBodySyntax body = null)
        {
            var result = new QueryExpressionSyntax();
            
            result.FromClause = fromClause;
            result.Body = body;
            
            return result;
        }
        
        public static ReturnStatementSyntax ReturnStatement(ExpressionSyntax expression = null)
        {
            var result = new ReturnStatementSyntax();
            
            result.Expression = expression;
            
            return result;
        }
        
        public static SelectClauseSyntax SelectClause(ExpressionSyntax expression = null)
        {
            var result = new SelectClauseSyntax();
            
            result.Expression = expression;
            
            return result;
        }
        
        public static SimpleLambdaExpressionSyntax SimpleLambdaExpression(ParameterSyntax parameter = null, SyntaxNode body = null)
        {
            var result = new SimpleLambdaExpressionSyntax();
            
            result.Parameter = parameter;
            result.Body = body;
            
            return result;
        }
        
        public static SimpleLambdaExpressionSyntax SimpleLambdaExpression(Modifiers modifiers = default(Modifiers), ParameterSyntax parameter = null, SyntaxNode body = null)
        {
            var result = new SimpleLambdaExpressionSyntax();
            
            result.Modifiers = modifiers;
            result.Parameter = parameter;
            result.Body = body;
            
            return result;
        }
        
        public static SizeOfExpressionSyntax SizeOfExpression(TypeSyntax type = null)
        {
            var result = new SizeOfExpressionSyntax();
            
            result.Type = type;
            
            return result;
        }
        
        public static SizeOfExpressionSyntax SizeOfExpression(string type = null)
        {
            var result = new SizeOfExpressionSyntax();
            
            if (type != null)
                result.Type = ParseName(type);
            
            return result;
        }
        
        public static StructDeclarationSyntax StructDeclaration(IEnumerable<AttributeListSyntax> attributeLists = null, Modifiers modifiers = default(Modifiers), string identifier = null, TypeParameterListSyntax typeParameterList = null, BaseListSyntax baseList = null, IEnumerable<TypeParameterConstraintClauseSyntax> constraintClauses = null, IEnumerable<MemberDeclarationSyntax> members = null)
        {
            var result = new StructDeclarationSyntax();
            
            if (attributeLists != null)
                result.AttributeLists.AddRange(attributeLists);
            result.Modifiers = modifiers;
            result.Identifier = identifier;
            result.TypeParameterList = typeParameterList;
            result.BaseList = baseList;
            if (constraintClauses != null)
                result.ConstraintClauses.AddRange(constraintClauses);
            if (members != null)
                result.Members.AddRange(members);
            
            return result;
        }
        
        public static StructDeclarationSyntax StructDeclaration(string identifier = null)
        {
            var result = new StructDeclarationSyntax();
            
            result.Identifier = identifier;
            
            return result;
        }
        
        public static SwitchLabelSyntax SwitchLabel(CaseOrDefault kind = default(CaseOrDefault), ExpressionSyntax value = null)
        {
            var result = new SwitchLabelSyntax();
            
            result.Kind = kind;
            result.Value = value;
            
            return result;
        }
        
        public static SwitchSectionSyntax SwitchSection(IEnumerable<SwitchLabelSyntax> labels = null, IEnumerable<StatementSyntax> statements = null)
        {
            var result = new SwitchSectionSyntax();
            
            if (labels != null)
                result.Labels.AddRange(labels);
            if (statements != null)
                result.Statements.AddRange(statements);
            
            return result;
        }
        
        public static SwitchStatementSyntax SwitchStatement(ExpressionSyntax expression = null, IEnumerable<SwitchSectionSyntax> sections = null)
        {
            var result = new SwitchStatementSyntax();
            
            result.Expression = expression;
            if (sections != null)
                result.Sections.AddRange(sections);
            
            return result;
        }
        
        public static ThisExpressionSyntax ThisExpression()
        {
            var result = new ThisExpressionSyntax();
            
            
            return result;
        }
        
        public static ThrowStatementSyntax ThrowStatement(ExpressionSyntax expression = null)
        {
            var result = new ThrowStatementSyntax();
            
            result.Expression = expression;
            
            return result;
        }
        
        public static TryStatementSyntax TryStatement(BlockSyntax block = null, IEnumerable<CatchClauseSyntax> catches = null, FinallyClauseSyntax @finally = null)
        {
            var result = new TryStatementSyntax();
            
            result.Block = block;
            if (catches != null)
                result.Catches.AddRange(catches);
            result.Finally = @finally;
            
            return result;
        }
        
        public static TryStatementSyntax TryStatement(IEnumerable<CatchClauseSyntax> catches = null)
        {
            var result = new TryStatementSyntax();
            
            if (catches != null)
                result.Catches.AddRange(catches);
            
            return result;
        }
        
        public static TryStatementSyntax TryStatement(params CatchClauseSyntax[] catches)
        {
            var result = new TryStatementSyntax();
            
            if (catches != null)
                result.Catches.AddRange(catches);
            
            return result;
        }
        
        public static TypeArgumentListSyntax TypeArgumentList(IEnumerable<TypeSyntax> arguments = null)
        {
            var result = new TypeArgumentListSyntax();
            
            if (arguments != null)
                result.Arguments.AddRange(arguments);
            
            return result;
        }
        
        public static TypeArgumentListSyntax TypeArgumentList(params TypeSyntax[] arguments)
        {
            var result = new TypeArgumentListSyntax();
            
            if (arguments != null)
                result.Arguments.AddRange(arguments);
            
            return result;
        }
        
        public static TypeArgumentListSyntax TypeArgumentList(IEnumerable<string> arguments = null)
        {
            var result = new TypeArgumentListSyntax();
            
            if (arguments != null)
                result.Arguments.AddRange(ParseNames<TypeSyntax>(arguments));
            
            return result;
        }
        
        public static TypeArgumentListSyntax TypeArgumentList(params string[] arguments)
        {
            var result = new TypeArgumentListSyntax();
            
            if (arguments != null)
                result.Arguments.AddRange(ParseNames<TypeSyntax>(arguments));
            
            return result;
        }
        
        public static TypeConstraintSyntax TypeConstraint(TypeSyntax type = null)
        {
            var result = new TypeConstraintSyntax();
            
            result.Type = type;
            
            return result;
        }
        
        public static TypeConstraintSyntax TypeConstraint(string type = null)
        {
            var result = new TypeConstraintSyntax();
            
            if (type != null)
                result.Type = ParseName(type);
            
            return result;
        }
        
        public static TypeOfExpressionSyntax TypeOfExpression(TypeSyntax type = null)
        {
            var result = new TypeOfExpressionSyntax();
            
            result.Type = type;
            
            return result;
        }
        
        public static TypeOfExpressionSyntax TypeOfExpression(string type = null)
        {
            var result = new TypeOfExpressionSyntax();
            
            if (type != null)
                result.Type = ParseName(type);
            
            return result;
        }
        
        public static TypeParameterConstraintClauseSyntax TypeParameterConstraintClause(IdentifierNameSyntax name = null, IEnumerable<TypeParameterConstraintSyntax> constraints = null)
        {
            var result = new TypeParameterConstraintClauseSyntax();
            
            result.Name = name;
            if (constraints != null)
                result.Constraints.AddRange(constraints);
            
            return result;
        }
        
        public static TypeParameterConstraintClauseSyntax TypeParameterConstraintClause(string name = null, IEnumerable<TypeParameterConstraintSyntax> constraints = null)
        {
            var result = new TypeParameterConstraintClauseSyntax();
            
            if (name != null)
                result.Name = (IdentifierNameSyntax)ParseName(name);
            if (constraints != null)
                result.Constraints.AddRange(constraints);
            
            return result;
        }
        
        public static TypeParameterListSyntax TypeParameterList(IEnumerable<TypeParameterSyntax> parameters = null)
        {
            var result = new TypeParameterListSyntax();
            
            if (parameters != null)
                result.Parameters.AddRange(parameters);
            
            return result;
        }
        
        public static TypeParameterListSyntax TypeParameterList(params TypeParameterSyntax[] parameters)
        {
            var result = new TypeParameterListSyntax();
            
            if (parameters != null)
                result.Parameters.AddRange(parameters);
            
            return result;
        }
        
        public static TypeParameterSyntax TypeParameter(IEnumerable<AttributeListSyntax> attributeLists = null, string identifier = null)
        {
            var result = new TypeParameterSyntax();
            
            if (attributeLists != null)
                result.AttributeLists.AddRange(attributeLists);
            result.Identifier = identifier;
            
            return result;
        }
        
        public static TypeParameterSyntax TypeParameter(string identifier = null)
        {
            var result = new TypeParameterSyntax();
            
            result.Identifier = identifier;
            
            return result;
        }
        
        public static UsingDirectiveSyntax UsingDirective(NameEqualsSyntax alias = null, NameSyntax name = null)
        {
            var result = new UsingDirectiveSyntax();
            
            result.Alias = alias;
            result.Name = name;
            
            return result;
        }
        
        public static UsingDirectiveSyntax UsingDirective(NameSyntax name = null)
        {
            var result = new UsingDirectiveSyntax();
            
            result.Name = name;
            
            return result;
        }
        
        public static UsingDirectiveSyntax UsingDirective(NameEqualsSyntax alias = null, string name = null)
        {
            var result = new UsingDirectiveSyntax();
            
            result.Alias = alias;
            if (name != null)
                result.Name = (NameSyntax)ParseName(name);
            
            return result;
        }
        
        public static UsingDirectiveSyntax UsingDirective(string name = null)
        {
            var result = new UsingDirectiveSyntax();
            
            if (name != null)
                result.Name = (NameSyntax)ParseName(name);
            
            return result;
        }
        
        public static UsingStatementSyntax UsingStatement(VariableDeclarationSyntax declaration = null, ExpressionSyntax expression = null, StatementSyntax statement = null)
        {
            var result = new UsingStatementSyntax();
            
            result.Declaration = declaration;
            result.Expression = expression;
            result.Statement = statement;
            
            return result;
        }
        
        public static UsingStatementSyntax UsingStatement(StatementSyntax statement = null)
        {
            var result = new UsingStatementSyntax();
            
            result.Statement = statement;
            
            return result;
        }
        
        public static VariableDeclarationSyntax VariableDeclaration(TypeSyntax type = null, IEnumerable<VariableDeclaratorSyntax> variables = null)
        {
            var result = new VariableDeclarationSyntax();
            
            result.Type = type;
            if (variables != null)
                result.Variables.AddRange(variables);
            
            return result;
        }
        
        public static VariableDeclarationSyntax VariableDeclaration(string type = null, IEnumerable<VariableDeclaratorSyntax> variables = null)
        {
            var result = new VariableDeclarationSyntax();
            
            if (type != null)
                result.Type = ParseName(type);
            if (variables != null)
                result.Variables.AddRange(variables);
            
            return result;
        }
        
        public static VariableDeclaratorSyntax VariableDeclarator(string identifier = null, BracketedArgumentListSyntax argumentList = null, EqualsValueClauseSyntax initializer = null)
        {
            var result = new VariableDeclaratorSyntax();
            
            result.Identifier = identifier;
            result.ArgumentList = argumentList;
            result.Initializer = initializer;
            
            return result;
        }
        
        public static WhereClauseSyntax WhereClause(ExpressionSyntax condition = null)
        {
            var result = new WhereClauseSyntax();
            
            result.Condition = condition;
            
            return result;
        }
        
        public static WhileStatementSyntax WhileStatement(ExpressionSyntax condition = null, StatementSyntax statement = null)
        {
            var result = new WhileStatementSyntax();
            
            result.Condition = condition;
            result.Statement = statement;
            
            return result;
        }
        
        public static YieldStatementSyntax YieldStatement(ReturnOrBreak kind = default(ReturnOrBreak), ExpressionSyntax expression = null)
        {
            var result = new YieldStatementSyntax();
            
            result.Kind = kind;
            result.Expression = expression;
            
            return result;
        }
    }
}
