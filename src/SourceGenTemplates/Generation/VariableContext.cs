using System;
using System.Collections.Generic;

using SourceGenTemplates.Generation.Variables;
using SourceGenTemplates.Parsing;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Generation;

public class VariableContext
{
    private readonly Dictionary<string, Variable> _variables = [];

    public IDisposable? AddVariableToContext(IdentifierToken? identifier, Variable variable)
    {
        if (identifier is null)
        {
            return null;
        }

        string variableName = identifier.Identifier;
        _variables[variableName] = variable;
        return new DisposeVariableName(this, variableName);
    }

    public Variable GetOrThrow(IdentifierToken identifier)
    {
        var value = GetValue(identifier.Identifier);

        if (value is null)
        {
            throw new ParserException($"Variable with name {identifier.Identifier} was not defined", identifier);
        }
        
        return value;
    }
    
    public Variable? GetValue(string variableName)
    {
        _variables.TryGetValue(variableName, out var variableValue);
        return variableValue;
    }

    private class DisposeVariableName(VariableContext context, string variableName) : IDisposable
    {
        public void Dispose()
        {
            context._variables.Remove(variableName);
        }
    }
}