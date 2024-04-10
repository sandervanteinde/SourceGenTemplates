using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class PrivateToken(LinePositionSpan position) : Token(TokenType.Private, position);