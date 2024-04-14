using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGenTemplates.Parsing.Foreach.Conditions;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Generation.Variables;

public class TypeVariable(TypeSyntax typeDeclarationSyntax)
    : Variable(VariableKind.Type)
        , IVariableWithStringRepresentation
{
    public string GetCodeRepresentation(CompilationContext compilationContext)
    {
        var model = compilationContext.GetSemanticModel(typeDeclarationSyntax.SyntaxTree);
        var symbolInfo = model.GetSymbolInfo(typeDeclarationSyntax)
            .Symbol as INamedTypeSymbol;

        return symbolInfo!.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
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
        return null;
    }
}