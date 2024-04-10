using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class EndToken(LinePositionSpan position) : Token(TokenType.End, position);