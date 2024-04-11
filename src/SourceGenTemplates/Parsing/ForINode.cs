using System.Collections.Generic;
using System.Linq;
using System.Text;
using SourceGenTemplates.Parsing.BlockNodes;

namespace SourceGenTemplates.Parsing;

public class ForINode(IReadOnlyCollection<BlockNode> blocks, RangeNode range, IdentifierNode? identifier) : Node
{
    public IReadOnlyCollection<BlockNode> Blocks => blocks;
    public RangeNode Range => range;
    public IdentifierNode? Identifier => identifier;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        sb.Append("for");
        range.AppendDebugString(sb);
        sb.Append("in");
        identifier?.AppendDebugString(sb);

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