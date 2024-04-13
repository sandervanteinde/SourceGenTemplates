using System.Text;
using SourceGenTemplates.Parsing.Foreach.Conditions;
using SourceGenTemplates.Parsing.VariableExpressions;

namespace SourceGenTemplates.Parsing.ControlDirectives;

public class SimpleComparisonBooleanExpressionNode(
    VariableExpressionNode variableExpression,
    PredefinedConditionNode predefinedConditionNode
) : BooleanExpressionNode(BooleanExpressionType.Simple)
{
    public VariableExpressionNode VariableExpression => variableExpression;
    public PredefinedConditionNode PredefinedCondition => predefinedConditionNode;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        variableExpression.AppendDebugString(sb);
        sb.Append(" is ");
        predefinedConditionNode.AppendDebugString(sb);
    }
}