using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class StringToken(LinePositionSpan position, string value) : Token(TokenType.String, position)
{
    public string Value => value;
}