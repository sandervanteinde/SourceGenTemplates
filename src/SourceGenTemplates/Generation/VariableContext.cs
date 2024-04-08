using System;
using System.Collections.Generic;

using SourceGenTemplates.Parsing;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Generation;

public class VariableContext
{
    private readonly Dictionary<string, object> _variables = [];

    public IDisposable? AddVariableToContext<T>(IdentifierToken? identifier, T value)
        where T : notnull
    {
        if (identifier is null)
        {
            return null;
        }

        string variableName = identifier.Identifier;
        _variables[variableName] = value;
        return new DisposeVariableName(this, variableName);
    }

    public T GetOrThrow<T>(IdentifierToken identifier)
    {
        var value = GetValue<T>(identifier.Identifier);

        if (value is null)
        {
            throw new ParserException($"Variable with name {identifier.Identifier} was not defined", identifier);
        }
        
        return value;
    }
    
    public T? GetValue<T>(string variableName)
    {
        if (!_variables.TryGetValue(variableName, out var variableValue))
        {
            return default;
        }

        if (variableValue is T value)
        {
            return value;
        }
        
        return default;
    }

    private class DisposeVariableName(VariableContext context, string variableName) : IDisposable
    {
        public void Dispose()
        {
            context._variables.Remove(variableName);
        }
    }
}