using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public abstract class Token(LinePositionSpan position)
{
    public abstract TokenType TokenType { get; }
    public LinePositionSpan Position => position;
}