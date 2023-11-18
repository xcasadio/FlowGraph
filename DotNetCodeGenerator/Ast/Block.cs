namespace DotNetCodeGenerator.Ast;

public class Block : Statement
{
    public Block(List<Statement>? statements = null)
    {
        Statements = statements ?? new List<Statement>();
    }

    public override void Accept(IStatementVisitor statementVisitor)
    {
        statementVisitor.VisitStatement(this);
    }

    public readonly List<Statement> Statements;
}