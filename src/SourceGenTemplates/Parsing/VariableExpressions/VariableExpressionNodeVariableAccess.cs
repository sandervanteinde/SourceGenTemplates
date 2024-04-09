using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.VariableExpressions;

public class VariableExpressionNodeVariableAccess(IdentifierToken identifier) : VariableExpressionNode(VariableExpressionNodeType.VariableAccess, identifier)
{
    public IdentifierToken Identifier => identifier;
}