using System.Text;
using SourceGenTemplates.Parsing.ControlDirectives;

namespace SourceGenTemplates.Parsing.LogicalOperators;

public class NotLogicalOperator(BooleanExpressionNode condition) : LogicalOperator(LogicalOperatorType.Not)
{
    public BooleanExpressionNode Condition => condition;

    public override int Precedence => 3;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        sb.Append('(');
        sb.Append("not ");
        condition.AppendDebugString(sb);
        sb.Append(')');
    }
}