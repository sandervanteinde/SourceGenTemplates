using System.Text;
using SourceGenTemplates.Parsing.ControlDirectives;

namespace SourceGenTemplates.Parsing.TemplateInstructions;

public class IfTemplateInstructionNode(IfNode @if) : TemplateInstructionNode(TemplateInstructionNodeType.If)
{
    public IfNode If { get; } = @if;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        If.AppendDebugString(sb);
    }
}