using System.Text;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing;

public class PropertyAccessNode(IdentifierToken identifier, PropertyAccessNode? propertyAccess) : Node
{
    public IdentifierToken Identifier => identifier;
    public PropertyAccessNode? PropertyAccess => propertyAccess;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        sb.Append(identifier.Identifier);

        if (propertyAccess is null)
        {
            return;
        }

        sb.Append('.');
        propertyAccess.AppendDebugString(sb);
    }
}