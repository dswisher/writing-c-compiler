using System.IO;
using FluentAssertions;
using SwishCC.Lexing;
using SwishCC.Models.TackyTree;
using SwishCC.Parsing;
using SwishCC.Tackying;
using Xunit;

namespace SwishCC.UnitTests.Tackying
{
    public class TackyGeneratorTests
    {
        private readonly Lexer lexer = new();
        private readonly Parser parser = new();
        private readonly TackyGenerator tackyGenerator = new();


        [Theory]
        [InlineData("2", 1)]
        public void CanGenerateTackyExpression(string exp, int expectedInstructions)
        {
            // Arrange
            var content = "int main(void) {\n" +
                          $"    return {exp};\n" +
                          "}";

            LexerResult tokens;

            using (var reader = new StringReader(content))
            {
                tokens = lexer.Tokenize(reader);
            }

            var ast = parser.Parse(tokens);

            // Act
            var tackyProgram = tackyGenerator.EmitTacky(ast);

            // Assert
            tackyProgram.Should().NotBeNull();
            tackyProgram.Should().BeOfType<TackyProgramNode>();

            var tackyFunction = tackyProgram.FunctionDefinition;

            tackyFunction.Should().NotBeNull();
            tackyFunction.Name.Should().NotBeNull();
            tackyFunction.Instructions.Should().HaveCount(expectedInstructions);
        }
    }
}
