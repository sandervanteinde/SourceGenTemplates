namespace SourceGenTemplates.Parsing.Foreach.Conditions;

public enum ForeachConditionNodeType
{
    Partial
}
public abstract class ForeachConditionNode(ForeachConditionNodeType type)
{
    public ForeachConditionNodeType Type => type;
}