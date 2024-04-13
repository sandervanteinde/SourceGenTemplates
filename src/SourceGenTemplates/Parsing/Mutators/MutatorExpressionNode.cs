using System;
using System.Text;
using SourceGenTemplates.Generation.Variables;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.Mutators;

public class MutatorExpressionNode(MutatorOperand operand, Token token, MutatorExpressionNode? mutator) : Node
{
    public MutatorOperand Operand => operand;
    public Token Token => token;
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

    public Variable Mutate(Variable variable)
    {
        return Operand switch
        {
            MutatorOperand.PascalCase => MakePascalCase(variable)
        };
    }

    private Variable MakePascalCase(Variable variable)
    {
        if (variable is not IVariableWithStringRepresentation variableWithStringRepresentation)
        {
            throw new ParserException("The variable could not be represented as a string and thus not converted to pascalcase", token);
        }

        var text = variableWithStringRepresentation.GetCodeRepresentation()
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