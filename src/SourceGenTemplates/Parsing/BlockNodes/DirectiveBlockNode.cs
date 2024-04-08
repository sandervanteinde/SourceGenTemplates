using SourceGenTemplates.Parsing.Directives;

namespace SourceGenTemplates.Parsing.BlockNodes;

public class DirectiveBlockNode(DirectiveNode directive) : BlockNode
{
    public DirectiveNode Directive => directive;
    public override BlockNodeType Type => BlockNodeType.Directive;
}