using System.Collections.Generic;

namespace SourceGenTemplates.Parsing.Foreach;

public class ForeachNode(ForeachType foreachType, ForeachTarget target, IdentifierNode? identifier, IReadOnlyCollection<BlockNode> blocks) : Node
{
    public ForeachType ForeachType => foreachType;
    public ForeachTarget ForeachTarget => target;
    public IdentifierNode? Identifier => identifier;
    public IReadOnlyCollection<BlockNode> Blocks => blocks;
}