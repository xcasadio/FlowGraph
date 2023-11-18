namespace DotNetCodeGenerator.Ast;

public class StatementExpression : Statement
{
    public StatementExpression(Expression expression)
    {
        Expression = expression;
    }

    public override void Accept(IStatementVisitor statementVisitor)
    {
        statementVisitor.VisitStatement(this);
    }

    public Expression Expression;
}