using System;
using System.Diagnostics;
using System.IO;
using SwishCC.Lexing;

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
            var lexer = new Lexer();
            var tokens = lexer.Tokenize(context.PreprocessedFilePath);

            if (tokens == null)
            {
                Console.WriteLine("Lexer encountered an error.");
                return 1;
            }

            if (options.Lexer)
            {
                return 0;
            }

            // TODO - parse, code gen, etc.

            // Assemble and link
            // TODO

            // Clean up the intermediate files
            Cleanup(context);

            // No errors!
            return 0;
        }


        private static Context CreateContext(Options options)
        {
            var baseFileName = Path.GetFileNameWithoutExtension(options.FilePath);

            var context = new Context
            {
                Options = options,
                InputFilePath = options.FilePath,
                PreprocessedFilePath = $"{baseFileName}.i",
                AssemblyFilePath = $"{baseFileName}.s",
                ExecutableFilePath = baseFileName
            };

            return context;
        }


        private static bool PreprocessFile(Context context)
        {
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

            // TODO - do the preprocessing

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

            // TODO - clean up the assembly file (.s)
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
