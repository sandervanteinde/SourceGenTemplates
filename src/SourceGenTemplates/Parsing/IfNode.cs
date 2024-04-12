using System.Collections.Generic;
using System.Text;
using SourceGenTemplates.Parsing.BlockNodes;
using SourceGenTemplates.Parsing.Directives;
using SourceGenTemplates.Parsing.Foreach.Conditions;
using SourceGenTemplates.Parsing.VariableExpressions;

namespace SourceGenTemplates.Parsing;

public class IfNode(BooleanExpressionNode booleanExpressionNode, IReadOnlyCollection<BlockNode> blocks) : Node
{
    public BooleanExpressionNode BooleanExpression => booleanExpressionNode;
    public IReadOnlyCollection<BlockNode> Blocks => blocks;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        sb.Append("if ");
        booleanExpressionNode.AppendDebugString(sb);
        sb.Append(" end;");

        foreach (var block in blocks)
        {
            block.AppendDebugString(sb);
        }

        sb.Append(" end;");
    }
}

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