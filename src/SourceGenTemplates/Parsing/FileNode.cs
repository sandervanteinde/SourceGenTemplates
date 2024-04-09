using System.Collections.Generic;
using SourceGenTemplates.Parsing.BlockNodes;

namespace SourceGenTemplates.Parsing;

public class FileNode(IReadOnlyCollection<BlockNode> blocks) : Node
{
    public IReadOnlyCollection<BlockNode> Blocks => blocks;
}