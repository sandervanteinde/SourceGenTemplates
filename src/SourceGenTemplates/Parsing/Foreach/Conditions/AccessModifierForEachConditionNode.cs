using System.Text;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.Foreach.Conditions;

public class AccessModifierForEachConditionNode(AccessModifierNode accessModifier, Token token)
    : LogicalOperationForeachCondition(ForeachConditionNodeType.AccessModifier, token)
{
    public AccessModifierNode AccessModifier => accessModifier;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        accessModifier.AppendDebugString(sb);
    }
}