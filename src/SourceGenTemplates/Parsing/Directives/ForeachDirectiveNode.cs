using System.Text;
using SourceGenTemplates.Parsing.Foreach;

namespace SourceGenTemplates.Parsing.Directives;

public class ForeachDirectiveNode(ForeachNode foreachNode) : DirectiveNode(DirectiveNodeType.Foreach)
{
    public ForeachNode ForeachNode => foreachNode;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        foreachNode.AppendDebugString(sb);
    }
}