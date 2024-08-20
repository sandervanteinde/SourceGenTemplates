namespace SourceGenTemplates.Parsing.TemplateInstructions;

public abstract class TemplateInstructionNode(TemplateInstructionNodeType type) : Node
{
    public TemplateInstructionNodeType Type { get; } = type;
}