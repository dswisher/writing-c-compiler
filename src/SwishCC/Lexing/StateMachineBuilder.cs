using System.Collections.Generic;

namespace SwishCC.Lexing
{
    public static class StateMachineBuilder
    {
        private static readonly HashSet<string> Keywords = ["int", "void", "return"];


        public static (LexerState, LexerState) BuildStateMachine()
        {
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Create all the states to make sure they exist, so we can link them together in any order.
            var lookingForToken = new LexerState("LookingForToken");    // aka "idle"
            var inConstant = new LexerState("InConstant");
            var inIdentifierOrKeyword = new LexerState("InIdentifier");
            var sawHyphen = new LexerState("SawHyphen");
            var done = new LexerState("Done");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Looking for the start of the next token
            lookingForToken
                .AddTransition(-1, (_, ctx) =>
                {
                    ctx.CurrentState = done;
                })
                .AddTransition(" \n\t", (_, ctx) =>
                {
                    ctx.LexerReader.Advance();
                })
                .AddTransition("0123456789", (_, ctx) =>
                {
                    ctx.CurrentState = inConstant;
                })
                .AddTransition("a-zA-Z_", (_, ctx) =>
                {
                    ctx.CurrentState = inIdentifierOrKeyword;
                })
                .AddTransition("-", (ch, ctx) =>
                {
                    ctx.AppendCharacter(ch);
                    ctx.LexerReader.Advance();
                    ctx.CurrentState = sawHyphen;
                })
                .AddTransition(";{}()~", (ch, ctx) =>
                {
                    ctx.EmitToken(ch);
                    ctx.LexerReader.Advance();
                });

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Working on building a constant value ("123", etc.)
            inConstant
                .AddTransition("0123456789", (ch, ctx) =>
                {
                    ctx.AppendCharacter(ch);
                    ctx.LexerReader.Advance();
                })
                .AddTransition("a-zA-Z_", (_, ctx) =>
                {
                    throw new LexException($"Invalid identifier at line {ctx.LineNumber}, column {ctx.ColumnNumber}.");
                })
                .Otherwise((_, ctx) =>
                {
                    ctx.EmitToken(TokenType.Constant);
                    ctx.CurrentState = lookingForToken;
                });

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Working on building an identifier or keyword
            inIdentifierOrKeyword
                .AddTransition("a-zA-Z_", (ch, ctx) =>
                {
                    ctx.AppendCharacter(ch);
                    ctx.LexerReader.Advance();
                })
                .Otherwise((_, ctx) =>
                {
                    var tokenType = Keywords.Contains(ctx.CurrentBuffer) ? TokenType.Keyword : TokenType.Identifier;

                    ctx.EmitToken(tokenType);
                    ctx.CurrentState = lookingForToken;
                });

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Just saw a hyphen, so we need to check if it's a double hyphen or not.
            sawHyphen
                .AddTransition("-", (ch, ctx) =>
                {
                    ctx.AppendCharacter(ch);
                    ctx.LexerReader.Advance();
                    ctx.EmitToken(TokenType.TwoHyphens);
                    ctx.CurrentState = lookingForToken;
                })
                .Otherwise((_, ctx) =>
                {
                    ctx.EmitToken(TokenType.Hyphen);
                    ctx.CurrentState = lookingForToken;
                });

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Return the initial state.
            return (lookingForToken, done);
        }
    }
}
