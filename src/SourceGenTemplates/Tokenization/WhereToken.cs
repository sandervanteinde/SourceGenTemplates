using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class WhereToken(LinePositionSpan position) : Token(position)
{
    public override TokenType TokenType => TokenType.Where;
}