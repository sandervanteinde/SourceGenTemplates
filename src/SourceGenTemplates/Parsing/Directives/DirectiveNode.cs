namespace SourceGenTemplates.Parsing.Directives;

public enum DirectiveNodeType
{
    ForI,
    Filename,
    Foreach
}

public abstract class DirectiveNode : Node
{
    public abstract DirectiveNodeType Type { get; }
}