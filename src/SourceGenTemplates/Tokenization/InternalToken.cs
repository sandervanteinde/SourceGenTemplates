using Microsoft.CodeAnalysis.Text;

namespace SourceGenTemplates.Tokenization;

public class InternalToken(LinePositionSpan position) : Token(TokenType.Internal, position);