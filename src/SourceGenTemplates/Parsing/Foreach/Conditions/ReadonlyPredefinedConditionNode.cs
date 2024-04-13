using System.Text;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.Foreach.Conditions;

public class ReadonlyPredefinedConditionNode(Token token) : PredefinedConditionNode(PredefinedConditionNodeType.Readonly, token)
{
    protected internal override void AppendDebugString(StringBuilder sb)
    {
        sb.Append(" readonly ");
    }
}