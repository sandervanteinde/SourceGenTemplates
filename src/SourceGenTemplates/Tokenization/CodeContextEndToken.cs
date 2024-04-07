using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class CodeContextEndToken(LinePositionSpan position) : Token(position)
{
    public override TokenType TokenType => TokenType.CodeContextEnd;
}