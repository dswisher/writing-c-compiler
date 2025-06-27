// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Diagnostics;
using SwishCC.Exceptions;
using SwishCC.IntegrationTests.Models;

namespace SwishCC.IntegrationTests
{
    public class TestRunner
    {
        private readonly TestSeeker testSeeker = new();


        public int Run(TestRunnerOptions options)
        {
            // Time it
            var timer = Stopwatch.StartNew();

            // Find all the test files that should be run - a list of file paths, relative to the test directory
            var testFiles = testSeeker.FindTests(options);
            if (testFiles == null)
            {
                Console.WriteLine("Could not load test files.");
                return 12;
            }

            Console.WriteLine($"Found {testFiles.Count} test files.");

            // Go through and run each test
            var failedTests = 0;
            var passedTests = 0;
            var first = true;

            foreach (var testFile in testFiles)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    Console.WriteLine();
                }

                Console.WriteLine($"******************* {testFile.SubPath} *******************");

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
                        if (options.Stage == Stage.CompileAndRun && !options.EmitAssembly)
                        {
                            // Run the executable and verify the proper exit code
                            if (options.Verbose)
                            {
                                Console.WriteLine($"Running executable {testFile.SubPath}, expecting exit code {testFile.ExpectedExitCode}...");
                            }

                            var exitCode = RunProgram(testFile);

                            if (exitCode != testFile.ExpectedExitCode)
                            {
                                Console.WriteLine($"Valid test {testFile.SubPath} FAILED! Expected exit code {testFile.ExpectedExitCode}, but got {exitCode}.");

                                failedTests += 1;

                                if (options.StopOnFailure)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Valid test {testFile.SubPath} PASSED, properly returned exit code {exitCode}.");

                                passedTests += 1;
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Valid test {testFile.SubPath} PASSED.");

                            passedTests += 1;
                        }
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
                        Console.WriteLine($"Invalid test {testFile.SubPath} PASSED, with exit code {compilerResult}.");

                        passedTests += 1;
                    }
                }
            }

            // Report!
            Console.WriteLine($"{testFiles.Count} tests run, {passedTests} passed, {failedTests} failed, in {timer.Elapsed:hh\\:mm\\:ss}.");

            // Return the exit code
            return failedTests > 0 ? 1 : 0;
        }


        private static int RunProgram(TestFile testFile)
        {
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = testFile.FullExecutablePath;
            var process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();

            return process.ExitCode;
        }


        private static int RunOneTest(TestRunnerOptions options, TestFile testFile)
        {
            try
            {
                var driver = new Driver();
                var driverOptions = new CompilerOptions
                {
                    LexerOnly = options.Stage == Stage.Lex,
                    ParseOnly = options.Stage == Stage.Parse,
                    TackyOnly = options.Stage == Stage.Tacky,
                    CodeGenOnly = options.Stage == Stage.CodeGen,
                    EmitAssembly = options.EmitAssembly,
                    FilePath = testFile.FullPath,
                    Quiet = !options.Verbose,
                    DumpCTree = options.DumpCTree,
                    DumpTacky = options.DumpTacky,
                    DumpAssemblyTree = options.DumpAssemblyTree,
                    KeepIntermediateFiles = options.KeepIntermediateFiles
                };

                return driver.Run(driverOptions);
            }
            catch (LexerException ex)
            {
                Console.WriteLine("Lexer Error: {0}", ex.Message);
                return 1;
            }
            catch (ParseException ex)
            {
                Console.WriteLine("Parser Error: {0}", ex.Message);
                return 2;
            }
            catch (CompilerException ex)
            {
                Console.WriteLine("Compiler Error: {0}", ex.Message);
                return 3;
            }
            catch (Exception)
            {
                Console.WriteLine($"*** Unexpected exception running test {testFile.SubPath}");
                throw;
            }
        }
    }
}
