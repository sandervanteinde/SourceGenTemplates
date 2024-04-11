using System.Text;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.Expressions;

public class NumberExpressionNode(NumberToken value) : ExpressionNode(ExpressionType.Number, value)
{
    public NumberToken Value => value;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        sb.Append(value.Number);
    }
}