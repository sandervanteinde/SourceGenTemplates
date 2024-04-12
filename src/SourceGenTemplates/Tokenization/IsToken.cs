using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class IsToken(LinePositionSpan position): Token(TokenType.Is, position);