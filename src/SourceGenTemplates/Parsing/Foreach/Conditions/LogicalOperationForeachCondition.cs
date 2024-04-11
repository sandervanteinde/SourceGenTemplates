using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.Foreach.Conditions;

public abstract class LogicalOperationForeachCondition(ForeachConditionNodeType type, Token token) : ForeachConditionNode(token)
{
    public ForeachConditionNodeType Type => type;
}