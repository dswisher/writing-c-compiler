// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using SwishCC.AssemblyGen;
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

                if (options.DumpCTree)
                {
                    var cTreeWriter = new CTreeWriter();

                    cTreeWriter.Write(ast, context.CTreeFilePath);
                }

                if (options.ParseOnly)
                {
                    return 0;
                }

                // Convert the AST to TACKY
                if (!options.Quiet)
                {
                    Console.WriteLine("Converting C AST to TACKY...");
                }

                var tackyGenerator = new TackyGenerator();
                var tacky = tackyGenerator.ConvertCTree(ast);

                if (options.DumpTacky)
                {
                    var tackyWriter = new TackyTreeWriter();

                    tackyWriter.Write(tacky, context.TackyFilePath);
                }

                if (options.TackyOnly)
                {
                    return 0;
                }

                // Convert the TACKY to an Assembly AST
                if (!options.Quiet)
                {
                    Console.WriteLine("Converting TACKY to Assembly AST...");
                }

                var assemblyGenerator = new AssemblyGenerator();
                var assemblyAst = assemblyGenerator.ConvertTacky(tacky);

                if (options.DumpAssemblyTree)
                {
                    var assemblyTreeWriter = new AssemblyTreeWriter();

                    assemblyTreeWriter.Write(assemblyAst, context.AssemblyTreeFilePath);
                }

                // Emit the assembly
                // TODO - emit the assembly

                // Assemble and link
                if (!AssembleAndLink(context))
                {
                    return 4;
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
                CTreeFilePath = Path.Join(path, $"{baseFileName}.c.tree"),
                TackyFilePath = Path.Join(path, $"{baseFileName}.tacky"),
                AssemblyTreeFilePath = Path.Join(path, $"{baseFileName}.a.tree"),
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

            if (process.ExitCode != 0)
            {
                Console.WriteLine(output);
                Console.WriteLine(error);

                return false;
            }

            // No errors!
            return true;
        }


        private static void Cleanup(Context context)
        {
            // If we're not dumping the C AST, clean it up
            if (!context.Options.DumpCTree)
            {
                if (File.Exists(context.CTreeFilePath))
                {
                    File.Delete(context.CTreeFilePath);
                }
            }

            // If we're not dumping TACKY, clean it up
            if (!context.Options.DumpTacky)
            {
                if (File.Exists(context.TackyFilePath))
                {
                    File.Delete(context.TackyFilePath);
                }
            }

            // If we're not dumping the Assembly AST, clean it up
            if (!context.Options.DumpAssemblyTree)
            {
                if (File.Exists(context.AssemblyTreeFilePath))
                {
                    File.Delete(context.AssemblyTreeFilePath);
                }
            }

            // Keep the remaining files, if requested
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
            public string CTreeFilePath { get; set; }
            public string TackyFilePath { get; set; }
            public string AssemblyTreeFilePath { get; set; }
            public string AssemblyFilePath { get; set; }
            public string ExecutableFilePath { get; set; }
        }
    }
}
