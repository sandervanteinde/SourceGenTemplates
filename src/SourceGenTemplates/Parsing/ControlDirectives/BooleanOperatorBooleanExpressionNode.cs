using System.Text;
using SourceGenTemplates.Parsing.LogicalOperators;

namespace SourceGenTemplates.Parsing.ControlDirectives;

public class BooleanOperatorBooleanExpressionNode(LogicalOperator logicalOperator) : BooleanExpressionNode(BooleanExpressionType.BooleanOperator)
{
    public LogicalOperator LogicalOperator => logicalOperator;

    protected internal override void AppendDebugString(StringBuilder sb)
    {
        logicalOperator.AppendDebugString(sb);
    }
}