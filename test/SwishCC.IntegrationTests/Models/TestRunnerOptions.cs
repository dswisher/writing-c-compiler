// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using CommandLine;

namespace SwishCC.IntegrationTests.Models
{
    public class TestRunnerOptions
    {
        [Option("test-root", Default = "/Users/swisherd/git/other/writing-a-c-compiler-tests", HelpText = "The root directory where the test files are located.")]
        public string TestRoot { get; set; }

        [Option("stage", Default = Stage.CompileAndRun, HelpText = "The stage of the compiler to test.")]
        public Stage Stage { get; set; }

        [Option("chapter", Required = true, HelpText = "Specify the chapter whose tests should be run.")]
        public int Chapter { get; set; }

        [Option("latest-only", HelpText = "Only run the tests for the latest chapter.")]
        public bool LatestOnly { get; set; }

        [Option('f', "stop-on-failure", HelpText = "Stop as soon as a test fails.")]
        public bool StopOnFailure { get; set; }

        [Option("dump-c-tree", HelpText = "Save the C AST to a file.")]
        public bool DumpCTree { get; set; }

        [Option("dump-tacky", HelpText = "Save the TACKY to a file.")]
        public bool DumpTacky { get; set; }

        [Option("dump-assembly-tree", HelpText = "Save the Assembly AST to a file.")]
        public bool DumpAssemblyTree { get; set; }

        [Option("verbose", HelpText = "Print verbose log messages.")]
        public bool Verbose { get; set; }

        [Option("skip-invalid", HelpText = "Skip invalid tests.")]
        public bool SkipInvalid { get; set; }

        [Option('S', "emit-assembly", HelpText = "Emit an assembly file, but do not assemble or link it.")]
        public bool EmitAssembly { get; set; }

        [Option("keep", HelpText = "Keep the intermediate files generated during compilation (for debugging).")]
        public bool KeepIntermediateFiles { get; set; }
    }
}
