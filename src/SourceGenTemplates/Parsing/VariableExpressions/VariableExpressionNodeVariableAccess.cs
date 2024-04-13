using System.Text;
using SourceGenTemplates.Parsing.Mutators;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.VariableExpressions;

public class VariableExpressionNodeVariableAccess(IdentifierToken identifier, MutatorExpressionNode? mutator)
    : VariableExpressionNode(VariableExpressionNodeType.VariableAccess, identifier)
{
    public IdentifierToken Identifier => identifier;
    public MutatorExpressionNode? Mutator => mutator;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        sb.Append(identifier.Identifier);
        mutator?.AppendDebugString(sb);
    }
}