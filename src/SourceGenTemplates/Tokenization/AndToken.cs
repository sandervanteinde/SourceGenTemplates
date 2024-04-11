using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class AndToken(LinePositionSpan position) : Token(TokenType.And, position);