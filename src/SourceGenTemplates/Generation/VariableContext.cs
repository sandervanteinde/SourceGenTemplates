using System;
using System.Collections.Generic;
using SourceGenTemplates.Generation.Variables;
using SourceGenTemplates.Parsing.Mutators;
using SourceGenTemplates.Parsing.VariableExpressions;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Generation;

public class VariableContext(CompilationContext compilationContext)
{
    private readonly Dictionary<string, Variable> _variables = [];

    public IDisposable? AddVariableToContext(IdentifierToken? identifier, Variable variable)
    {
        if (identifier is null)
        {
            return null;
        }

        var variableName = identifier.Identifier;
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

    public Variable GetOrThrow(VariableExpressionNode variableExpression)
    {
        return variableExpression.Type switch
        {
            VariableExpressionNodeType.VariableAccess => GetOrThrowVariableAccess((VariableExpressionNodeVariableAccess)variableExpression),
            VariableExpressionNodeType.PropertyAccess => GetOrThrowPropertyAccess((VariableExpressionNodePropertyAccess)variableExpression)
        };

        Variable GetOrThrowVariableAccess(VariableExpressionNodeVariableAccess variableAccess)
        {
            var variable = GetOrThrow(variableAccess.Identifier);
            return ApplyMutator(variable, variableAccess.Mutator);
        }

        Variable GetOrThrowPropertyAccess(VariableExpressionNodePropertyAccess propertyAccess)
        {
            var variable = GetOrThrow(propertyAccess.Identifier);
            var currentPropertyAccess = propertyAccess.PropertyAccess;

            while (currentPropertyAccess is not null)
            {
                variable = variable.AccessProperty(currentPropertyAccess.Identifier);
                currentPropertyAccess = currentPropertyAccess.PropertyAccess;
            }

            return ApplyMutator(variable, propertyAccess.Mutator);
        }

        Variable ApplyMutator(Variable variable, MutatorExpressionNode? mutator)
        {
            while (mutator is not null)
            {
                variable = mutator.Mutate(variable, compilationContext);
                mutator = mutator.Mutator;
            }

            return variable;
        }
    }

    private class DisposeVariableName(VariableContext context, string variableName) : IDisposable
    {
        public void Dispose()
        {
            context._variables.Remove(variableName);
        }
    }
}