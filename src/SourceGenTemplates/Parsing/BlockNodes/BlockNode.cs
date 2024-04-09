namespace SourceGenTemplates.Parsing.BlockNodes;

public abstract class BlockNode(BlockNodeType type) : Node
{
    public BlockNodeType Type => type;
}