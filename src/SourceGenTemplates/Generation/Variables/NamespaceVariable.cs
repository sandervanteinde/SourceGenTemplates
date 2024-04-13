using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGenTemplates.Parsing.Foreach.Conditions;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Generation.Variables;

public class NamespaceVariable(BaseNamespaceDeclarationSyntax @namespace) : Variable(VariableKind.Namespace)
{
    public override string GetCodeRepresentation()
    {
        return @namespace.Name.ToFullString();
    }

    protected override bool MatchCondition(PredefinedConditionNode predefinedCondition)
    {
        return false;
    }

    protected override Variable? TryAccessProperty(IdentifierToken identifier)
    {
        return null;
    }
}