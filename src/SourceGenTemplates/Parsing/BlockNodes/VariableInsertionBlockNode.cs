using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.BlockNodes;

public class VariableInsertionBlockNode(IdentifierToken identifier) : BlockNode
{
    public IdentifierToken Identifier => identifier;
    public override BlockNodeType Type => BlockNodeType.VariableInsertion;
}