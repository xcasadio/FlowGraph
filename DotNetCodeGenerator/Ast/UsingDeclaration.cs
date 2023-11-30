namespace DotNetCodeGenerator.Ast;

public class UsingDeclaration : Statement
{
    public UsingDeclaration(string name)
    {
        Name = new Token(TokenType.Namespace, "using ", name);
    }

    public override void Accept(IStatementVisitor statementVisitor)
    {
        statementVisitor.VisitStatement(this);
    }

    public Token Name;
}