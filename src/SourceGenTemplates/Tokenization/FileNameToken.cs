using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class FileNameToken(LinePositionSpan position) : Token(position)
{
    public override TokenType TokenType => TokenType.FileNameDirective;
}