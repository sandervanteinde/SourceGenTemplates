using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGenTemplates.Parsing.Foreach.Conditions;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Generation.Variables;

public abstract class Variable(VariableKind kind)
{
    public VariableKind Kind => kind;

    protected abstract bool MatchCondition(PredefinedConditionNode predefinedCondition);

    protected abstract Variable? TryAccessProperty(IdentifierToken identifier);

    public bool MatchesCondition(PredefinedConditionNode predefinedCondition)
    {
        return MatchCondition(predefinedCondition);
    }

    protected abstract SyntaxList<AttributeListSyntax>? GetAttributes();
    public abstract bool IsEqualToVariable(Variable rightValue);

    public Variable AccessProperty(IdentifierToken identifier)
    {
        return TryAccessProperty(identifier)
            ?? throw new ParserException($"Cannot access property {identifier.Identifier} of variable of type {kind}", identifier);
    }

    public bool HasAttributeWithName(StringToken stringToken)
    {
        var attributes = GetAttributes();
        return attributes?
                .SelectMany(attributeList => attributeList.ChildNodes())
                .OfType<AttributeSyntax>()
                .Any(attr => attr.Name.ToString() == stringToken.Value)
            ?? false;
    }
}