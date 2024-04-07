using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing;

public class IdentifierNode(IdentifierToken identifier) : Node
{
    public IdentifierToken Identifier => identifier;
}