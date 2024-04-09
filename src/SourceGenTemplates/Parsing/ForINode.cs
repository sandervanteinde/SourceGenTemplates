using System.Collections.Generic;
using SourceGenTemplates.Parsing.BlockNodes;

namespace SourceGenTemplates.Parsing;

public class ForINode(IReadOnlyCollection<BlockNode> blocks, RangeNode range, IdentifierNode? identifier) : Node
{
    public IReadOnlyCollection<BlockNode> Blocks => blocks;
    public RangeNode Range => range;
    public IdentifierNode? Identifier => identifier;
}