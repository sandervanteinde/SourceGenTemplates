using System.Collections.Generic;

namespace SourceGenTemplates.Parsing;

public class FileNode(IReadOnlyCollection<BlockNode> blocks) : Node
{
    public IReadOnlyCollection<BlockNode> Blocks => blocks;
}