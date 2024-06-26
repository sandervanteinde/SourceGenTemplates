﻿using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGenTemplates.Parsing.Foreach.Conditions;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Generation.Variables;

public class ClassVariable(ClassDeclarationSyntax classDeclaration)
    : Variable(VariableKind.Class),
        IVariableWithStringRepresentation
{
    public ClassDeclarationSyntax Class => classDeclaration;

    public string GetCodeRepresentation(CompilationContext compilationContext)
    {
        return classDeclaration.Identifier.ToString();
    }

    protected override bool MatchCondition(PredefinedConditionNode predefinedCondition)
    {
        return predefinedCondition.Type switch
        {
            PredefinedConditionNodeType.Partial => classDeclaration.Modifiers.Any(SyntaxKind.PartialKeyword),
            PredefinedConditionNodeType.AccessModifier => ((AccessModifierPredefinedConditionNode)predefinedCondition).AccessModifier.IsApplicableFor(
                classDeclaration.Modifiers
            ),
            PredefinedConditionNodeType.Readonly => false
        };
    }

    protected override Variable? TryAccessProperty(IdentifierToken identifier)
    {
        return identifier.Identifier switch
        {
            "Namespace" => new NamespaceVariable(FindNamespace()),
            "Properties" => GetProperties(),
            "Fields" => GetFields(),
            _ => null
        };
    }

    protected override SyntaxList<AttributeListSyntax>? GetAttributes()
    {
        return classDeclaration.AttributeLists;
    }

    public override bool IsEqualToVariable(Variable rightValue)
    {
        return rightValue is ClassVariable classVariable && classVariable.Class == classDeclaration;
    }

    private VariableCollection GetFields()
    {
        var fields = classDeclaration.Members
            .OfType<FieldDeclarationSyntax>()
            .Select(field => new FieldVariable(field))
            .ToList();

        return new VariableCollection(fields);
    }

    private VariableCollection GetProperties()
    {
        var properties = classDeclaration.Members
            .OfType<PropertyDeclarationSyntax>()
            .Select(property => new PropertyVariable(property))
            .ToList();

        return new VariableCollection(properties);
    }

    private BaseNamespaceDeclarationSyntax FindNamespace()
    {
        var currentParent = classDeclaration.Parent;

        while (currentParent is not null)
        {
            if (currentParent is BaseNamespaceDeclarationSyntax namespaceDeclaration)
            {
                return namespaceDeclaration;
            }

            currentParent = currentParent.Parent;
        }

        throw new NotImplementedException();
    }
}