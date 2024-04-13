using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class ElseToken(LinePositionSpan position) : Token(TokenType.Else, position);