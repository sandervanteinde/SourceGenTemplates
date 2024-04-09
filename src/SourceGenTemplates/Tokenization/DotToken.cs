using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class DotToken(LinePositionSpan position) : Token(position)
{
    public override TokenType TokenType => TokenType.Dot;
}