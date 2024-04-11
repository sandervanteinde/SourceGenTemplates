using System.Text;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.VariableExpressions;

public class VariableExpressionNodePropertyAccess(IdentifierToken identifier, PropertyAccessNode propertyAccess)
    : VariableExpressionNode(VariableExpressionNodeType.PropertyAccess, propertyAccess.Identifier)
{
    public IdentifierToken Identifier => identifier;
    public PropertyAccessNode PropertyAccess => propertyAccess;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        sb.Append(identifier.Identifier);
        sb.Append(".");
        propertyAccess.AppendDebugString(sb);
    }
}