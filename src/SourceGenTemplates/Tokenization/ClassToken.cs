using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class ClassToken(LinePositionSpan position) : Token(TokenType.Class, position);
