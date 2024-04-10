using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGenTemplates.Parsing.Foreach.Conditions;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Generation.Variables;

public class ClassVariable(ClassDeclarationSyntax @class) : Variable(VariableKind.Class)
{
    public override string GetCodeRepresentation()
    {
        return @class.Identifier.ToString();
    }

    public override bool MatchesCondition(ForeachConditionNode foreachCondition)
    {
        return foreachCondition.Type switch
        {
            ForeachConditionNodeType.Partial => @class.Modifiers.Any(SyntaxKind.PartialKeyword),
            ForeachConditionNodeType.AccessModifier => ((AccessModifierForEachConditionNode)foreachCondition).AccessModifier.IsApplicableFor(@class.Modifiers)
        };
    }

    protected override Variable? TryAccessProperty(IdentifierToken identifier)
    {
        return identifier.Identifier switch
        {
            "Namespace" => new NamespaceVariable(FindNamespace()),
            "Properties" => GetProperties(),
            _ => null
        };
    }

    private VariableCollection GetProperties()
    {
        var properties = @class.Members
            .OfType<PropertyDeclarationSyntax>()
            .Select(property => new PropertyVariable(property))
            .ToList();

        return new VariableCollection(properties);
    }

    private BaseNamespaceDeclarationSyntax FindNamespace()
    {
        var currentParent = @class.Parent;

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