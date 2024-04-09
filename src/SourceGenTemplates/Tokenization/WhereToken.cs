using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class WhereToken(LinePositionSpan position) : Token(TokenType.Where, position);
