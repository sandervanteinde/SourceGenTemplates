using SourceGenTemplates.Parsing.Foreach.Conditions;
using SourceGenTemplates.Parsing.LogicalOperators;
using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Generation.Variables;

public abstract class Variable(VariableKind kind)
{
    public VariableKind Kind => kind;

    public abstract string GetCodeRepresentation();

    protected abstract bool MatchCondition(LogicalOperationForeachCondition foreachCondition);

    protected abstract Variable? TryAccessProperty(IdentifierToken identifier);

    public bool MatchesCondition(ForeachConditionNode foreachCondition)
    {
        return foreachCondition switch
        {
            LogicalOperationForeachCondition logicalOperationForeachCondition => MatchCondition(logicalOperationForeachCondition),
            LogicalOperatorForeachConditionNode logicalOperatorForeachConditionNode => MatchLogicalOperatorForeachConditionNode(
                logicalOperatorForeachConditionNode.LogicalOperator
            ),
            _ => throw new ParserException("Invalid logical operator", foreachCondition.Token)
        };
    }

    public Variable AccessProperty(IdentifierToken identifier)
    {
        return TryAccessProperty(identifier)
            ?? throw new ParserException($"Cannot access property {identifier.Identifier} of variable of type {kind}", identifier);
    }

    private bool MatchLogicalOperatorForeachConditionNode(LogicalOperator op)
    {
        return op.Type switch
        {
            LogicalOperatorType.Or => OrOperator((OrLogicalOperator)op),
            LogicalOperatorType.And => AndOperator((AndLogicalOperator)op)
        };

        bool OrOperator(OrLogicalOperator or)
        {
            return MatchesCondition(or.Left) || MatchesCondition(or.Right);
        }

        bool AndOperator(AndLogicalOperator and)
        {
            return MatchesCondition(and.Left) && MatchesCondition(and.Right);
        }
    }
}