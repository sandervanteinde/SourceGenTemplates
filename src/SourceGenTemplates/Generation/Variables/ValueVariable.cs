using SourceGenTemplates.Parsing.Foreach.Conditions;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Generation.Variables;

public class ValueVariable(object value) : Variable(VariableKind.Value)
{
    public override string GetCodeRepresentation()
    {
        return value.ToString();
    }

    protected override bool MatchCondition(PredefinedConditionNode predefinedCondition)
    {
        return false;
    }

    protected override Variable? TryAccessProperty(IdentifierToken identifier)
    {
        return null;
    }
}