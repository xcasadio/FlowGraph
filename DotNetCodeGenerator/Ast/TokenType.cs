namespace DotNetCodeGenerator.Ast;

public enum TokenType
{
    // Single-character tokens.
    LeftParentese, RightParen, LeftBrace, RightBrace,
    Comma, Dot, Minus, Plus, Semicolon, Slash, Star,

    // One or two character tokens.
    Bang, BangEqual,
    Equal, EqualEqual,
    Greater, GreaterEqual,
    Less, LessEqual,

    // Literals.
    Identifier, String, Number,

    // Keywords.
    And, Using, Namespace, Class, Else, False, Function, For, If, Null, Or,
    Return, Base, This, True, Var, While,
    Break, Continue,

    Eof
}