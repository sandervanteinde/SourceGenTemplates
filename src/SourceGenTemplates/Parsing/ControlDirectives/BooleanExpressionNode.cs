using System.Text;
using SourceGenTemplates.Parsing.Directives;
using SourceGenTemplates.Parsing.Foreach.Conditions;
using SourceGenTemplates.Parsing.VariableExpressions;

namespace SourceGenTemplates.Parsing.ControlDirectives;

public class BooleanExpressionNode(VariableExpressionNode variableExpression, ForeachConditionNode foreachConditionNode) : DirectiveNode(DirectiveNodeType.If)
{
    public VariableExpressionNode VariableExpression => variableExpression;
    public ForeachConditionNode ForeachCondition => foreachConditionNode;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        variableExpression.AppendDebugString(sb);
        sb.Append(" is ");
        foreachConditionNode.AppendDebugString(sb);
    }
}