using System.Text;

namespace SourceGenTemplates.Parsing;

public class RangeNode(RangeValueNode startRange, RangeValueNode endRange) : Node
{
    public RangeValueNode StartRange => startRange;
    public RangeValueNode EndRange => endRange;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        startRange.AppendDebugString(sb);
        sb.Append("..");
        endRange.AppendDebugString(sb);
    }
}