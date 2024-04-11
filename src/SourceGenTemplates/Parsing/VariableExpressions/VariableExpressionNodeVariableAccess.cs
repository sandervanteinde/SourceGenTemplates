using System.Text;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.VariableExpressions;

public class VariableExpressionNodeVariableAccess(IdentifierToken identifier) : VariableExpressionNode(VariableExpressionNodeType.VariableAccess, identifier)
{
    public IdentifierToken Identifier => identifier;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        sb.Append(identifier.Identifier);
    }
}