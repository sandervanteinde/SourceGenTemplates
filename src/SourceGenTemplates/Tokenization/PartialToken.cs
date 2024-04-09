using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class PartialToken(LinePositionSpan position) : Token(position)
{
    
    public override TokenType TokenType => TokenType.Partial;
}