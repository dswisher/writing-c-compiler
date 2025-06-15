namespace SwishCC.Models.CTree
{
    public class CUnaryExpressionNode : CAbstractExpressionNode
    {
        public CUnaryOperator Operator { get; set; }
        public CAbstractExpressionNode Operand { get; set; }
    }
}
