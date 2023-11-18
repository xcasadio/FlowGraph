namespace DotNetCodeGenerator.Ast;

public class VariableStatement : Statement
{
    public Token Name;
    public Expression Initializer;

    public VariableStatement(Token name, Expression initializer)
    {
        Name = name;
        Initializer = initializer;
    }

    public override void Accept(IStatementVisitor statementVisitor)
    {
        statementVisitor.VisitStatement(this);
    }
}