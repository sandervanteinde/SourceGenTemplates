using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.Expressions;

public abstract class ExpressionNode(ExpressionType type, Token token) : Node
{
    public ExpressionType Type => type;
    public Token Token => token;
}