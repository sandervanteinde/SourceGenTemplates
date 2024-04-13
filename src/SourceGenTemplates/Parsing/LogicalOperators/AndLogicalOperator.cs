using System.Text;
using SourceGenTemplates.Parsing.ControlDirectives;

namespace SourceGenTemplates.Parsing.LogicalOperators;

public class AndLogicalOperator(BooleanExpressionNode left, BooleanExpressionNode right)
    : LogicalOperator(LogicalOperatorType.And)
{
    public BooleanExpressionNode Left => left;
    public BooleanExpressionNode Right => right;

    public override int Precedence => 2;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        sb.Append('(');
        left.AppendDebugString(sb);
        sb.Append(" and ");
        right.AppendDebugString(sb);
        sb.Append(')');
    }
}