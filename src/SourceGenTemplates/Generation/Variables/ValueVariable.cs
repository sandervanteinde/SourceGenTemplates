using SourceGenTemplates.Parsing.Foreach.Conditions;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Generation.Variables;

public class ValueVariable(object value) : Variable(VariableKind.Value)
{
    public override string GetCodeRepresentation()
    {
        return value.ToString();
    }

    public override bool MatchesCondition(ForeachConditionNode foreachCondition)
    {
        return false;
    }

    protected override Variable? TryAccessProperty(IdentifierToken identifier)
    {
        return null;
    }
}