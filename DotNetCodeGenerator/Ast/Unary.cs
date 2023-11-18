namespace DotNetCodeGenerator.Ast;

public class Unary : Expression
{
    public Unary(Token op, Expression right)
    {
        Op = op;
        Right = right;
    }

    public override void Accept(IExpressionVisitor expressionVisitor)
    {
        expressionVisitor.VisitUnaryExpression(this);
    }

    public Token Op;
    public Expression Right;
}