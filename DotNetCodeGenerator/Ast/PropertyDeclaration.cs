namespace DotNetCodeGenerator.Ast;

public class PropertyDeclaration : Statement
{
    public bool IsOverride;
    public readonly Token Name;

    public PropertyDeclaration(string type, string name)
    {
        Name = new Token(TokenType.Property, type, name);
    }

    public override void Accept(IStatementVisitor statementVisitor)
    {
        statementVisitor.VisitStatement(this);
    }
}