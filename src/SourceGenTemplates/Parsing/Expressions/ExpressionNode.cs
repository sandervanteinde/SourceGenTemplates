using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.Expressions;

public abstract class ExpressionNode
{
    public abstract ExpressionType Type { get; }
    public abstract Token Token { get; }
}