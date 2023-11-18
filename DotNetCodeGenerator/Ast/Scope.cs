namespace DotNetCodeGenerator.Ast;

public class Scope : Statement
{
    public Scope(List<Statement>? statements = null)
    {
        Statements = statements ?? new List<Statement>();
    }

    public override void Accept(IStatementVisitor statementVisitor)
    {
        statementVisitor.VisitStatement(this);
    }

    public readonly List<Statement> Statements;
}