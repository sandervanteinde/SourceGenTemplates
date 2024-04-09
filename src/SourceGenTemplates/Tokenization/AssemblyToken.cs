using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class AssemblyToken(LinePositionSpan position) : Token(TokenType.Assembly, position);