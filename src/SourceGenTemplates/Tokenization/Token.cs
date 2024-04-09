using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class Token(TokenType tokenType, LinePositionSpan position)
{
    public TokenType TokenType => tokenType;
    public LinePositionSpan Position => position;
}