namespace SwishCC.Lexing
{
    public static class StateMachineBuilder
    {
        public static (LexerState, LexerState) BuildStateMachine()
        {
            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Create all the states to make sure they exist, so we can link them together in any order.
            var lookingForToken = new LexerState("LookingForToken");    // aka "idle"
            var inConstant = new LexerState("InConstant");
            var done = new LexerState("Done");

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Looking for the start of the next token
            lookingForToken
                .AddTransition(-1, (_, ctx) =>
                {
                    ctx.CurrentState = done;
                })
                .AddTransition(" ", (_, ctx) =>
                {
                    ctx.LexerReader.Advance();
                })
                .AddTransition("0123456789", (_, ctx) =>
                {
                    ctx.CurrentState = inConstant;
                })
                .AddTransition(";{}()", (ch, ctx) =>
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
                .Otherwise((_, ctx) =>
                {
                    ctx.EmitToken(TokenType.Constant);
                    ctx.CurrentState = lookingForToken;
                });

            // - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
            // Return the initial state.
            return (lookingForToken, done);
        }
    }
}
