namespace DotNetCodeGenerator.Ast;

public class VariableExpression : Expression
{
    public VariableExpression(Token name)
    {
        Name = name;
    }

    public override void Accept(IExpressionVisitor expressionVisitor)
    {
        expressionVisitor.VisitVariableExpression(this);
    }

    public Token Name;
}