using CommandLine;

namespace SwishCC
{
    public class Options
    {
        [Option("quiet", HelpText = "Do not emit progress messages.")]
        public bool Quiet { get; set; }

        [Option("lex", HelpText = "Run the lexer, but stop before parsing.")]
        public bool LexerOnly { get; set; }

        [Option("parse", HelpText = "Run the lexer and parser, but stop before assembly generation.")]
        public bool ParseOnly { get; set; }

        [Option("tacky", HelpText = "Run the lexer, parser, and tacky generation, but stop before code generation.")]
        public bool TackyOnly { get; set; }

        [Option("codegen", HelpText = "Run the lexer, parser, and assembly generation, but stop before code emission.")]
        public bool CodeGenOnly { get; set; }

        [Option('S', HelpText = "Emit an assembly file, but do not assemble or link it.")]
        public bool EmitAssembly { get; set; }

        [Value(0)]
        public string FilePath { get; set; }

        [Option("keep", HelpText = "Keep the intermediate files generated during compilation (for debugging).")]
        public bool KeepIntermediateFiles { get; set; }

        [Option("dump-ast", HelpText = "Save the AST to a file.")]
        public bool DumpAst { get; set; }

        [Option("dump-tacky", HelpText = "Save the TACKY to a file.")]
        public bool DumpTacky { get; set; }
    }
}
