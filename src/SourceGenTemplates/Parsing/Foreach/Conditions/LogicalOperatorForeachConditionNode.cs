using System.Text;
using SourceGenTemplates.Parsing.LogicalOperators;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.Foreach.Conditions;

public class LogicalOperatorForeachConditionNode(LogicalOperator logicalOperator, Token token)
    : ForeachConditionNode(token)
{
    public LogicalOperator LogicalOperator => logicalOperator;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        logicalOperator.AppendDebugString(sb);
    }
}