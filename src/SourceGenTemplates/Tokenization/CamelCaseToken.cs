using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class CamelCaseToken(LinePositionSpan position) : Token(TokenType.CamelCase, position);