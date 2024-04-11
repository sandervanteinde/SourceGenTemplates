using System.Text;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.Expressions;

public class StringExpressionNode(StringToken value) : ExpressionNode(ExpressionType.String, value)
{
    public StringToken Value => value;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        sb.Append($"\"{value.Value}\"");
    }
}