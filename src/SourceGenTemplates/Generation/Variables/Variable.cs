using SourceGenTemplates.Parsing.Foreach.Conditions;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Generation.Variables;

public abstract class Variable(VariableKind kind)
{
    public VariableKind Kind => kind;

    public abstract string GetCodeRepresentation();

    protected abstract bool MatchCondition(PredefinedConditionNode predefinedCondition);

    protected abstract Variable? TryAccessProperty(IdentifierToken identifier);

    public bool MatchesCondition(PredefinedConditionNode predefinedCondition)
    {
        return MatchCondition(predefinedCondition);
    }

    public Variable AccessProperty(IdentifierToken identifier)
    {
        return TryAccessProperty(identifier)
            ?? throw new ParserException($"Cannot access property {identifier.Identifier} of variable of type {kind}", identifier);
    }
}