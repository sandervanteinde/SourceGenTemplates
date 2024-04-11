using System.Text;
using SourceGenTemplates.Parsing.Foreach.Conditions;

namespace SourceGenTemplates.Parsing.LogicalOperators;

public class AndLogicalOperator(ForeachConditionNode left, ForeachConditionNode right)
    : LogicalOperator(LogicalOperatorType.And)
{
    public ForeachConditionNode Left => left;
    public ForeachConditionNode Right => right;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        left.AppendDebugString(sb);
        sb.Append(" and ");
        right.AppendDebugString(sb);
    }
}

public class NotLogicalOperator(ForeachConditionNode condition) : LogicalOperator(LogicalOperatorType.Not)
{
    public ForeachConditionNode Condition => condition;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        sb.Append("not ");
        condition.AppendDebugString(sb);
    }
}