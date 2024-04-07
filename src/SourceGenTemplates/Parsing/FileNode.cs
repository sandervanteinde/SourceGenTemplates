using System.Collections.Generic;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing;

public abstract class Node
{
    public abstract NodeType NodeType { get; }
}

public enum NodeType
{
    File,
    Block,
    Directive,
    FileName,
    Identifier,
    ForI,
    Range
}

public class FileNode(IReadOnlyCollection<BlockNode> blocks) : Node
{
    public override NodeType NodeType => NodeType.File;
    public IReadOnlyCollection<BlockNode> Blocks => blocks;
}

public abstract class BlockNode : Node
{
    public override NodeType NodeType => NodeType.Block;
}

public class CSharpBlockNode(SourceTextToken sourceText) : BlockNode
{
    public SourceTextToken SourceText => sourceText;
}

public class VariableInsertionBlockNode(IdentifierToken identifier) : BlockNode
{
    public IdentifierToken Identifier => identifier;
}

public class DirectiveBlockNode(DirectiveNode directive) : BlockNode
{
    public DirectiveNode Directive => directive;
}

public abstract class DirectiveNode : Node
{
    public override NodeType NodeType => NodeType.Directive;
}

public class FileNameDirectiveNode(FileNameNode fileName) : DirectiveNode
{
    public FileNameNode FileName => fileName;
}

public class ForIDirectiveNode(ForINode forINode) : DirectiveNode
{
    public ForINode ForINode => forINode;
}

public class ForINode(IReadOnlyCollection<BlockNode> blocks, RangeNode range, IdentifierNode? identifier) : Node
{
    public override NodeType NodeType => NodeType.ForI;
    public IReadOnlyCollection<BlockNode> Blocks => blocks;
    public RangeNode Range => range;
    public IdentifierNode? Identifier => identifier;
}

public class RangeNode(NumberToken startRange, NumberToken endRange) : Node
{
    public override NodeType NodeType => NodeType.Range;
    public NumberToken StartRange => startRange;
    public NumberToken EndRange => endRange;
}

public class FileNameNode(IdentifierNode identifier) : Node
{
    public override NodeType NodeType => NodeType.FileName;

    public IdentifierNode Identifier => identifier;
}

public class IdentifierNode(IdentifierToken identifier) : Node
{
    public override NodeType NodeType => NodeType.Identifier;
    public IdentifierToken Identifier => identifier;
}