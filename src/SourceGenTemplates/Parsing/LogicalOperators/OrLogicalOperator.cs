using System.Text;
using SourceGenTemplates.Parsing.Foreach.Conditions;

namespace SourceGenTemplates.Parsing.LogicalOperators;

public class OrLogicalOperator(ForeachConditionNode left, ForeachConditionNode right) : LogicalOperator(LogicalOperatorType.Or)
{
    public ForeachConditionNode Left => left;
    public ForeachConditionNode Right => right;

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