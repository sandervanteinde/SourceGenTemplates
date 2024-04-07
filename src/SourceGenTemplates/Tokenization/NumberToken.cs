using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class NumberToken(LinePositionSpan position, int number) : Token(position)
{
    public override TokenType TokenType => TokenType.Number;
    public int Number => number;
}