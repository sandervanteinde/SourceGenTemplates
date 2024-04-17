using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGenTemplates.Parsing.Foreach.Conditions;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Generation.Variables;

public class StringVariable(string value) : Variable(VariableKind.String), IVariableWithStringRepresentation
{
    public string String => value;

    public string GetCodeRepresentation(CompilationContext compilationContext)
    {
        return value;
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

    public override bool IsEqualToVariable(Variable rightValue)
    {
        return rightValue is StringVariable stringVariable && stringVariable.String == value;
    }
}