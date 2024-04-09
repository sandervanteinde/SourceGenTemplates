using SourceGenTemplates.Parsing.VariableExpressions;

namespace SourceGenTemplates.Parsing.BlockNodes;

public class VariableInsertionBlockNode(VariableExpressionNode variableExpression) : BlockNode(BlockNodeType.VariableInsertion)
{
    public VariableExpressionNode VariableExpression => variableExpression;
}