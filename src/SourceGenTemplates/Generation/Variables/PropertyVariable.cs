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

    protected override bool MatchCondition(PredefinedConditionNode predefinedCondition)
    {
        return predefinedCondition.Type switch
        {
            PredefinedConditionNodeType.Partial => false,
            PredefinedConditionNodeType.AccessModifier => ((AccessModifierPredefinedConditionNode)predefinedCondition).AccessModifier.IsApplicableFor(
                property.Modifiers
            )
        };
    }

    protected override Variable? TryAccessProperty(IdentifierToken identifier)
    {
        return identifier.Identifier switch
        {
            _ => null
        };
    }
}