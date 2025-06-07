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

        // Identifiers, keywords, and literals
        Identifier,
        Constant,
        Keyword
    }
}
