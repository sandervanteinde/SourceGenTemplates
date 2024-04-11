using System.Text;
using SourceGenTemplates.Parsing.Expressions;

namespace SourceGenTemplates.Parsing;

public class FileNameNode(ExpressionNode expression) : Node
{
    public ExpressionNode Expression => expression;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        sb.Append("Filename ");
        expression.AppendDebugString(sb);
    }
}