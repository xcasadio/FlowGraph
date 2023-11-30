namespace DotNetCodeGenerator.Ast;

public class ClassDeclaration : Statement
{
    public string? Inhereted;
    public Token Name;
    public Scope Body;

    public ClassDeclaration(string name, string? inhereted = null, Scope? body = null)
    {
        Inhereted = inhereted;
        Name = new Token(TokenType.Class, "public class", name);
        Body = body ?? new Scope();
    }

    public override void Accept(IStatementVisitor statementVisitor)
    {
        statementVisitor.VisitStatement(this);
    }
}