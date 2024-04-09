namespace SourceGenTemplates.Parsing.Directives;

public abstract class DirectiveNode(DirectiveNodeType type) : Node
{
    public DirectiveNodeType Type => type;
}