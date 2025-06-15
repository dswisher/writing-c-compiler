using System.Collections.Generic;

namespace SwishCC.Models.TackyTree
{
    public class TackyFunctionNode
    {
        public TackyIdentifierNode Name { get; set; }
        public List<TackyAbstractInstructionNode> Instructions { get; } = new();
    }
}
