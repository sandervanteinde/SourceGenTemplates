using System.Text;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.Expressions;

public class IdentifierExpressionNode(IdentifierToken identifier) : ExpressionNode(ExpressionType.Identifier, identifier)
{
    public IdentifierToken Identifier => identifier;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        sb.Append(identifier.Identifier);
    }
}