using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class IfToken(LinePositionSpan position) : Token(TokenType.If, position);