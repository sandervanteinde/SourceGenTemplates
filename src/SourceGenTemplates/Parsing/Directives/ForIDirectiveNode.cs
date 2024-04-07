namespace SourceGenTemplates.Parsing.Directives;

public class ForIDirectiveNode(ForINode forINode) : DirectiveNode
{
    public ForINode ForINode => forINode;
    public override DirectiveNodeType Type => DirectiveNodeType.ForI;
}