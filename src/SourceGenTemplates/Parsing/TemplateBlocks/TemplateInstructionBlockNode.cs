using System.Text;
using SourceGenTemplates.Parsing.TemplateInstructions;

namespace SourceGenTemplates.Parsing.TemplateBlocks;

public class TemplateInstructionBlockNode(TemplateInstructionNode templateInstruction) : TemplateBlockNode(TemplateBlockNodeType.Instruction)
{
    public TemplateInstructionNode TemplateInstruction { get; } = templateInstruction;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        templateInstruction.AppendDebugString(sb);
    }
}