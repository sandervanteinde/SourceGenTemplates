using System.Text;
using SourceGenTemplates.Parsing.Foreach.Conditions;

namespace SourceGenTemplates.Parsing.LogicalOperators;

public class NotLogicalOperator(ForeachConditionNode condition) : LogicalOperator(LogicalOperatorType.Not)
{
    public ForeachConditionNode Condition => condition;

    public override int Precedence => 3;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        sb.Append('(');
        sb.Append("not ");
        condition.AppendDebugString(sb);
        sb.Append(')');
    }
}