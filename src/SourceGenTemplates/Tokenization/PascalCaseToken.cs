using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class PascalCaseToken(LinePositionSpan position) : Token(TokenType.PascalCase, position);