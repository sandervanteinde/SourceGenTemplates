using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class EndToken(LinePositionSpan position) : Token(position)
{
    public override TokenType TokenType => TokenType.End;
}