using System;
using System.Collections.Generic;

using SourceGenTemplates.Parsing;

namespace SourceGenTemplates.Generation;

public class VariableContext
{
    private readonly Dictionary<string, object> _variables = [];

    public IDisposable AddVariableToContext(string variableName, string value)
    {
        _variables.Add(variableName, value);
        return new DisposeVariableName(this, variableName);
    }

    public T GetOrThrow<T>(IdentifierNode identifier)
    {
        
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