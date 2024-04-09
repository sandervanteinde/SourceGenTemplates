using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class ForToken(LinePositionSpan position) : Token(TokenType.For, position);
