namespace SwishCC.AST
{
    public class UnaryExpressionNode : ExpressionNode
    {
        public UnaryOperator Operator { get; set; }
        public ExpressionNode Operand { get; set; }
    }
}
