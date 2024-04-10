using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class PublicToken(LinePositionSpan position) : Token(TokenType.Public, position);