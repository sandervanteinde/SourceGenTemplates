﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGenTemplates.Parsing.Foreach.Conditions;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Generation.Variables;

public class IntegerVariable(int value)
    : Variable(VariableKind.Integer)
        , IVariableWithStringRepresentation
{
    public int Integer => value;

    public string GetCodeRepresentation(CompilationContext compilationContext)
    {
        return value.ToString();
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
        return rightValue is IntegerVariable integerVariable && integerVariable.Integer == value;
    }
}