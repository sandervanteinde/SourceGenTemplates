using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class EscapeKeywordToken(LinePositionSpan position) : Token(TokenType.EscapeKeyword, position);