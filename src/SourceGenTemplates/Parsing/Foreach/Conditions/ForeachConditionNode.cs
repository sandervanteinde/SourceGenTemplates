using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.Foreach.Conditions;

public abstract class ForeachConditionNode(Token token) : Node
{
    public Token Token => token;
    public abstract int Precedence { get; }
}