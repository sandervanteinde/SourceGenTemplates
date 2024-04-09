﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGenTemplates.Parsing.Foreach.Conditions;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Generation.Variables;

public class NamespaceVariable(BaseNamespaceDeclarationSyntax @namespace) : Variable(VariableKind.Namespace)
{
    public override string GetCodeRepresentation()
    {
        return @namespace.Name.ToFullString();
    }

    public override bool MatchesCondition(ForeachConditionNode foreachCondition)
    {
        return false;
    }

    protected override Variable? TryAccessProperty(IdentifierToken identifier)
    {
        return null;
    }
}