using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing;

public class VariableInsertionBlockNode(IdentifierToken identifier) : BlockNode
{
    public IdentifierToken Identifier => identifier;
    public override BlockNodeType Type => BlockNodeType.VariableInsertion;
}