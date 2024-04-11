using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.VariableExpressions;

public abstract class VariableExpressionNode(VariableExpressionNodeType type, Token token) : Node
{
    public VariableExpressionNodeType Type => type;
    public Token Token => token;
}