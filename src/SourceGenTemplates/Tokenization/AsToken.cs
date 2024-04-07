using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class AsToken(LinePositionSpan position) : Token(position)
{
    public override TokenType TokenType => TokenType.As;
}