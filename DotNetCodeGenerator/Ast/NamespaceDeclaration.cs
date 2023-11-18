namespace DotNetCodeGenerator.Ast;

public class NamespaceDeclaration : Statement
{
    public NamespaceDeclaration(string name)
    {
        Name = new Token(TokenType.Namespace, "namespace ", name);
    }

    public override void Accept(IStatementVisitor statementVisitor)
    {
        statementVisitor.VisitStatement(this);
    }

    public Token Name;
}