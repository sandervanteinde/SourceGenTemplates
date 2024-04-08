namespace SourceGenTemplates.Parsing.BlockNodes;

public abstract class BlockNode : Node
{
    public abstract BlockNodeType Type { get; }
}