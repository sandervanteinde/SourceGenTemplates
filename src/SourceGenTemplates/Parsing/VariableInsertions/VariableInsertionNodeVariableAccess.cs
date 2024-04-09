using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.VariableInsertions;

public class VariableInsertionNodeVariableAccess(IdentifierToken identifier) : VariableInsertionNode(VariableInsertionNodeType.VariableAccess)
{
    public IdentifierToken Identifier => identifier;
}