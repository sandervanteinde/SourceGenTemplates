using System.Text;
using SourceGenTemplates.Generation;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing;

public class NumberRangeValueNode(NumberToken numberToken) : RangeValueNode
{
    public NumberToken Number => numberToken;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        sb.Append(numberToken.Number);
    }

    public override int GetNumericValue(VariableContext variables)
    {
        return numberToken.Number;
    }
}