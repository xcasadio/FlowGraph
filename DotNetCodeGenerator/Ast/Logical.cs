namespace DotNetCodeGenerator.Ast;

public class Logical : Expression
{
    public Logical(Expression left, Token op, Expression right)
    {
        Left = left;
        Op = op;
        Right = right;
    }

    public override void Accept(IExpressionVisitor expressionVisitor)
    {
        expressionVisitor.VisitLogicalExpression(this);
    }

    public Expression Left;
    public Token Op;
    public Expression Right;
}