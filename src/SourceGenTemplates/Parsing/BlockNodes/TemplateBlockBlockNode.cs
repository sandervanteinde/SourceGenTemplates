using System.Text;
using SourceGenTemplates.Parsing.TemplateBlocks;

namespace SourceGenTemplates.Parsing.BlockNodes;

public class TemplateBlockBlockNode(TemplateBlockNode templateBlockNode) : BlockNode(BlockNodeType.Template)
{
    public TemplateBlockNode TemplateBlockNode { get; } = templateBlockNode;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        TemplateBlockNode.AppendDebugString(sb);
    }
}