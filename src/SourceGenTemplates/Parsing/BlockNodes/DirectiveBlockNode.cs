using SourceGenTemplates.Parsing.Directives;

namespace SourceGenTemplates.Parsing.BlockNodes;

public class DirectiveBlockNode(DirectiveNode directive) : BlockNode(BlockNodeType.Directive)
{
    public DirectiveNode Directive => directive;
}