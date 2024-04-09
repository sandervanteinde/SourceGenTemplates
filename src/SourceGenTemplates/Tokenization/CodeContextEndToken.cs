using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class CodeContextEndToken(LinePositionSpan position) : Token(TokenType.CodeContextEnd, position);
