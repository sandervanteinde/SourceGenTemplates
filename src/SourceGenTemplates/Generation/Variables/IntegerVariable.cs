using SourceGenTemplates.Parsing.Foreach.Conditions;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Generation.Variables;

public class IntegerVariable(int value)
    : Variable(VariableKind.Integer)
        , IVariableWithStringRepresentation
{
    public string GetCodeRepresentation(CompilationContext compilationContext)
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