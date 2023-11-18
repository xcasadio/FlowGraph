namespace DotNetCodeGenerator.Ast;

public class ClassDeclaration : Statement
{
    public Token Name;
    public Scope Body;

    public ClassDeclaration(string name, Scope? body = null)
    {
        Name = new Token(TokenType.Class, "public class", name);
        Body = body ?? new Scope();
    }

    public override void Accept(IStatementVisitor statementVisitor)
    {
        statementVisitor.VisitStatement(this);
    }
}