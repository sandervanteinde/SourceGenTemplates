namespace SourceGenTemplates.Parsing.Foreach.Conditions;

public class AccessModifierForEachConditionNode(AccessModifierNode accessModifier) : ForeachConditionNode(ForeachConditionNodeType.AccessModifier)
{
    public AccessModifierNode AccessModifier => accessModifier;
}