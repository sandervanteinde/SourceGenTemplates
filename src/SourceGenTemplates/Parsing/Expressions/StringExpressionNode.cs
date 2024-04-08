using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.Expressions;

public class StringExpressionNode(StringToken value) : ExpressionNode
{
    public StringToken Value => value;
    public override ExpressionType Type => ExpressionType.String;
    public override Token Token => value;
}