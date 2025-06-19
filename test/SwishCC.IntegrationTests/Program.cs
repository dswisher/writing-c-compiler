// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using CommandLine;

namespace SwishCC.IntegrationTests
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                var runner = new TestRunner();

                var parser = new Parser(with =>
                {
                    with.CaseInsensitiveEnumValues = true;
                    with.HelpWriter = Console.Error;
                });

                var parsed = parser.ParseArguments<Models.TestRunnerOptions>(args);

                return parsed.MapResult(
                    options => runner.Run(options),
                    _ => 1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }
    }
}
