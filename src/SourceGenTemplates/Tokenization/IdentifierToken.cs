using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class IdentifierToken(LinePositionSpan position, string identifier) : Token(TokenType.Identifier, position)
{
    public string Identifier => identifier;
}