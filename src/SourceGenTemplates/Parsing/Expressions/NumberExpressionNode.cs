using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.Expressions;

public class NumberExpressionNode(NumberToken value) : ExpressionNode(ExpressionType.Number, value)
{
    public NumberToken Value => value;
}