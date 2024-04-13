using SourceGenTemplates.Parsing.Foreach.Conditions;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Generation.Variables;

public class StringVariable(string value) : Variable(VariableKind.String), IVariableWithStringRepresentation
{
    public string GetCodeRepresentation()
    {
        return value;
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