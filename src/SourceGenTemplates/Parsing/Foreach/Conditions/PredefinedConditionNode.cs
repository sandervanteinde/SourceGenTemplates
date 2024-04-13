using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.Foreach.Conditions;

public abstract class PredefinedConditionNode(PredefinedConditionNodeType type, Token token) : Node
{
    public PredefinedConditionNodeType Type => type;
    public Token Token => token;
}