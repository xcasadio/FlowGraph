namespace DotNetCodeGenerator.Ast;

public class While : Statement
{
    public While(Expression condition, Statement body)
    {
        Condition = condition;
        Body = body;
    }

    public override void Accept(IStatementVisitor statementVisitor)
    {
        statementVisitor.VisitStatement(this);
    }

    public Expression Condition;
    public Statement Body;
}