using System;
using System.Diagnostics;
using SwishCC.IntegrationTests.Models;
using SwishCC.Lexing;
using SwishCC.Parsing;
using DriverOps = SwishCC.Options;

namespace SwishCC.IntegrationTests
{
    public class TestRunner
    {
        private readonly TestSeeker testSeeker = new();


        public int Run(Models.Options options)
        {
            // Time it
            var timer = Stopwatch.StartNew();

            // Load the list of expected results
            // TODO

            // Find all the test files that should be run - a list of file paths, relative to the test directory
            var testFiles = testSeeker.FindTests(options);

            Console.WriteLine($"Found {testFiles.Count} test files.");

            // Go through and run each test
            var failedTests = 0;
            var passedTests = 0;

            foreach (var testFile in testFiles)
            {
                var compilerResult = RunOneTest(options, testFile);

                if (testFile.IsValid)
                {
                    if (compilerResult != 0)
                    {
                        Console.WriteLine($"Valid test {testFile.SubPath} FAILED with exit code {compilerResult}.");

                        failedTests += 1;

                        if (options.StopOnFailure)
                        {
                            break;
                        }
                    }
                    else
                    {
                        // TODO - if we compiled, run the resulting executable and check the result code
                        Console.WriteLine($"Valid test {testFile.SubPath} passed.");

                        passedTests += 1;
                    }
                }
                else
                {
                    if (compilerResult == 0)
                    {
                        Console.WriteLine($"Invalid test {testFile.SubPath} did not fail.");

                        failedTests += 1;

                        if (options.StopOnFailure)
                        {
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Invalid test {testFile.SubPath} passed, with exit code {compilerResult}.");

                        passedTests += 1;
                    }
                }
            }

            // Report!
            Console.WriteLine($"{testFiles.Count} tests run, {passedTests} passed, {failedTests} failed, in {timer.Elapsed:hh\\:mm\\:ss}.");

            // Return the exit code
            return failedTests > 0 ? 1 : 0;
        }


        private static int RunOneTest(Models.Options options, TestFile testFile)
        {
            try
            {
                var driver = new Driver();
                var driverOptions = new DriverOps
                {
                    LexerOnly = options.Stage == Stage.Lex,
                    ParseOnly = options.Stage == Stage.Parse,
                    TackyOnly = options.Stage == Stage.Tacky,
                    CodeGenOnly = options.Stage == Stage.CodeGen,
                    FilePath = testFile.FullPath,
                    Quiet = true
                };

                return driver.Run(driverOptions);
            }
            catch (LexException)
            {
                return 1;
            }
            catch (ParseException)
            {
                return 2;
            }
            catch (Exception)
            {
                Console.WriteLine($"*** Unexpected exception running test {testFile.SubPath}");
                throw;
            }
        }
    }
}
