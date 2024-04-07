using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class SourceTextToken(LinePositionSpan position, string sourceText) : Token(position)
{
    public override TokenType TokenType => TokenType.SourceText;

    public string SourceText => sourceText;
}