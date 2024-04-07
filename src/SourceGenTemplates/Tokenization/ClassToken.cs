using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class ClassToken(LinePositionSpan position) : Token(position)
{
    public override TokenType TokenType => TokenType.Class;
}