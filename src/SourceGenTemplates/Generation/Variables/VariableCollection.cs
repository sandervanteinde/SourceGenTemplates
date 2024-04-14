using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGenTemplates.Parsing.Foreach.Conditions;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Generation.Variables;

public class VariableCollection(IReadOnlyCollection<Variable> variables) : Variable(VariableKind.Collection)
{
    public IReadOnlyCollection<Variable> Variables => variables;

    protected override bool MatchCondition(PredefinedConditionNode predefinedCondition)
    {
        return variables.All(variable => variable.MatchesCondition(predefinedCondition));
    }

    protected override Variable? TryAccessProperty(IdentifierToken identifier)
    {
        return null;
    }

    protected override SyntaxList<AttributeListSyntax>? GetAttributes()
    {
        return null;
    }
}