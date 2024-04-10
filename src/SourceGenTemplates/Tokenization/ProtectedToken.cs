using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class ProtectedToken(LinePositionSpan position) : Token(TokenType.Protected, position);