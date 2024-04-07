using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class DoubleDotToken(LinePositionSpan position) : Token(position)
{
    public override TokenType TokenType => TokenType.DoubleDot;
}