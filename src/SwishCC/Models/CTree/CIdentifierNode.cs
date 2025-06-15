namespace SwishCC.Models.CTree
{
    public class CIdentifierNode
    {
        public CIdentifierNode(string value)
        {
            Value = value;
        }


        public string Value { get; private set; }
    }
}
