using CommandLine;

namespace SwishCC.IntegrationTests.Models
{
    public class Options
    {
        [Option("test-root", Default = "/Users/swisherd/git/other/writing-a-c-compiler-tests/tests", HelpText = "The root directory where the test files are located.")]
        public string TestRoot { get; set; }

        [Option("stage", HelpText = "The stage of the compiler to test.")]
        public Stage Stage { get; set; }

        [Option("chapter", Required = true, HelpText = "Specify the chapter whose tests should be run.")]
        public int Chapter { get; set; }

        [Option("latest-only", HelpText = "Only run the tests for the latest chapter.")]
        public bool LatestOnly { get; set; }

        [Option('f', "stop-on-failure", HelpText = "Stop as soon as a test fails.")]
        public bool StopOnFailure { get; set; }
    }
}
