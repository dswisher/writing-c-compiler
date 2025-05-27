namespace SwishCC.Lexing
{
    public enum TokenType
    {
        // Punctuation
        Semicolon,
        LeftParen,
        RightParen,
        LeftCurly,
        RightCurly,

        // Identifiers and literals
        Identifier,
        Constant,

        // Keywords
        Int,
        Return,
        Void
    }
}
