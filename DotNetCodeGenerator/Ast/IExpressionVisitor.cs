namespace DotNetCodeGenerator.Ast;

public interface IExpressionVisitor
{
    void VisitAssignExpression(Assign expression);
    void VisitBinaryExpression(Binary expression);
    void VisitGroupingExpression(Grouping expression);
    void VisitLiteralExpression(Literal expression);
    void VisitLogicalExpression(Logical expression);
    void VisitUnaryExpression(Unary expression);
    void VisitVariableExpression(VariableExpression expression);
}