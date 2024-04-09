using SourceGenTemplates.Parsing.VariableExpressions;

namespace SourceGenTemplates.Parsing.BlockNodes;

public class VariableInsertionBlockNode(VariableExpressionNode variableExpression) : BlockNode
{
    public VariableExpressionNode VariableExpression => variableExpression;
    public override BlockNodeType Type => BlockNodeType.VariableInsertion;
}