using System.Collections.Generic;
using System.Text;
using SourceGenTemplates.Parsing.BlockNodes;

namespace SourceGenTemplates.Parsing.ControlDirectives;

public class IfNode(BooleanExpressionNode booleanExpressionNode, IReadOnlyCollection<BlockNode> blocks, ElseExpressionNode? elseExpressionNode) : Node
{
    public BooleanExpressionNode BooleanExpression => booleanExpressionNode;
    public IReadOnlyCollection<BlockNode> Blocks => blocks;
    public ElseExpressionNode? ElseExpression => elseExpressionNode;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        sb.Append("{{#if ");
        booleanExpressionNode.AppendDebugString(sb);
        sb.AppendLine("}}");

        foreach (var block in blocks)
        {
            block.AppendDebugString(sb);
        }

        elseExpressionNode?.AppendDebugString(sb);

        sb.AppendLine("{{/if}}");
    }
}

public enum ElseNodeType
{
    Else,
    ElseIf
}

public abstract class ElseExpressionNode(ElseNodeType type) : Node
{
    public ElseNodeType Type => type;
}

public class ElseElseExpressionNode(IReadOnlyCollection<BlockNode> blocks) : ElseExpressionNode(ElseNodeType.Else)
{
    public IReadOnlyCollection<BlockNode> Blocks => blocks;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        sb.AppendLine(" else;");

        foreach (var block in blocks)
        {
            block.AppendDebugString(sb);
        }
    }
}

public class ElseIfElseExpressionNode(IReadOnlyCollection<BlockNode> blocks, BooleanExpressionNode booleanExpression, ElseExpressionNode? elseExpressionNode)
    : ElseExpressionNode(ElseNodeType.ElseIf)
{
    public IReadOnlyCollection<BlockNode> Blocks => blocks;
    public BooleanExpressionNode BooleanExpression => booleanExpression;
    public ElseExpressionNode? ElseExpression => elseExpressionNode;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        sb.Append(" else if ");
        booleanExpression.AppendDebugString(sb);
        sb.AppendLine(";");

        foreach (var block in blocks)
        {
            block.AppendDebugString(sb);
        }

        elseExpressionNode?.AppendDebugString(sb);
    }
}