using System;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using SourceGenTemplates.Generation.Variables;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.Mutators;

public class MutatorExpressionNode(MutatorOperand operand, Token token, MutatorExpressionNode? mutator) : Node
{
    public MutatorOperand Operand => operand;
    public MutatorExpressionNode? Mutator => mutator;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        sb.Append(" to ");
        sb.Append(
            operand.ToString()
                .ToLower()
        );
        mutator?.AppendDebugString(sb);
    }

    public Variable Mutate(Variable variable, CompilationContext compilationContext)
    {
        return Operand switch
        {
            MutatorOperand.PascalCase => MakePascalCase(variable, compilationContext),
            MutatorOperand.CamelCase => MakeCamelCase(variable, compilationContext),
            MutatorOperand.EscapeKeyword => EscapeKeyword(variable, compilationContext)
        };
    }

    private Variable EscapeKeyword(Variable variable, CompilationContext compilationContext)
    {
        if (variable is not IVariableWithStringRepresentation variableWithStringRepresentation)
        {
            throw new ParserException("The variable could not be represented as a string and thus not converted to pascalcase", token);
        }

        var text = variableWithStringRepresentation.GetCodeRepresentation(compilationContext);

        if (SyntaxFacts.GetKeywordKind(text) != SyntaxKind.None)
        {
            text = $"@{text}";
        }

        return new StringVariable(text);
    }

    private Variable MakeCamelCase(Variable variable, CompilationContext compilationContext)
    {
        if (variable is not IVariableWithStringRepresentation variableWithStringRepresentation)
        {
            throw new ParserException("The variable could not be represented as a string and thus not converted to pascalcase", token);
        }

        var text = variableWithStringRepresentation.GetCodeRepresentation(compilationContext)
            .AsSpan();

        while (!text.IsEmpty && !char.IsLetter(text[0]))
        {
            text = text.Slice(1);
        }

        if (text.IsEmpty)
        {
            throw new ParserException("The variable contained no letters and could not be represented in pascalcase", token);
        }

        if (char.IsLower(text[0]))
        {
            return new StringVariable(text.ToString());
        }

        Span<char> copy = stackalloc char[text.Length];
        text.CopyTo(copy);

        copy[0] = char.ToLower(copy[0]);
        return new StringVariable(copy.ToString());
    }

    private Variable MakePascalCase(Variable variable, CompilationContext compilationContext)
    {
        if (variable is not IVariableWithStringRepresentation variableWithStringRepresentation)
        {
            throw new ParserException("The variable could not be represented as a string and thus not converted to pascalcase", token);
        }

        var text = variableWithStringRepresentation.GetCodeRepresentation(compilationContext)
            .AsSpan();

        while (!text.IsEmpty && !char.IsLetter(text[0]))
        {
            text = text.Slice(1);
        }

        if (text.IsEmpty)
        {
            throw new ParserException("The variable contained no letters and could not be represented in pascalcase", token);
        }

        if (char.IsUpper(text[0]))
        {
            return new StringVariable(text.ToString());
        }

        Span<char> copy = stackalloc char[text.Length];
        text.CopyTo(copy);

        copy[0] = char.ToUpper(copy[0]);
        return new StringVariable(copy.ToString());
    }
}