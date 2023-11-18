namespace DotNetCodeGenerator.Ast;

public class FunctionDeclaration : Statement
{
    public readonly Token Name;
    public readonly List<Token> Parameters = new();
    public readonly Scope Body;

    public FunctionDeclaration(string name, IEnumerable<Token> parameters, Scope? body = null)
    {
        Name = new Token(TokenType.Function, "void", name);
        Parameters.AddRange(parameters);
        Body = body ?? new Scope();
    }

    public override void Accept(IStatementVisitor statementVisitor)
    {
        statementVisitor.VisitStatement(this);
    }
}