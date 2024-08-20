using System.Text;
using SourceGenTemplates.Parsing.Expressions;

namespace SourceGenTemplates.Parsing.TemplateBlocks;

public class TemplateExpressionBlockNode(ExpressionNode expression) : TemplateBlockNode(TemplateBlockNodeType.Expression)
{
    public ExpressionNode Expression { get; } = expression;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        sb.Append("{{");
        Expression.AppendDebugString(sb);
        sb.Append("}}");
    }
}