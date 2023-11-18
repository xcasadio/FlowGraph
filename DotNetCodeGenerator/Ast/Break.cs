namespace DotNetCodeGenerator.Ast;

public class Break : Statement
{
    public override void Accept(IStatementVisitor statementVisitor)
    {
        statementVisitor.VisitStatement(this);
    }

}