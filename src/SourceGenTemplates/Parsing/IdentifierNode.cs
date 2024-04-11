using System.Text;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing;

public class IdentifierNode(IdentifierToken identifier) : Node
{
    public IdentifierToken Identifier => identifier;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        sb.Append(identifier.Identifier);
    }
}