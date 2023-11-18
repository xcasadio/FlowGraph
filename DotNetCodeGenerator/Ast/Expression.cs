namespace DotNetCodeGenerator.Ast;

public abstract class Expression
{
    public abstract void Accept(IExpressionVisitor expressionVisitor);
}