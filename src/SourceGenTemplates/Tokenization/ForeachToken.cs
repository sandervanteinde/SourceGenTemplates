using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class ForeachToken(LinePositionSpan position) : Token(TokenType.Foreach, position);
