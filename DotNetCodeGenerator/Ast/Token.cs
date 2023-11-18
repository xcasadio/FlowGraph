namespace DotNetCodeGenerator.Ast;

public class Token
{
    public readonly TokenType Type;
    public readonly string Lexeme;
    public readonly object Literal;

    public Token(TokenType type, string lexeme, object literal)
    {
        Type = type;
        Lexeme = lexeme;
        Literal = literal;
    }

    public override string ToString()
    {
        return $"{Type} {Lexeme} {Literal}";
    }
}