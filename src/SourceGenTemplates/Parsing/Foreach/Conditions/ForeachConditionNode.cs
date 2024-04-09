namespace SourceGenTemplates.Parsing.Foreach.Conditions;

public abstract class ForeachConditionNode(ForeachConditionNodeType type)
{
    public ForeachConditionNodeType Type => type;
}