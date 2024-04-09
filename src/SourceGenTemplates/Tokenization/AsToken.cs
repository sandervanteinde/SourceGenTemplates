using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class AsToken(LinePositionSpan position) : Token(TokenType.As, position);