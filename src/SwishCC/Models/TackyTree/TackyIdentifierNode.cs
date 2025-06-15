namespace SwishCC.Models.TackyTree
{
    public class TackyIdentifierNode
    {
        public TackyIdentifierNode(string value)
        {
            Value = value;
        }


        public string Value { get; private set; }
    }
}
