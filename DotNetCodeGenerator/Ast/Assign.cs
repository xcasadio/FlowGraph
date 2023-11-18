namespace DotNetCodeGenerator.Ast;

public class Assign : Expression
{
    public Assign(Token name, Expression value)
    {
        Name = name;
        Value = value;
    }

    public override void Accept(IExpressionVisitor expressionVisitor)
    {
        expressionVisitor.VisitAssignExpression(this);
    }

    public Token Name;
    public Expression Value;
}