namespace SourceGenTemplates.Parsing.VariableInsertions;

public abstract class VariableInsertionNode(VariableInsertionNodeType type)
{
    public VariableInsertionNodeType Type => type;
}