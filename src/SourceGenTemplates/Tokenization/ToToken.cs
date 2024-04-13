using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class ToToken(LinePositionSpan position) : Token(TokenType.To, position);