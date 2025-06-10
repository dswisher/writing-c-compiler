using CommandLine;

namespace SwishCC
{
    public class Options
    {
        [Option("lex", HelpText = "Run the lexer, but stop before parsing.")]
        public bool LexerOnly { get; set; }

        [Option("parse", HelpText = "Run the lexer and parser, but stop before assembly generation.")]
        public bool ParseOnly { get; set; }

        [Option("codegen", HelpText = "Run the lexer, parser, and assembly generation, but stop before code emission.")]
        public bool CodeGen { get; set; }

        [Option('S', HelpText = "Emit an assembly file, but do not assemble or link it.")]
        public bool EmitAssembly { get; set; }

        [Value(0)]
        public string FilePath { get; set; }

        [Option("keep", HelpText = "Keep the intermediate files generated during compilation (for debugging).")]
        public bool KeepIntermediateFiles { get; set; }

        [Option("print-ast", HelpText = "Print the AST after parsing.")]
        public bool PrintAst { get; set; }
    }
}
