using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.VariableExpressions;

public class VariableExpressionNodeVariableAccess(IdentifierToken identifier) : VariableExpressionNode(VariableInsertionNodeType.VariableAccess)
{
    public IdentifierToken Identifier => identifier;
}