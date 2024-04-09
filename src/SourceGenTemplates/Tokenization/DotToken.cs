using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class DotToken(LinePositionSpan position) : Token(TokenType.Dot, position);
