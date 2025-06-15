using System;
using System.Diagnostics;
using System.IO;
using SwishCC.Lexing;
using SwishCC.Models.CTree;
using SwishCC.Parsing;
using SwishCC.Tackying;

namespace SwishCC
{
    public class Driver
    {
        public int Run(Options options)
        {
            // Sanity check
            if (!File.Exists(options.FilePath))
            {
                Console.WriteLine($"File not found: {options.FilePath}");
                return 1;
            }

            // Build out the paths of the intermediate files
            var context = CreateContext(options);

            try
            {
                // Run the file through the preprocessor
                if (!PreprocessFile(context))
                {
                    return 1;
                }

                // Run the preprocessed file through the lexer
                if (!options.Quiet)
                {
                    Console.WriteLine("Lexing...");
                }

                var lexer = new Lexer();
                var tokens = lexer.Tokenize(context.PreprocessedFilePath);

                if (tokens == null)
                {
                    Console.WriteLine("Lexer encountered an error.");
                    return 1;
                }

                if (options.LexerOnly)
                {
                    return 0;
                }

                // Parse
                if (!options.Quiet)
                {
                    Console.WriteLine("Parsing...");
                }

                var parser = new Parser();
                var ast = parser.Parse(tokens);

                if (options.DumpAst)
                {
                    var astWriter = new AstWriter();

                    astWriter.Write(ast, context.AstFilePath);
                }

                if (options.ParseOnly)
                {
                    return 0;
                }

                // Convert the AST to TACKY
                var tackyGenerator = new TackyGenerator();
                var tacky = tackyGenerator.EmitTacky(ast);

                if (options.DumpTacky)
                {
                    var tackyWriter = new TackyWriter();

                    tackyWriter.Write(tacky, context.TackyFilePath);
                }

                if (options.TackyOnly)
                {
                    return 0;
                }

                // Convert the AST to an assembly (.s) file
                if (!CreateAssemblyFile(context, ast))
                {
                    return 2;
                }

                // Assemble and link
                if (!AssembleAndLink(context))
                {
                    return 3;
                }

                // No errors!
                return 0;
            }
            finally
            {
                // Clean up the intermediate files
                Cleanup(context);
            }
        }


        private static Context CreateContext(Options options)
        {
            var path = Path.GetDirectoryName(options.FilePath);
            var baseFileName = Path.GetFileNameWithoutExtension(options.FilePath);

            var context = new Context
            {
                Options = options,
                InputFilePath = options.FilePath,
                PreprocessedFilePath = Path.Join(path, $"{baseFileName}.i"),
                AstFilePath = Path.Join(path, $"{baseFileName}.ast"),
                TackyFilePath = Path.Join(path, $"{baseFileName}.tacky"),
                AssemblyFilePath = Path.Join(path, $"{baseFileName}.s"),
                ExecutableFilePath = Path.Join(path, baseFileName)
            };

            return context;
        }


        private static bool PreprocessFile(Context context)
        {
            if (!context.Options.Quiet)
            {
                Console.WriteLine($"Preprocessing {context.InputFilePath} -> {context.PreprocessedFilePath}");
            }

            var startInfo = new ProcessStartInfo();
            startInfo.FileName = "gcc";
            startInfo.Arguments = $"-E -P {context.InputFilePath} -o {context.PreprocessedFilePath}";
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            var process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            // Read the output and error streams
            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();

            // TODO - do something with the output and error

            // No errors!
            // TODO - actually check for errors
            return true;
        }


        private static bool CreateAssemblyFile(Context context, CProgramNode ast)
        {
            if (!context.Options.Quiet)
            {
                Console.WriteLine($"Creating assembly file {context.AssemblyFilePath}");
            }

#if false
            if (ast is ProgramNode pn)
            {
                var val = pn.FunctionDefinition.Body.Expression.Value;

                // TODO - this is a hack, until TACKY
                using (var writer = new StreamWriter(context.AssemblyFilePath))
                {
                    // NOTE: On MAC, need "_main" instead of "main"

                    // writer.WriteLine(".section .text");
                    writer.WriteLine("    .globl _main");
                    writer.WriteLine("_main:");
                    writer.WriteLine($"    movl ${val}, %eax");
                    writer.WriteLine("    ret");
                }

                return true;
            }

            Console.WriteLine("Error: AST is not a ProgramNode.");
#endif

            Console.WriteLine("Error: CreateAssemblyFile not implemented yet.");

            return false;
        }


        private static bool AssembleAndLink(Context context)
        {
            if (!context.Options.Quiet)
            {
                Console.WriteLine($"Assembling and linking {context.AssemblyFilePath} -> {context.ExecutableFilePath}");
            }

            var startInfo = new ProcessStartInfo();
            startInfo.FileName = "gcc";
            startInfo.Arguments = $"{context.AssemblyFilePath} -o {context.ExecutableFilePath}";
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            var process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            // Read the output and error streams
            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();

            // TODO - do something with the output and error

            // No errors!
            // TODO - actually check for errors
            return true;
        }


        private static void Cleanup(Context context)
        {
            // Keep the files, if requested
            if (context.Options.KeepIntermediateFiles)
            {
                return;
            }

            // Go clean up the files
            if (File.Exists(context.PreprocessedFilePath))
            {
                File.Delete(context.PreprocessedFilePath);
            }

            if (File.Exists(context.AstFilePath))
            {
                File.Delete(context.AstFilePath);
            }

            if (File.Exists(context.TackyFilePath))
            {
                File.Delete(context.TackyFilePath);
            }

            if (File.Exists(context.AssemblyFilePath))
            {
                File.Delete(context.AssemblyFilePath);
            }
        }


        private class Context
        {
            public Options Options { get; set; }

            public string InputFilePath { get; set; }
            public string PreprocessedFilePath { get; set; }
            public string AstFilePath { get; set; }
            public string TackyFilePath { get; set; }
            public string AssemblyFilePath { get; set; }
            public string ExecutableFilePath { get; set; }
        }
    }
}
