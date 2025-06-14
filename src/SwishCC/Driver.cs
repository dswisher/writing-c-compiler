using System;
using System.Diagnostics;
using System.IO;
using SwishCC.AST;
using SwishCC.Lexing;
using SwishCC.Parsing;

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

            if (options.PrintAst)
            {
                // TODO - print the AST
            }

            if (options.ParseOnly)
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

            // Clean up the intermediate files
            Cleanup(context);

            // No errors!
            return 0;
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


        private static bool CreateAssemblyFile(Context context, TreeNode ast)
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
            public string AssemblyFilePath { get; set; }
            public string ExecutableFilePath { get; set; }
        }
    }
}
