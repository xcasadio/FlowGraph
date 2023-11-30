namespace DotNetCodeGenerator.Ast;

public interface IStatementVisitor
{
    void VisitStatement(NamespaceDeclaration statement);
    void VisitStatement(UsingDeclaration statement);
    void VisitStatement(ClassDeclaration statement);
    void VisitStatement(FunctionDeclaration statement);
    void VisitStatement(PropertyDeclaration statement);

    void VisitStatement(FunctionCall statement);

    void VisitStatement(Block statement);
    void VisitStatement(Scope statement);
    void VisitStatement(StatementExpression statement);
    void VisitStatement(If statement);
    void VisitStatement(VariableStatement statement);
    void VisitStatement(While statement);
    void VisitStatement(Break statement);
    void VisitStatement(LiteralAccessor statement);
}