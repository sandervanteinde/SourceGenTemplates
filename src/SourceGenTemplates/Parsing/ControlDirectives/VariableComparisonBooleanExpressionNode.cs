using System.Text;
using SourceGenTemplates.Parsing.VariableExpressions;

namespace SourceGenTemplates.Parsing.ControlDirectives;

public class VariableComparisonBooleanExpressionNode(VariableExpressionNode left, VariableExpressionNode right)
    : BooleanExpressionNode(BooleanExpressionType.VariableComparison)
{
    public VariableExpressionNode Left => left;
    public VariableExpressionNode Right => right;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        left.AppendDebugString(sb);
        sb.Append(" is ");
        right.AppendDebugString(sb);
    }
}