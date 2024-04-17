using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGenTemplates.Parsing.Foreach.Conditions;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Generation.Variables;

public class NamespaceVariable(BaseNamespaceDeclarationSyntax @namespace)
    : Variable(VariableKind.Namespace)
        , IVariableWithStringRepresentation

{
    public BaseNamespaceDeclarationSyntax Namespace => @namespace;

    public string GetCodeRepresentation(CompilationContext compilationContext)
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

    protected override SyntaxList<AttributeListSyntax>? GetAttributes()
    {
        return @namespace.AttributeLists;
    }

    public override bool IsEqualToVariable(Variable rightValue)
    {
        return rightValue is NamespaceVariable namespaceVariable && namespaceVariable.Namespace == @namespace;
    }
}