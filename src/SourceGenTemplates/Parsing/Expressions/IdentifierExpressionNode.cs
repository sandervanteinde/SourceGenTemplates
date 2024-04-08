using SourceGenTemplates.Tokenization;

namespace SourceGenTemplates.Parsing.Expressions;

public class IdentifierExpressionNode(IdentifierToken identifier) : ExpressionNode
{
    public IdentifierToken Identifier => identifier;
    public override ExpressionType Type => ExpressionType.Identifier;
    public override Token Token => identifier;
}