﻿using System;
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
                });

                var parsed = parser.ParseArguments<Models.Options>(args);

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
