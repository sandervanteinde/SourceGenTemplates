using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class ForeachToken(LinePositionSpan position) : Token(position)
{
    public override TokenType TokenType => TokenType.Foreach;
}