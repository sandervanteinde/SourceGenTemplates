namespace SourceGenTemplates.Parsing.VariableExpressions;

public abstract class VariableExpressionNode(VariableExpressionNodeType type)
{
    public VariableExpressionNodeType Type => type;
}