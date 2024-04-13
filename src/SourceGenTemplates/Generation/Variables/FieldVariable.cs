using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGenTemplates.Parsing.Foreach.Conditions;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Generation.Variables;

public class FieldVariable(FieldDeclarationSyntax fieldDeclaration)
    : Variable(VariableKind.Field)
        , IVariableWithStringRepresentation
{
    public string GetCodeRepresentation()
    {
        return fieldDeclaration.Declaration
            .Variables.First()
            .Identifier.ToFullString();
    }

    protected override bool MatchCondition(PredefinedConditionNode predefinedCondition)
    {
        return predefinedCondition.Type switch
        {
            PredefinedConditionNodeType.Partial => fieldDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword),
            PredefinedConditionNodeType.AccessModifier => ((AccessModifierPredefinedConditionNode)predefinedCondition).AccessModifier.IsApplicableFor(
                fieldDeclaration.Modifiers
            ),
            PredefinedConditionNodeType.Readonly => fieldDeclaration.Modifiers.Any(SyntaxKind.ReadOnlyKeyword)
        };
    }

    protected override Variable? TryAccessProperty(IdentifierToken identifier)
    {
        return null;
    }
}