using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class ForToken(LinePositionSpan position) : Token(position)
{
    public override TokenType TokenType => TokenType.For;
}