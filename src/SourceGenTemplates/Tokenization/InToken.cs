using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class InToken(LinePositionSpan position) : Token(TokenType.In, position);