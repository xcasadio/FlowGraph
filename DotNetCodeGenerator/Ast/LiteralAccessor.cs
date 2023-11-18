namespace DotNetCodeGenerator.Ast;

public class LiteralAccessor : Statement
{
    public Literal Literal;

    public LiteralAccessor(object value)
    {
        Literal = new Literal(value);
    }

    public override void Accept(IStatementVisitor statementVisitor)
    {
        statementVisitor.VisitStatement(this);
    }
}