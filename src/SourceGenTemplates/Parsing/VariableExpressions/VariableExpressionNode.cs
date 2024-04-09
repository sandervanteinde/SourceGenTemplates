namespace SourceGenTemplates.Parsing.VariableExpressions;

public abstract class VariableExpressionNode(VariableInsertionNodeType type)
{
    public VariableInsertionNodeType Type => type;
}