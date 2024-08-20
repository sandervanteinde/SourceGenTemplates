using System.Text;
using SourceGenTemplates.Parsing.VariableExpressions;

namespace SourceGenTemplates.Parsing.Expressions;

public class VariableExpressionExpressionNode(VariableExpressionNode variableExpressionNode) : ExpressionNode(ExpressionType.VariableExpression, variableExpressionNode.Token)
{
    public VariableExpressionNode VariableExpression { get; } = variableExpressionNode;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        variableExpressionNode.AppendDebugString(sb);
    }
}