using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class InToken(LinePositionSpan position) : Token(position)
{
    public override TokenType TokenType => TokenType.In;
}