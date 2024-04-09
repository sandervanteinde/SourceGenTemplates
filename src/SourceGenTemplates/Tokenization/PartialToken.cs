using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class PartialToken(LinePositionSpan position) : Token(TokenType.Partial, position);
