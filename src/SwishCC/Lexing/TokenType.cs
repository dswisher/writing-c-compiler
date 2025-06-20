// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

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
        Plus,
        Star,
        Slash,
        Percent,

        // Identifiers, keywords, and literals
        Identifier,
        Constant,
        Keyword
    }
}
