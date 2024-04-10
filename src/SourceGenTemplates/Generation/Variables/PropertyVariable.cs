using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGenTemplates.Parsing.Foreach.Conditions;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Generation.Variables;

public class PropertyVariable(PropertyDeclarationSyntax property) : Variable(VariableKind.Property)
{
    public override string GetCodeRepresentation()
    {
        return property.Identifier.ToString();
    }

    public override bool MatchesCondition(ForeachConditionNode foreachCondition)
    {
        return false;
    }

    protected override Variable? TryAccessProperty(IdentifierToken identifier)
    {
        return identifier.Identifier switch
        {
            _ => null
        };
    }
}