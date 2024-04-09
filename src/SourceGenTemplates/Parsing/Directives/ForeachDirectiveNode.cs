using SourceGenTemplates.Parsing.Foreach;

namespace SourceGenTemplates.Parsing.Directives;

public class ForeachDirectiveNode(ForeachNode foreachNode) : DirectiveNode(DirectiveNodeType.Foreach)
{
    public ForeachNode ForeachNode => foreachNode;
}