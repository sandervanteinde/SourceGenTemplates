using System.Text;
using SourceGenTemplates.Parsing.ControlDirectives;

namespace SourceGenTemplates.Parsing.Directives;

public class IfDirectiveNode(IfNode @if) : DirectiveNode(DirectiveNodeType.If)
{
    public IfNode If => @if;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        @if.AppendDebugString(sb);
    }
}