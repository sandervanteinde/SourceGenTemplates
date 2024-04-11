using System.Text;

namespace SourceGenTemplates.Parsing.Directives;

public class ForIDirectiveNode(ForINode forINode) : DirectiveNode(DirectiveNodeType.ForI)
{
    public ForINode ForINode => forINode;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        forINode.AppendDebugString(sb);
    }
}