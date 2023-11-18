namespace DotNetCodeGenerator.Ast;

public class FunctionCall : Statement
{
    public readonly Token Name;
    public readonly List<Statement> Parameters = new();

    public FunctionCall(string name, IEnumerable<Statement> parameters)
    {
        Name = new Token(TokenType.Function, "", name);
        Parameters.AddRange(parameters);
    }

    public override void Accept(IStatementVisitor visitor)
    {
        visitor.VisitStatement(this);
    }
}