using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.Expressions;

public class StringExpressionNode(StringToken value) : ExpressionNode(ExpressionType.String, value)
{
    public StringToken Value => value;
}