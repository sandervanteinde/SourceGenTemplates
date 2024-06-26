﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGenTemplates.Parsing.Foreach.Conditions;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Generation.Variables;

public class FieldVariable(FieldDeclarationSyntax fieldDeclaration)
    : Variable(VariableKind.Field)
        , IVariableWithStringRepresentation
{
    public FieldDeclarationSyntax Field => fieldDeclaration;

    public string GetCodeRepresentation(CompilationContext compilationContext)
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
        return identifier.Identifier switch
        {
            "Type" => new TypeVariable(fieldDeclaration.Declaration.Type),
            _ => null
        };
    }

    protected override SyntaxList<AttributeListSyntax>? GetAttributes()
    {
        return fieldDeclaration.AttributeLists;
    }

    public override bool IsEqualToVariable(Variable rightValue)
    {
        return rightValue is FieldVariable field && field.Field == fieldDeclaration;
    }
}