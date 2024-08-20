namespace SourceGenTemplates.Parsing.TemplateBlocks;

public abstract class TemplateBlockNode(TemplateBlockNodeType type) : Node
{
    public TemplateBlockNodeType Type { get; } = type;
}