using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class CodeContextToken(LinePositionSpan position) : Token(position)
{
    public override TokenType TokenType => TokenType.CodeContextSwitch;
}