namespace DotNetCodeGenerator.Ast;

public class Literal : Expression
{
    public Literal(object value)
    {
        Value = value;
    }

    public override void Accept(IExpressionVisitor expressionVisitor)
    {
        expressionVisitor.VisitLiteralExpression(this);
    }

    public readonly object Value;
}