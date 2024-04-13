using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.Text;
using SourceGenTemplates.Parsing.Foreach.Conditions;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Generation.Variables;

public class VariableCollection(IReadOnlyCollection<Variable> variables) : Variable(VariableKind.Collection)
{
    public IReadOnlyCollection<Variable> Variables => variables;

    public override string GetCodeRepresentation()
    {
        throw new ParserException("Attempted to parse a collection to code which is not possible", new LinePositionSpan());
    }

    protected override bool MatchCondition(PredefinedConditionNode predefinedCondition)
    {
        return variables.All(variable => variable.MatchesCondition(predefinedCondition));
    }

    protected override Variable? TryAccessProperty(IdentifierToken identifier)
    {
        return null;
    }
}