namespace DotNetCodeGenerator.Ast;

public abstract class Statement
{
    public abstract void Accept(IStatementVisitor visitor);
}