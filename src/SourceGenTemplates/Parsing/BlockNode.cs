namespace SourceGenTemplates.Parsing;

public abstract class BlockNode : Node
{
    public abstract BlockNodeType Type { get; }
}