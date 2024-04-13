using System.Collections.Generic;
using System.Linq;
using System.Text;
using SourceGenTemplates.Parsing.BlockNodes;
using SourceGenTemplates.Parsing.ControlDirectives;

namespace SourceGenTemplates.Parsing.Foreach;

public class ForeachNode(
    ForeachTarget target,
    IdentifierNode? identifier,
    IReadOnlyCollection<BlockNode> blocks,
    BooleanExpressionNode? condition) : Node
{
    public ForeachTarget ForeachTarget => target;
    public IdentifierNode? Identifier => identifier;
    public IReadOnlyCollection<BlockNode> Blocks => blocks;
    public BooleanExpressionNode? Condition => condition;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        sb.Append("foreach ");
        target.AppendDebugString(sb);

        if (identifier is not null)
        {
            sb.Append(" as ");
            identifier.AppendDebugString(sb);
        }

        if (condition is not null)
        {
            sb.Append(" where ");
            condition.AppendDebugString(sb);
        }

        foreach (var (block, i) in blocks.Select((b, i) => (b, i)))
        {
            if (i > 0)
            {
                sb.Append('|');
            }

            block.AppendDebugString(sb);
        }
    }
}