using System.Text;
using SourceGenTemplates.Parsing.Mutators;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.VariableExpressions;

public class VariableExpressionNodePropertyAccess(IdentifierToken identifier, PropertyAccessNode propertyAccess, MutatorExpressionNode? mutator)
    : VariableExpressionNode(VariableExpressionNodeType.PropertyAccess, propertyAccess.Identifier)
{
    public IdentifierToken Identifier => identifier;
    public PropertyAccessNode PropertyAccess => propertyAccess;
    public MutatorExpressionNode? Mutator => mutator;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        sb.Append(identifier.Identifier);
        sb.Append(".");
        propertyAccess.AppendDebugString(sb);
        mutator?.AppendDebugString(sb);
    }
}