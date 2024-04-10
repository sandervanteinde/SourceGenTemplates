using SourceGenTemplates.Generation;
using SourceGenTemplates.Generation.Variables;
using SourceGenTemplates.Parsing.VariableExpressions;

namespace SourceGenTemplates.Parsing.Foreach;

public class ForeachTargetVariableExpression(VariableExpressionNode variableExpression) : ForeachTarget(ForeachTargetType.VariableExpression, variableExpression.Token)
{
    public VariableExpressionNode VariableExpression => variableExpression;

    public override Variable GetVariableForType(CompilationContext compilation, VariableContext variables)
    {
        return variables.GetOrThrow(variableExpression);
    }
}