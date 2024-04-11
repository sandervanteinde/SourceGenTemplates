using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class OrToken(LinePositionSpan position) : Token(TokenType.Or, position);