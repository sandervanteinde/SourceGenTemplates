using SourceGenTemplates.Parsing.Foreach.Conditions;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Generation.Variables;

public abstract class Variable(VariableKind kind)
{
    public VariableKind Kind => kind;

    public abstract string GetCodeRepresentation();

    public abstract bool MatchesCondition(ForeachConditionNode foreachCondition);

    protected abstract Variable? TryAccessProperty(IdentifierToken identifier);

    public Variable AccessProperty(IdentifierToken identifier)
    {
        return TryAccessProperty(identifier)
            ?? throw new ParserException($"Cannot access property {identifier.Identifier} of variable of type {kind}", identifier);
    }
}