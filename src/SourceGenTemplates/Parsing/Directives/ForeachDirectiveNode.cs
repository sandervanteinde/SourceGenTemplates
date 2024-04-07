using SourceGenTemplates.Parsing.Foreach;

namespace SourceGenTemplates.Parsing.Directives;

public class ForeachDirectiveNode(ForeachNode foreachNode) : DirectiveNode
{
    public ForeachNode ForeachNode => foreachNode;
    public override DirectiveNodeType Type => DirectiveNodeType.Foreach;
}