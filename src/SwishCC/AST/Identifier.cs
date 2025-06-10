namespace SwishCC.AST
{
    public class Identifier
    {
        public Identifier(string value)
        {
            Value = value;
        }


        public string Value { get; private set; }
    }
}
