using System.Text;
using SourceGenTemplates.Parsing.Foreach;

namespace SourceGenTemplates.Parsing.TemplateInstructions;

public class ForeachTemplateInstructionNode(ForeachNode foreachNode) : TemplateInstructionNode(TemplateInstructionNodeType.Foreach)
{
    public ForeachNode ForeachNode { get; } = foreachNode;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        foreachNode.AppendDebugString(sb);
    }
}