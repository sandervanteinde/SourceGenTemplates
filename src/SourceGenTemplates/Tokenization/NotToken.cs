using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class NotToken(LinePositionSpan position) : Token(TokenType.Not, position);