namespace SwishCC.AST
{
    public class FunctionNode : TreeNode
    {
        public Identifier Name { get; set; }
        public ReturnNode Body { get; set; }
    }
}
