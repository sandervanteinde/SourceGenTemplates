using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class SourceTextToken(LinePositionSpan position, string sourceText) : Token(TokenType.SourceText, position)
{
    public string SourceText => sourceText;
}