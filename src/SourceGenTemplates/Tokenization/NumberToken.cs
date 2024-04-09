using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class NumberToken(LinePositionSpan position, int number) : Token(TokenType.Number, position)
{
    public int Number => number;
}