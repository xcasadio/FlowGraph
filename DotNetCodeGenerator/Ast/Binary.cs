namespace DotNetCodeGenerator.Ast;

public class Binary : Expression
{
    public Binary(Expression left, Token op, Expression right)
    {
        Left = left;
        Op = op;
        Right = right;
    }

    public override void Accept(IExpressionVisitor expressionVisitor)
    {
        expressionVisitor.VisitBinaryExpression(this);
    }

    public Expression Left;
    public Token Op;
    public Expression Right;
}