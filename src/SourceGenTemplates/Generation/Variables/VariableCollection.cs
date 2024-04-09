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

    public override bool MatchesCondition(ForeachConditionNode foreachCondition)
    {
        return variables.All(variable => variable.MatchesCondition(foreachCondition));
    }

    public VariableCollection ApplyFilter(ForeachConditionNode condition)
    {
        var newVariables = variables
            .Where(variable => variable.MatchesCondition(condition))
            .ToList();
        
        
        return new VariableCollection(newVariables);
    }
    
    protected override Variable? TryAccessProperty(IdentifierToken identifier)
    {
        return null;
    }
}