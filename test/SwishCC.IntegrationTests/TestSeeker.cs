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
        public List<TestFile> FindTests(Models.Options options)
        {
            // Get the list of chapters
            var chapters = GetChapters(options);

            // Scan for all the test files
            var testFiles = new List<TestFile>();
            foreach (var chapter in chapters)
            {
                Console.WriteLine($"Scanning for tests in chapter {chapter}...");

                testFiles.AddRange(FindTestFiles(options, chapter));
            }

            // Return what we found
            return testFiles;
        }


        private static List<int> GetChapters(Models.Options options)
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


        private static List<TestFile> FindTestFiles(Models.Options options, int chapter)
        {
            var result = new List<TestFile>();
            var chapterSubDir = $"chapter_{chapter}";
            var (validDirs, invalidDirs) = GetDirs(options);

            foreach (var dir in validDirs)
            {
                var dirSubPath = Path.Combine(chapterSubDir, dir);
                var fullDirPath = Path.Combine(options.TestRoot, dirSubPath);

                result.AddRange(ScanForFiles(fullDirPath, dirSubPath, true));
            }

            foreach (var dir in invalidDirs)
            {
                var dirSubPath = Path.Combine(chapterSubDir, dir);
                var fullDirPath = Path.Combine(options.TestRoot, dirSubPath);

                result.AddRange(ScanForFiles(fullDirPath, dirSubPath, false));
            }

            return result;
        }


        private static IEnumerable<TestFile> ScanForFiles(string fullDirPath, string dirSubPath, bool isValid)
        {
            if (!Directory.Exists(fullDirPath))
            {
                yield break;
            }

            foreach (var filePath in Directory.EnumerateFiles(fullDirPath, "*.c"))
            {
                yield return new TestFile
                {
                    FullPath = filePath,
                    SubPath = Path.Join(dirSubPath, Path.GetFileName(filePath)),
                    IsValid = isValid
                };
            }
        }


        private static (List<string> ValidDirs, List<string> InvalidDirs) GetDirs(Models.Options options)
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
