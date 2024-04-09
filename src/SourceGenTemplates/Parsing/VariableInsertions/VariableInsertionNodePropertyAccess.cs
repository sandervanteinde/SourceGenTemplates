using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.VariableInsertions;

public class VariableInsertionNodePropertyAccess(IdentifierToken identifier, PropertyAccessNode propertyAccess)
    : VariableInsertionNode(VariableInsertionNodeType.PropertyAccess)
{
    public IdentifierToken Identifier => identifier;
    public PropertyAccessNode PropertyAccess => propertyAccess;
}