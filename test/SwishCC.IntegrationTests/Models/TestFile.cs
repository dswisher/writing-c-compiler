namespace SwishCC.IntegrationTests.Models
{
    public class TestFile
    {
        public string FullPath { get; init; }
        public string SubPath { get; init; }
        public bool IsValid { get; init; }
        public int ExpectedExitCode { get; set; }


        public override string ToString()
        {
            return $"{FullPath,-110} - {SubPath,-50} - {(IsValid ? "valid" : "invalid"),-10} - exit code {ExpectedExitCode}";
        }
    }
}
