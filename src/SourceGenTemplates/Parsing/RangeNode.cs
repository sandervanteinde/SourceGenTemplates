using System.Text;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing;

public class RangeNode(NumberToken startRange, NumberToken endRange) : Node
{
    public NumberToken StartRange => startRange;
    public NumberToken EndRange => endRange;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        sb.Append(startRange.Number);
        sb.Append("..");
        sb.Append(endRange.Number);
    }
}