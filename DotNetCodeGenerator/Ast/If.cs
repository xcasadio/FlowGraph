namespace DotNetCodeGenerator.Ast;

public class If : Statement
{
    public If(Expression condition, Statement thenBranch, Statement elseBranch)
    {
        Condition = condition;
        ThenBranch = thenBranch;
        ElseBranch = elseBranch;
    }

    public override void Accept(IStatementVisitor statementVisitor)
    {
        statementVisitor.VisitStatement(this);
    }

    public Expression Condition;
    public Statement ThenBranch;
    public Statement ElseBranch;
}