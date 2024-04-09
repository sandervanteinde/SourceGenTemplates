using System;
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
            ForeachConditionNodeType.Partial => @class.Modifiers.Any(SyntaxKind.PartialKeyword)
        };
    }

    protected override Variable? TryAccessProperty(IdentifierToken identifier)
    {
        return identifier.Identifier == "Namespace"
            ? new NamespaceVariable(FindNamespace())
            : null;
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