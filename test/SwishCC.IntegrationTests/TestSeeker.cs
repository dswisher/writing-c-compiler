// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SwishCC.IntegrationTests.Models;

namespace SwishCC.IntegrationTests
{
    public class TestSeeker
    {
        public List<TestFile> FindTests(TestRunnerOptions options)
        {
            // Load the list of expected results
            var expectedResults = LoadExpectedResults(options);
            if (expectedResults == null)
            {
                return null;
            }

            // Get the list of chapters
            var chapters = GetChapters(options);

            // Scan for all the test files
            var testFiles = new List<TestFile>();
            foreach (var chapter in chapters)
            {
                Console.WriteLine($"Scanning for tests in chapter {chapter}...");

                testFiles.AddRange(FindTestFiles(options, chapter, expectedResults));
            }

            // Return what we found
            return testFiles;
        }


        private static Dictionary<string, ExpectedResultItem> LoadExpectedResults(TestRunnerOptions options)
        {
            var path = Path.Join(options.TestRoot, "expected_results.json");

            if (!File.Exists(path))
            {
                Console.WriteLine($"Expected results file not found: {path}");
                return null;
            }

            var json = File.ReadAllText(path);

            var expectedResults = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, ExpectedResultItem>>(json);

            Console.WriteLine($"Loaded {expectedResults.Count} expected results.");

            return expectedResults;
        }


        private static List<int> GetChapters(TestRunnerOptions options)
        {
            var chapters = new List<int>();

            if (options.LatestOnly)
            {
                chapters.Add(options.Chapter);
            }
            else
            {
                chapters.AddRange(Enumerable.Range(1, options.Chapter));
            }

            return chapters;
        }


        private static List<TestFile> FindTestFiles(TestRunnerOptions options, int chapter, Dictionary<string, ExpectedResultItem> expectedResults)
        {
            var result = new List<TestFile>();
            var chapterSubDir = $"chapter_{chapter}";
            var (validDirs, invalidDirs) = GetDirs(options);

            foreach (var dir in validDirs)
            {
                var dirSubPath = Path.Combine(chapterSubDir, dir);
                var fullDirPath = Path.Combine(options.TestRoot, "tests", dirSubPath);

                result.AddRange(ScanForFiles(fullDirPath, dirSubPath, expectedResults, true));
            }

            if (!options.SkipInvalid)
            {
                foreach (var dir in invalidDirs)
                {
                    var dirSubPath = Path.Combine(chapterSubDir, dir);
                    var fullDirPath = Path.Combine(options.TestRoot, "tests", dirSubPath);

                    result.AddRange(ScanForFiles(fullDirPath, dirSubPath, expectedResults, false));
                }
            }

            return result;
        }


        private static IEnumerable<TestFile> ScanForFiles(string fullDirPath, string dirSubPath, Dictionary<string, ExpectedResultItem> expectedResults, bool isValid)
        {
            if (!Directory.Exists(fullDirPath))
            {
                yield break;
            }

            foreach (var filePath in Directory.EnumerateFiles(fullDirPath, "*.c"))
            {
                var subPath = Path.Join(dirSubPath, Path.GetFileName(filePath));
                var resultItem = expectedResults.GetValueOrDefault(subPath);

                yield return new TestFile
                {
                    FullPath = filePath,
                    SubPath = subPath,
                    FullExecutablePath = Path.Join(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath)),
                    IsValid = isValid,
                    ExpectedExitCode = resultItem?.ReturnCode ?? 0,
                    ExpectedStandardOutput = resultItem?.StandardOutput
                };
            }
        }


        private static (List<string> ValidDirs, List<string> InvalidDirs) GetDirs(TestRunnerOptions options)
        {
            const string invalidLex = "invalid_lex";
            const string invalidParse = "invalid_parse";
            const string invalidSemantics = "invalid_semantics";
            const string invalidDeclarations = "invalid_declarations";
            const string invalidTypes = "invalid_types";
            const string invalidLabels = "invalid_labels";
            const string invalidStructTags = "invalid_struct_tags";

            const string valid = "valid";

            var validDirs = new List<string> { valid };
            var invalidDirs = new List<string>();

            if (options.Stage == Stage.Lex)
            {
                invalidDirs.Add(invalidLex);

                validDirs.Add(invalidParse);
                validDirs.Add(invalidSemantics);
                validDirs.Add(invalidDeclarations);
                validDirs.Add(invalidTypes);
                validDirs.Add(invalidLabels);
                validDirs.Add(invalidStructTags);
            }
            else if (options.Stage == Stage.Parse)
            {
                invalidDirs.Add(invalidLex);
                invalidDirs.Add(invalidParse);

                validDirs.Add(invalidSemantics);
                validDirs.Add(invalidDeclarations);
                validDirs.Add(invalidTypes);
                validDirs.Add(invalidLabels);
                validDirs.Add(invalidStructTags);
            }
            else
            {
                invalidDirs.Add(invalidLex);
                invalidDirs.Add(invalidParse);
                invalidDirs.Add(invalidSemantics);
                invalidDirs.Add(invalidDeclarations);
                invalidDirs.Add(invalidTypes);
                invalidDirs.Add(invalidLabels);
                invalidDirs.Add(invalidStructTags);
            }

            return (validDirs, invalidDirs);
        }
    }
}
