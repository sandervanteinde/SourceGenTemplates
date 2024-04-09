using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing;

public class PropertyAccessNode(IdentifierToken identifier, PropertyAccessNode? propertyAccess) : Node
{
    public IdentifierToken Identifier => identifier;
    public PropertyAccessNode? PropertyAccess => propertyAccess;
}