namespace DotNetCodeGenerator.Ast;

public class LiteralAccessor : Statement
{
    public readonly Literal Literal;

    public LiteralAccessor(object value)
    {
        Literal = value is Literal ? value as Literal : new Literal(value);
    }

    public override void Accept(IStatementVisitor statementVisitor)
    {
        statementVisitor.VisitStatement(this);
    }
}