using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class AssemblyToken(LinePositionSpan position) : Token(position)
{
    public override TokenType TokenType => TokenType.Assembly;
}