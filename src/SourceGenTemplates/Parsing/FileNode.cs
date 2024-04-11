using System.Collections.Generic;
using System.Linq;
using System.Text;
using SourceGenTemplates.Parsing.BlockNodes;

namespace SourceGenTemplates.Parsing;

public class FileNode(IReadOnlyCollection<BlockNode> blocks) : Node
{
    public IReadOnlyCollection<BlockNode> Blocks => blocks;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        foreach (var (block, i) in blocks.Select((block, i) => (block, i)))
        {
            if (i > 0)
            {
                sb.Append('|');
            }

            sb.Append(block);
        }
    }
}