using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class IdentifierToken(LinePositionSpan position, string identifier) : Token(position)
{
    public string Identifier => identifier;
    public override TokenType TokenType => TokenType.Identifier;
}