using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class Token(TokenType type, LinePositionSpan position)
{
    public TokenType Type => type;
    public LinePositionSpan Position => position;
}