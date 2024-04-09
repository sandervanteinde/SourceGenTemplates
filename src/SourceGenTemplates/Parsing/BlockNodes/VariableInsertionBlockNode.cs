using SourceGenTemplates.Parsing.VariableInsertions;

namespace SourceGenTemplates.Parsing.BlockNodes;

public class VariableInsertionBlockNode(VariableInsertionNode variableInsertion) : BlockNode
{
    public VariableInsertionNode VariableInsertion => variableInsertion;
    public override BlockNodeType Type => BlockNodeType.VariableInsertion;
}