using System.Collections.Generic;
using SourceGenTemplates.Parsing.BlockNodes;
using SourceGenTemplates.Parsing.Foreach.Conditions;

namespace SourceGenTemplates.Parsing.Foreach;

public class ForeachNode(
    ForeachTarget target,
    IdentifierNode? identifier,
    IReadOnlyCollection<BlockNode> blocks,
    ForeachConditionNode? condition) : Node
{
    public ForeachTarget ForeachTarget => target;
    public IdentifierNode? Identifier => identifier;
    public IReadOnlyCollection<BlockNode> Blocks => blocks;
    public ForeachConditionNode? Condition => condition;
}