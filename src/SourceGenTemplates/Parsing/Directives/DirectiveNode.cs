namespace SourceGenTemplates.Parsing.Directives;

public abstract class DirectiveNode : Node
{
    public abstract DirectiveNodeType Type { get; }
}