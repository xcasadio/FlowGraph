namespace DotNetCodeGenerator.Ast;

public class Grouping : Expression
{
    public Grouping(Expression expression)
    {
        Expression = expression;
    }

    public override void Accept(IExpressionVisitor expressionVisitor)
    {
        expressionVisitor.VisitGroupingExpression(this);
    }

    public Expression Expression;
}