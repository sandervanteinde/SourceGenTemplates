namespace SourceGenTemplates.Parsing.Directives;

public class ForIDirectiveNode(ForINode forINode) : DirectiveNode(DirectiveNodeType.ForI)
{
    public ForINode ForINode => forINode;
}