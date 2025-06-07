using System.IO;

namespace SwishCC.Lexing
{
    public class Lexer
    {
        public LexerResult Tokenize(string path)
        {
            using (var reader = new StreamReader(path))
            {
                return Tokenize(reader);
            }
        }


        public LexerResult Tokenize(TextReader textReader)
        {
            var result = new LexerResult();
            var (startState, endState) = StateMachineBuilder.BuildStateMachine();

            using (var lexerReader = new LexerReader(textReader))
            {
                var context = new LexerContext(startState, lexerReader, result);

                while (context.CurrentState != endState)
                {
                    context.CurrentState.Execute(context);
                }
            }

            // Return the tokens we found
            return result;
        }
    }
}
