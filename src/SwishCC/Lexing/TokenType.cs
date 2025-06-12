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

        Tilde,
        Hyphen,
        TwoHyphens,

        // Identifiers, keywords, and literals
        Identifier,
        Constant,
        Keyword
    }
}
