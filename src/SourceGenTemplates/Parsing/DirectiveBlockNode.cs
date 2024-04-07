namespace SourceGenTemplates.Parsing.Directives;

public class DirectiveBlockNode(DirectiveNode directive) : BlockNode
{
    public DirectiveNode Directive => directive;
    public override BlockNodeType Type => BlockNodeType.Directive;
}