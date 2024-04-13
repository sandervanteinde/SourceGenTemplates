using System.Text;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.Foreach.Conditions;

public class AccessModifierPredefinedConditionNode(AccessModifierNode accessModifier, Token token)
    : PredefinedConditionNode(PredefinedConditionNodeType.AccessModifier, token)
{
    public AccessModifierNode AccessModifier => accessModifier;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        accessModifier.AppendDebugString(sb);
    }
}