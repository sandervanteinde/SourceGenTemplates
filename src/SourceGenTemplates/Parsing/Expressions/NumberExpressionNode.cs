using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.Expressions;

public class NumberExpressionNode(NumberToken value) : ExpressionNode
{
    public NumberToken Value => value;
    public override ExpressionType Type => ExpressionType.Number;
    public override Token Token => value;
}