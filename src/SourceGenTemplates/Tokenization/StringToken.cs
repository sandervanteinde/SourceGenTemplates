using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class StringToken(LinePositionSpan position, string value) : Token(position)
{
    public override TokenType TokenType => TokenType.String;
    public string Value => value;
}