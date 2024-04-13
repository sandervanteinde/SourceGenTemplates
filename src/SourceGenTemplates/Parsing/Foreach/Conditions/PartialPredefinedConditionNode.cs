using System.Text;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.Foreach.Conditions;

public class PartialPredefinedConditionNode(Token token) : PredefinedConditionNode(PredefinedConditionNodeType.Partial, token)
{
    protected internal override void AppendDebugString(StringBuilder sb)
    {
        sb.Append("partial");
    }
}