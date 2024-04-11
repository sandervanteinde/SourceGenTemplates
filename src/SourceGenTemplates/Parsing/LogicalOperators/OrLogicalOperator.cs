using System.Text;
using SourceGenTemplates.Parsing.Foreach.Conditions;

namespace SourceGenTemplates.Parsing.LogicalOperators;

public class OrLogicalOperator(ForeachConditionNode left, ForeachConditionNode right) : LogicalOperator(LogicalOperatorType.Or)
{
    public ForeachConditionNode Left => left;
    public ForeachConditionNode Right => right;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        left.AppendDebugString(sb);
        sb.Append(" or ");
        right.AppendDebugString(sb);
    }
}