using System.Text;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.Foreach.Conditions;

public class PartialForEachConditionNode(Token token) : LogicalOperationForeachCondition(ForeachConditionNodeType.Partial, token)
{
    public override int Precedence => 0;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        sb.Append("partial");
    }
}