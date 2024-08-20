using System.Text;

namespace SourceGenTemplates.Parsing.TemplateInstructions;

public class ForITemplateInstructionNode(ForINode forINode) : TemplateInstructionNode(TemplateInstructionNodeType.ForI)
{
    public ForINode ForINode { get; } = forINode;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        forINode.AppendDebugString(sb);
    }
}