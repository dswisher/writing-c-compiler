using System;
using CommandLine;

namespace SwishCC;

public static class Program
{
    public static int Main(string[] args)
    {
        var driver = new Driver();
        var parsed = Parser.Default.ParseArguments<Options>(args);

        return parsed.MapResult(
            options => driver.Run(options),
            _ => 1);
    }
}
