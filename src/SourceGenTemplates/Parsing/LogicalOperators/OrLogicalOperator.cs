using System.Text;
using SourceGenTemplates.Parsing.ControlDirectives;

namespace SourceGenTemplates.Parsing.LogicalOperators;

public class OrLogicalOperator(BooleanExpressionNode left, BooleanExpressionNode right) : LogicalOperator(LogicalOperatorType.Or)
{
    public BooleanExpressionNode Left => left;
    public BooleanExpressionNode Right => right;

    public override int Precedence => 1;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        sb.Append('(');
        left.AppendDebugString(sb);
        sb.Append(" or ");
        right.AppendDebugString(sb);
        sb.Append(')');
    }
}